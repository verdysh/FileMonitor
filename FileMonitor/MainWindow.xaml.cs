using System.Windows;
using FileMonitor.Models;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using FileMonitor.ViewModels;
using Services.SourceFiles;
using DataAccessLayer;
using Services.SourceFiles.Dto;

namespace FileMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FilesViewModel _viewModel;

        public MainWindow()
        {
            if (!File.Exists(ConfigurationManager.AppSettings["DatabasePath"]))
            {
                using var _db = new FileMonitorDbContext(ConfigurationManager.ConnectionStrings[nameof(FileMonitorDbContext)].ConnectionString);
                _db.Database.EnsureCreated();
            }

            InitializeComponent();
            using var service = new SourceFilesService();
            _viewModel = new FilesViewModel();
            _viewModel.Files = service.GetFiles();
            FilesDisplayed.DataContext = _viewModel;
        }

        // Add a file to be monitored by the program
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();

            if(newFile != "")
            {
                using var service = new SourceFilesService();
                service.Add(newFile);
                _viewModel.Files = service.GetFiles();
            }
        }
        // Add a folder to be monitored by the program
        private void AddNewFolder_Click(object sender, RoutedEventArgs e)
        {
            string newFolder = FolderDialogWindow.GetPath();
        }

        // Remove a file from the collection of monitored files
        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {
            // Cast to IEnumerable<>
            IEnumerable<SourceFileDto> selectedFiles = (IEnumerable<SourceFileDto>)FilesDisplayed.SelectedItems; 
            string filesFormatted = FormatFilesToDelete(selectedFiles); 
            MessageBoxResult result = ConfirmDeleteFiles(filesFormatted);
            if (result == MessageBoxResult.Yes)
            {
                SourceFilesService service = new SourceFilesService();
                service.Remove(selectedFiles);
            }
        }

        // Return a formatted string of all files to remove from the program
        private string FormatFilesToDelete(IEnumerable<SourceFileDto> selectedFiles)
        {
            string filesFormatted = "";
            List<SourceFileDto> filesToDelete = new List<SourceFileDto>();

            foreach (SourceFileDto file in selectedFiles) filesToDelete.Add(file);
            foreach (SourceFileDto file in filesToDelete) filesFormatted += $"{file}\n"; 
            return filesFormatted;
        }

        // Confirm if the user wants to delete these files from the program
        private MessageBoxResult ConfirmDeleteFiles(string filesToDelete)
        {
            string text = $"Do you wish to delete the following files from the program? This cannot be undone.\n\n{filesToDelete}";
            string caption = "Delete Files";

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Warning;
            return MessageBox.Show(text, caption, button, image);
        }
        
        // DeleteFiles button remains greyed out until this event handler is called
        private void FilesDisplayed_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DeleteFiles.IsEnabled = true;
        }

        private void PerformBackup_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = ChooseBackupLocation();

            if(result == MessageBoxResult.Yes)
            {
                string backupFolder = FolderDialogWindow.GetPath();
                if (backupFolder.Equals("")) return;
                Backup backup = new Backup(backupFolder);
                //backup.Run(viewModel.AllFilePaths);
            }
        }

        private void BackupRecentlyChangedFiles_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = ChooseBackupLocation();

            if (result == MessageBoxResult.Yes)
            {
                string backupFolder = FolderDialogWindow.GetPath();
                if (backupFolder.Equals("")) return;
                Backup backup = new Backup(backupFolder);
                //backup.Run(viewModel.RecentlyChangedFiles);
            }
        }

        private MessageBoxResult ChooseBackupLocation()
        {
            MessageBoxButton messageBoxButton = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Information;

            string text = "You must choose a backup location. Do you wish to proceed?";
            string caption = "Choose Backup Location";
            return MessageBox.Show(text, caption, messageBoxButton, image);
        }
    }
}


