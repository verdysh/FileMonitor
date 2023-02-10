using System.Windows;
using FileMonitor.Models;
using FileMonitor.Database;
using System;
using System.IO;
using System.Collections.Generic;

namespace FileMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            MonitoredFiles monitoredFiles = new MonitoredFiles();
            List<string> allFiles = monitoredFiles.AllFiles;
            ShowFiles(allFiles);
        }

        /// <summary>
        /// A method to display all monitored files in the UI
        /// </summary>
        /// <param name="files"> A list of files to display </param>
        private void ShowFiles(List<string> files)
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
                //// Surround with single quotes for SQL command
                //newFile = $"\'{newFile}\'"; 
                //// Create query and non-query objects
                //SQLQuery query = new SQLQuery(databasePath, "source_file");
                //SQLNonQuery nonQuery = new SQLNonQuery(databasePath);

                //// Execute SQL commands
                //int id = query.GetNextAvailableID("id");
                //nonQuery.Insert("source_file", $"({id}, {newFile})");

                // fires an event when the list of files have changed
                MonitoredFiles monitoredFiles = new MonitoredFiles(); 

                // Update UI by firing the PropertyChanged event
                monitoredFiles.FilesChangedEventHandler += MonitoredFiles_PropertyChanged;

                // Remove ;ogic once SQL tests pass
                JsonFile.WriteToFile(newFile);
                monitoredFiles.AllFiles = JsonFile.GetDeserializedList();
            }
        }

        /// <summary>
        /// Update XAML TextBlocks when the PropertyChanged event is fired
        /// </summary>
        /// <param name="sender"> object sender arg </param>
        /// <param name="e"> e.NewFiles contains the updated file list </param>
        private void MonitoredFiles_PropertyChanged(object? sender, FilesChangedEventArgs e)
        {
            if(e.OldFiles != e.NewFiles)
            {
                ShowFiles(e.NewFiles);
            }
        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
