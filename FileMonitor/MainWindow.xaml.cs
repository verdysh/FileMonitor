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

        FileTextBlockDisplay textBlockDisplay = new FileTextBlockDisplay(); // fires an event when the list of files have changed
        SQLNonQuery nonQuery = new SQLNonQuery(databasePath);


        public MainWindow()
        {
            if (!File.Exists(JsonFile.storedPaths))
            {
                File.Create(JsonFile.storedPaths);
            }

            InitializeComponent();

            if (!File.Exists($"{programDir}\\FMDB.sqlite"))
            {
                Directory.CreateDirectory(programDir);
                SQLNonQueryBuilder builder = new SQLNonQueryBuilder(databasePath);
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
                SQLQuery query = new SQLQuery(databasePath);
                List<string> ids = query.GetSingleColumnIDs("source_file");

                int id = Int32.Parse(ids[ids.Count - 1]);
                id++;

                nonQuery.Insert("source_file", $"({id} {newFile})");

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
