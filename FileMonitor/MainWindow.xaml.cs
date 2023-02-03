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
        static string programDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\FileMonitor";
        static string databasePath = $"{programDir}\\FMDB.sqlite";

        MonitoredFiles monitoredFiles = new MonitoredFiles(); // fires an event when the list of files have changed

        public MainWindow()
        {
            if (!File.Exists(JsonFile.storedPaths))
            {
                File.Create(JsonFile.storedPaths);
            }

            InitializeComponent();

            if (!File.Exists(databasePath))
            {
                Directory.CreateDirectory(programDir);
                DatabaseTables databaseTables = new DatabaseTables(databasePath);
                databaseTables.Create();
            }
            monitoredFiles.ShowAll(this);
        }

        /// <summary>
        /// Add new filepath to database. Notify the PropertyChanged event.
        /// </summary>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();

            if(newFile != "")
            {
                newFile = $"\'{newFile}\'"; // Surround with single quotes for SQL command
                // Create query and non-query objects
                SQLQuery query = new SQLQuery(databasePath, "source_file");
                SQLNonQuery nonQuery = new SQLNonQuery(databasePath);

                // Execute SQL commands
                int id = query.GetNextAvailableID("id");
                nonQuery.Insert("source_file", $"({id}, {newFile})");

                // Update UI by firing the PropertyChanged event
                monitoredFiles.FilesChangedEventHandler += MonitoredFiles_PropertyChanged;

                // Deprecated. Must remove once SQL tests pass
                JsonFile.WriteToFile(newFile);
                monitoredFiles.Files = JsonFile.GetDeserializedList();
            }
        }

        /// <summary>
        /// Update XAML TextBlocks when the PropertyChanged event is fired
        /// </summary>
        /// <remarks> Pass this MainWindow instance by reference to both method calls </remarks>
        private void MonitoredFiles_PropertyChanged(object? sender, FilesChangedEventArgs e)
        {
            monitoredFiles.ShowAll(this); 
            monitoredFiles.ShowChangedSinceBackup(this);
        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
