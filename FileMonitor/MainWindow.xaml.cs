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
        FileTextBlockDisplay textBlockDisplay = new FileTextBlockDisplay(); // fires an event when the list of files have changed
        DatabaseInserter inserter = new DatabaseInserter();
        string programDir = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\fileMonitor";

        public MainWindow()
        {
            if (!File.Exists(JsonFile.storedPaths))
            {
                File.Create(JsonFile.storedPaths);
            }

            InitializeComponent();

            if (!File.Exists($"{programDir}\\FMDB.sqllite"))
            {
                Directory.CreateDirectory(programDir);
                DatabaseBuilder builder = new DatabaseBuilder($"{programDir}\\FMDB.sqllite");
                builder.Create();
            }
            textBlockDisplay.ShowAllFiles(this);
        }

        /// <summary>
        /// Add new filepath to database. Notify the PropertyChanged event.
        /// </summary>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();
            if(newFile != "")
            {
                IDQuery idQuery = new IDQuery();
                List<string> ids = idQuery.GetSingleColumnIDs("source_file");

                int id = Int32.Parse(ids[ids.Count - 1]);
                id++;

                inserter.Insert("source_file", $"({id} {newFile})");

                JsonFile.WriteToFile(newFile);
                textBlockDisplay.PropertyChanged += TextBlockDisplay_PropertyChanged;
                textBlockDisplay.Files = JsonFile.GetDeserializedList();
            }
        }

        /// <summary>
        /// Update XAML TextBlocks when the PropertyChanged event is fired
        /// </summary>
        /// <remarks> Pass this MainWindow instance by reference to both method calls </remarks>
        private void TextBlockDisplay_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            textBlockDisplay.ShowAllFiles(this); 
            textBlockDisplay.ShowRecentlyChangedFiles(this);
        }

        private void EditFiles_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
