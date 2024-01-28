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
        }
        private List<string> favoritesList;
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                favoritesList = new List<string>();

                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string filePath in files)
                {
                    Console.WriteLine(filePath);
                    //Load file, save list of seeds
                    
                    //TODO: process list(s)
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

        private void copyButton_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            labelCopyCount.Text = string.Empty;
            List <String> copyList = new List<string>();
            //Validate
            if (favoritesList.Count == 0)
            {
                MessageBox.Show("First, load list of favorites.");
                return;
            }
            //For each image file in the source folder, 
            //  search contents for one of the listed seeds.
            //  If match found, perform action -- copy to destination folder
            SearchFiles("*.png", "tEXtseed\0", copyList);
            SearchFiles("*.webp", "\"seed\": ", copyList);
            SearchFiles("*.jpeg", "\"seed\": ", copyList);
            try 
            {
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
                        labelCopyCount.Text = imageCount + " files copied out of "+copyList.Count;

                        //If there is a corresponding JSON or TXT file, copy that too
                        string textFile = fileName2;
                        textFile = fileName2.Substring(0, fileName2.IndexOf('.', fileName2.Length - 5) + 1) + "json";
                        CopyImageFile(textFile);
                        textFile = fileName2.Substring(0, fileName2.IndexOf('.', fileName2.Length - 5) + 1) + "txt";
                        CopyImageFile(textFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
            UseWaitCursor = false;
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
        /// 
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
                            while ((bytesRead = reader.Read(buffer, 0, 2000)) >0)
                            {
                                bool decodingUnicode = false;
                                fileIndex += bytesRead;

                                //search for metadata string pattern, and copy until null character
                                string data = new String(buffer);
                                if (fileName.Substring(fileName.Length-4)=="webp")
                                { 
                                    data = Encoding.Unicode.GetString(data.Select(c => (byte)c).ToArray());    //convert char array to byte array
                                }
                                else
                                if (fileName.Substring(fileName.Length - 4) == "jpeg")
                                {
                                    //TODO: slight chance of buffer not big enough here
                                    //Need to find the UNICODE block before decoding as UNICODE.
                                    int unicodeIndex = data.IndexOf("UNICODE");
                                    if (decodingUnicode || unicodeIndex >= 0)
                                    {
                                        data = Encoding.Unicode.GetString(data.Substring(unicodeIndex + "UNICODE".Length).Select(c => (byte)c).ToArray());    //convert char array to byte array
                                        decodingUnicode = true;    //once we're decoding UNICODE, need to keep going
                                    }
                                }
                                sb.Remove(0, Math.Max(0,sb.Length - searchPattern.Length));
                                //need to look just beyond the buffer, so as not to miss the pattern in the case it overlaps with the buffer size.
                                sb.Append(data);
                                data = sb.ToString();
                                int location = data.IndexOf(searchPattern,StringComparison.OrdinalIgnoreCase);
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
            }
        }

        private OpenFileDialog openFileDialog1;
        //see: https://learn.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-open-files-using-the-openfiledialog-component?view=netframeworkdesktop-4.8
        private void favoritesButton_Click(object sender, EventArgs e)
        {
            labelCopyCount.Text = string.Empty;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    favoritesList = new List<string>();
                    var filePath = openFileDialog1.FileName;
                    using (Stream str = openFileDialog1.OpenFile())
                    {
                        using (var reader = new StreamReader(str))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                favoritesList.Add(line);
                            }
                            favoritesLabel.Text = "Loaded: " + openFileDialog1.FileName;
                            //Process.Start("notepad.exe", filePath);
                        }
                    }

                    //attempt to match the closest directory that matches the loaded favorites file.
                    FileInfo file = new FileInfo(openFileDialog1.FileName);
                    Regex reg = new Regex("favoriteslist-([0-9]+)*");
                    var match= reg.Match(file.Name);
                    string time = match.Groups[1].Value;
                    //Regex reg = new Regex( Date, minus the last 3 characters + @"*");
                    reg = new Regex(time.Substring(0,time.Length-3)+"*");
                    var dirs = Directory.GetDirectories(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Stable Diffusion UI"))
                                         .Where(path => reg.IsMatch(path))
                                         .ToList();
                    // -- generally should match just one, but could match more than one.
                    if (dirs.Count==1)
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
        }
    }
}

