using System.Windows;
using FileMonitor.Models;
using System.Data.SQLite;
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
        /// <summary>
        /// This object fires an event when the list of files have changed
        /// </summary>
        FileTextBlockDisplay textBlockDisplay = new FileTextBlockDisplay();
        SQLStatements sqlStatements = new SQLStatements();
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
                builder.Create(sqlStatements.tablesColumnsCreate);
            }
            textBlockDisplay.ShowAllFiles(this);
        }

        /// <summary>
        /// Update JSON file, and update the Files property in textBlockDisplay in order to trigger
        /// the PropertyChanged event
        /// </summary>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();
            if(newFile != "")
            {
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
