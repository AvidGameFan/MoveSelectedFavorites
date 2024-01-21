﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
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
            if (String.IsNullOrEmpty(folderBrowserSource.SelectedPath))
            {
                folderBrowserSource.SelectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Stable Diffusion UI");
                //Could go further and attempt to match the closest directory that matches the loaded favorites file.
                //Regex reg = new Regex( Date, minus the last 3 characters + @"*");
                //var dirs = Directory.GetDirectories(path)
                //                     .Where(path => reg.IsMatch(path))
                //                     .ToList();
                // -- generally should match just one, but could match more than one.

            }
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

        private void copyButton_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            List<String> copyList = new List<string>();
            //Validate
            if (favoritesList.Count == 0)
            {
                MessageBox.Show("First, load list of favorites.");
                return;
            }
            //For each image file in the source folder, 
            //  search contents for one of the listed seeds.
            //  If match found, perform action -- copy to destination folder
            SearchFiles("*.png", "tEXtseed", copyList);

            try 
            { 
                //for each file to process, copy
                foreach (var fileName2 in copyList)
                {
                    //Build destination path
                    FileInfo file = new FileInfo(fileName2);
                    string originalFileName = file.FullName;
                    string newFileName = Path.Combine(destinationFolder.Text, file.Name);
                    File.Copy(originalFileName, newFileName);  //could also use move
                    //TODO: list files to the window as they are copied, or at least report a count
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
            UseWaitCursor = false;
        }

        private void SearchFiles(string filePattern, string searchPattern, List<string> copyList)
        {
            string[] files = Directory.GetFiles(sourceFolder.Text, filePattern, SearchOption.TopDirectoryOnly);
            foreach (string fileName in files)
            {
                //load first part of file in binary -- we don't need the entire file
                try
                {
                    using (Stream str = File.Open(fileName, FileMode.Open))
                    {
                        using (var reader = new StreamReader(str))
                        {
                            char[] buffer = new char[2000];
                            if ((reader.Read(buffer, 0, 2000)) != -1)
                            {
                                //search for metadata string pattern, and copy until null character
                                string data = new String(buffer);
                                int location = data.IndexOf(searchPattern);
                                if (location == -1)
                                    continue;
                                string seedData = data.Substring(location + searchPattern.Length + 1, 15);
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

