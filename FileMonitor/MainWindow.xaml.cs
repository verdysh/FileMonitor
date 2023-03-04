using System.Windows;
using FileMonitor.Models;
using FileMonitor.Database;
using System;
using System.IO;

namespace FileMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MonitoredFiles monitoredFiles;

        public static readonly string programDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\FileMonitor";
        public static readonly string databasePath = $"{programDir}\\FMDB.sqlite";

        public MainWindow()
        {
            if (!File.Exists(databasePath))
            {
                Directory.CreateDirectory(programDir);
                DatabaseBuilder database = new DatabaseBuilder();
                database.Build();
            }

            // Remove logic once SQL tests pass
            if (!File.Exists(JsonFile.storedPaths))
            {
                File.Create(JsonFile.storedPaths);
            }

            InitializeComponent();
            monitoredFiles = new MonitoredFiles();
            DataContext = monitoredFiles;
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
                monitoredFiles.AddFile(newFile);
            }
        }

        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}


