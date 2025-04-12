using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MoveSelectedFavorites
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(MainForm_DragEnter);
            this.DragDrop += new DragEventHandler(MainForm_DragDrop);


            openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Select the favorites text file",
                Filter = "Text files (*.txt)|*.txt",
                Title = "Open text file"
            };

            favoritesList = new List<string>();

            log.Text += Environment.NewLine + DateTime.Now + ": Starting" + Environment.NewLine;
        }
        private List<string> favoritesList;
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                favoritesList = new List<string>();

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                //foreach (string filePath in files)  -- currently, cannot handle multiple files -- just pick one
                if (files.Any())
                {
                    //Console.WriteLine(filePath);
                    //Load file, save list of seeds
                    try
                    {
                        ProcessFavoritesFile(files[0], File.OpenRead(files[0]));
                    }
                    catch
                    {
                        log.Text += Environment.NewLine + DateTime.Now + ": Unable to load file " + files[0] + Environment.NewLine
                            + "Only Drag-and-drop favorites text files containing seeds." + Environment.NewLine;
                    }
                }
            }
        }
        void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var folderDialog = new FolderBrowserDialog();
            DialogResult result = folderBrowserDestination.ShowDialog();

            if (result == DialogResult.OK)
            {
                destinationFolder.Text = folderBrowserDestination.SelectedPath;
            }
            else
            {
                //Operation cancelled
            }
        }

        private void selectSource_Click(object sender, EventArgs e)
        {
            //Reset any status fields
            labelCopyCount.Text = String.Empty;
            folderBrowserSource.SelectedPath = GetSourcePath();
            DialogResult result = folderBrowserSource.ShowDialog();

            if (result == DialogResult.OK)
            {
                sourceFolder.Text = folderBrowserSource.SelectedPath;
            }
            else
            {
                //Operation cancelled
            }
        }

        private string GetSourcePath()
        {
            string result = folderBrowserSource.SelectedPath;
            if (String.IsNullOrEmpty(folderBrowserSource.SelectedPath))
            {
                result = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Stable Diffusion UI");
            }
            return result;
        }

        private async void copyButton_Click(object sender, EventArgs e)
        {
            labelCopyCount.Text = string.Empty;

            //Validate
            if (favoritesList.Count == 0)
            {
                MessageBox.Show("First, load list of favorites.");
                return;
            }

            Application.UseWaitCursor = true;
            //The copy can be time-consuming, so show the wait cursor and spin off a thread.

            try
            {
                await Task.Run(() =>
                {
                    List<string> copyList = new List<string>();

                    //For each image file in the source folder, 
                    //  search contents for one of the listed seeds.
                    //  If match found, perform action -- copy to destination folder
                    SearchFiles("*.png", "tEXtseed\0", copyList);
                    SearchFiles("*.webp", "\"seed\": ", copyList);
                    SearchFiles("*.jpeg", "\"seed\": ", copyList);

                    int imageCount = 0;
                    //for each file to process, copy
                    foreach (var fileName2 in copyList)
                    {
                        bool success = false;
                        success = CopyImageFile(fileName2);
                        if (success)
                        {
                            //list files to the window as they are copied, or at least report a count

                            imageCount++;
                            Invoke(new Action(() =>
                            {
                                labelCopyCount.Text = $"{imageCount} files copied out of {copyList.Count}";
                            }));

                            //If there is a corresponding JSON or TXT file, copy that too
                            string textFile = fileName2;
                            textFile = fileName2.Substring(0, fileName2.IndexOf('.', fileName2.Length - 5) + 1) + "json";
                            CopyImageFile(textFile);
                            textFile = fileName2.Substring(0, fileName2.IndexOf('.', fileName2.Length - 5) + 1) + "txt";
                            CopyImageFile(textFile);
                        }
                    }

                    //All files should be copied at this point. Can rename folder.
                    if (checkRenSource.Checked)
                    {
                        //If no files were copied, probably got the wrong directory -- don't rename.
                        if (imageCount > 0)
                        {
                            Directory.Move(sourceFolder.Text, sourceFolder.Text.TrimEnd('\\') + suffix.Text); //use TrimEnd() to remove unnecessary ending slashes
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                log.Text += Environment.NewLine + DateTime.Now + ": No files copied - source folder not renamed." + Environment.NewLine;
                            }));
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
            finally
            {
                Application.UseWaitCursor = false;
            }

            //Done.  

            log.Text += Environment.NewLine + DateTime.Now + ": " + Environment.NewLine;
            log.Text += "List " + favoritesLabel.Text + Environment.NewLine;
            log.Text += $"Copied files from {sourceFolder.Text}" + Environment.NewLine;

            //sourceFolder.Text = String.Empty; //Could clear the source folder, as reproducing the copy will produce no further results.
            if (labelCopyCount.Text == string.Empty) //if empty, we must have failed to copy.  Could happen if attempting to copy a 2nd time on the same folder.
            {
                labelCopyCount.Text = "Images to copy not found, or other error.";
            }
            log.Text += labelCopyCount.Text + Environment.NewLine;

            Application.UseWaitCursor = false;
        }

        /// <summary>
        /// Copy file without overwrite
        /// </summary>
        /// <param name="fileName2">filename, with full path</param>
        /// <returns>true if copy successful, false if destination exists (no copy)</returns>
        private bool CopyImageFile(string fileName2)
        {
            //Build destination path
            FileInfo file = new FileInfo(fileName2);
            string originalFileName = file.FullName;
            //if source name doesn't exist, don't bother trying to copy.  This will happen with json/txt files.
            if (!File.Exists(originalFileName))
            {
                return false;
            }
            string newFileName = Path.Combine(destinationFolder.Text, file.Name);
            //if file exists, don't copy, and return status of false
            if (File.Exists(newFileName))
            {
                return false;
            }
            File.Copy(originalFileName, newFileName);  //could also use move
            return true;
        }

        /// <summary>
        /// Search Files for metadata
        /// </summary>
        /// <param name="filePattern"></param>
        /// <param name="searchPattern"></param>
        /// <param name="copyList"></param>
        /// <remarks>Shouldn't need to search the entire file for .png or .jpeg, but you do for .webp.</remarks>
        private void SearchFiles(string filePattern, string searchPattern, List<string> copyList)
        {
            string[] files = Directory.GetFiles(sourceFolder.Text, filePattern, SearchOption.TopDirectoryOnly);
            foreach (string fileName in files)
            {
                //load first part of file in binary -- we don't need the entire file
                try
                {
                    StringBuilder sb = new StringBuilder();
                    using (Stream str = File.Open(fileName, FileMode.Open))
                    {
                        using (var reader = new StreamReader(str))
                        {
                            char[] buffer = new char[2000];
                            int bytesRead = 0;
                            int fileIndex = 0;
                            while ((bytesRead = reader.Read(buffer, 0, 2000)) > 0)
                            {
                                bool decodingUnicode = false;
                                int unicodeIndex = 0;
                                fileIndex += bytesRead;

                                //search for metadata string pattern, and copy until null character
                                string data = new String(buffer);
                                //if (fileName.Substring(fileName.Length-4)=="webp")
                                //{ 
                                //    data = Encoding.Unicode.GetString(data.Select(c => (byte)c).ToArray());    //convert char array to byte array
                                //}
                                //else
                                if (fileName.Substring(fileName.Length - 4) == "jpeg" || fileName.Substring(fileName.Length - 4) == "webp")
                                {
                                    //TODO: slight chance of buffer not big enough here
                                    //Need to find the UNICODE block before decoding as UNICODE.
                                    unicodeIndex = data.IndexOf("UNICODE");
                                    if (decodingUnicode || unicodeIndex >= 0)
                                    {
                                        data = Encoding.Unicode.GetString(data.Substring(unicodeIndex + "UNICODE".Length).Select(c => (byte)c).ToArray());    //convert char array to byte array
                                        decodingUnicode = true;    //once we're decoding UNICODE, need to keep going
                                    }
                                }
                                sb.Remove(0, Math.Max(0, sb.Length - searchPattern.Length));
                                //need to look just beyond the buffer, so as not to miss the pattern in the case it overlaps with the buffer size.
                                sb.Append(data);
                                data = sb.ToString();
                                int location = data.IndexOf(searchPattern, StringComparison.OrdinalIgnoreCase);
                                if (location == -1)
                                    continue; //keep reading until we're done with the file
                                string seedData = data.Substring(location + searchPattern.Length, 15);
                                string seed = String.Empty;
                                foreach (char c in seedData)
                                {
                                    //TODO: it is possible that extra numeric characters may appear at the end, and thus the occasional file won't copy.  Need to revisit the file format.
                                    if (c < '0' || c > '9')
                                        break;
                                    seed += c;
                                }
                                if (favoritesList.IndexOf(seed) != -1)  //Found!
                                {
                                    //if metadata string matches any seed in our list, then copy
                                    //Need to save list of files to process after we're done reading them.
                                    copyList.Add(fileName);
                                }
                                break;  //get next file
                            }
                        }
                    }
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
                //if seed was never found in file, may need to fall back on the JSON or TXT sidecar file.
            }
        }

        private OpenFileDialog openFileDialog1;
        //see: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-open-files-using-the-openfiledialog-component?view=netframeworkdesktop-4.8
        private void favoritesButton_Click(object sender, EventArgs e)
        {
            labelCopyCount.Text = string.Empty;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ProcessFavoritesFile(openFileDialog1.FileName, openFileDialog1.OpenFile());
            }
        }

        private void ProcessFavoritesFile(string filePath, Stream fileStream)
        {
            try
            {
                favoritesList = new List<string>();

                using (Stream str = fileStream)
                {
                    using (var reader = new StreamReader(str))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            favoritesList.Add(line);
                        }
                        favoritesLabel.Text = "Loaded: " + filePath;
                        //Process.Start("notepad.exe", filePath);
                    }
                }

                //attempt to match the closest directory that matches the loaded favorites file.
                FileInfo file = new FileInfo(filePath);
                Regex reg = new Regex("favoriteslist-([0-9]+)*");
                var match = reg.Match(file.Name);
                string time = match.Groups[1].Value;
                //Regex reg = new Regex( Date, minus the last 3 characters + @"*");
                if (time.Length < 3)  //if no date found in the name, we can't process further - just exit
                {
                    return;
                }
                reg = new Regex(time.Substring(0, time.Length - 3) + "*");

                //usually, expect saved image numbered folders in the default folder, but in the case of having moved to another drive, allow searching that other drive.  Still requires parent name to be "Stable Diffusion UI".
                var rootFolder = sourceFolder.Text;
                //If there is already a folder listed in the source path field, use that as the root to find the next folder (as long as it contains the text Stable Diffusion UI)   
                if (!String.IsNullOrEmpty(rootFolder) && rootFolder.IndexOf("Stable Diffusion UI")>0)
                {
                    rootFolder = rootFolder.Substring(0, rootFolder.IndexOf("Stable Diffusion UI"));
                }
                //Else, use the user profile folder as the root.
                else
                {
                    rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                }

                    var dirs = Directory.GetDirectories(Path.Combine(rootFolder, "Stable Diffusion UI"))
                                             .Where(path => reg.IsMatch(path))
                                             .ToList();
                // -- generally should match just one, but could match more than one.
                if (dirs.Count == 1)
                {
                    sourceFolder.Text = dirs[0];
                }
            }
            catch (SecurityException ex)
            {
                MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
        }

        private void checkRenSource_CheckedChanged(object sender, EventArgs e)
        {
            suffix.Enabled = checkRenSource.Checked;
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            log.Width = this.ClientSize.Width - 467;
        }
    }
}

