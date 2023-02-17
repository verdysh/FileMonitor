using System.Windows;
using FileMonitor.Models;
using FileMonitor.Database;
using System;
using System.IO;
using System.Collections.Generic;
using FileMonitor.Tests;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FileMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MonitoredFiles sourceFileView = new MonitoredFiles();

        public static readonly string programDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\FileMonitor";
        public static readonly string databasePath = $"{programDir}\\FMDB.sqlite";

        public MainWindow()
        {
            // Remove logic once SQL tests pass
            if (!File.Exists(JsonFile.storedPaths))
            {
                File.Create(JsonFile.storedPaths);
            }

            InitializeComponent();

            if (!File.Exists(databasePath))
            {
                Directory.CreateDirectory(programDir);
                DatabaseBuilder database = new DatabaseBuilder();
                database.Build();
            }
            ReadOnlyObservableCollection<string> allFiles = sourceFileView.AllFiles;
            ShowFiles(allFiles);
        }

        /// <summary>
        /// A method to display all monitored files in the UI
        /// </summary>
        /// <param name="files"> A list of files to display </param>
        private void ShowFiles(ReadOnlyObservableCollection<string> files)
        {
            string result = "";
            for (int i = 0; i < files.Count; i++)
            {
                result += files[i] + "\n";
            }
            this.FilesDisplayed.Text = result;
        }

        /// <summary>
        /// A method to execute when the AddNewFile button is clicked
        /// </summary>
        /// <remarks>
        /// This method writes the new file to the SQLite database, and subscribes to the FilesChangedEventHandler.
        /// This event handler will receive a method in its invocation list to update the program UI
        /// </remarks>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();

            if(newFile != "")
            {
                //sourceFileView.AllFiles.Add(newFile);
            }
        }

        /// <summary>
        /// Update XAML TextBlocks when the PropertyChanged event is fired
        /// </summary>
        /// <param name="sender"> object sender arg </param>
        /// <param name="e"> e.NewFiles contains the updated file list </param>
        private void MonitoredFiles_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
