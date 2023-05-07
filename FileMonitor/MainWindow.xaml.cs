using System.Windows;
using FileMonitor.Models;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using FileMonitor.ViewModels;
using Services.SourceFiles;
using DataAccessLayer;
using Services.SourceFiles.Dto;
using System.Collections.ObjectModel;
using Services.Extensions;

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
            ObservableCollection<SourceFileDto> observableFiles = new ObservableCollection<SourceFileDto>(service.GetFiles());
            _viewModel.Files = observableFiles;
            FilesDisplayed.DataContext = _viewModel;
        }

        /// <summary>
        /// Add a file to be monitored by the program
        /// </summary>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();

            if(newFile != "")
            {
                using var service = new SourceFilesService();
                SourceFileDto addedFile = service.Add(newFile);
                if(addedFile != null) _viewModel.Files.Add(addedFile);
            }
        }

        /// <summary>
        /// Add a folder to be monitored by the program
        /// </summary>
        private void AddNewFolder_Click(object sender, RoutedEventArgs e)
        {
            string newFolder = FolderDialogWindow.GetPath();
        }

        /// <summary>
        /// Remove a file from the collection of monitored files
        /// </summary>
        private void DeleteFiles_Click(object sender, RoutedEventArgs e)
        {
            List<SourceFileDto> selectedFiles = new List<SourceFileDto>();
            foreach (object item in FilesDisplayed.SelectedItems)
            {
                selectedFiles.Add((SourceFileDto)item);
            }

            MessageBoxResult result = ConfirmDeleteFiles();

            if (result == MessageBoxResult.Yes)
            {
                SourceFilesService service = new SourceFilesService();
                List<int> ids = new List<int>();
                foreach (var item in selectedFiles)
                {
                    ids.Add(item.Id);
                }
                service.Remove(ids);
                _viewModel.Files.RemoveRange<SourceFileDto>(selectedFiles);
            }
        }

        /// <summary>
        /// Confirm if the user wants to delete these files from the program
        /// </summary>
        /// <returns> The user's message box selection </returns>
        private MessageBoxResult ConfirmDeleteFiles()
        {
            string text = "Do you wish to delete the selected file(s) from the program? This cannot be undone.";
            string caption = "Delete Files";

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Warning;
            return MessageBox.Show(text, caption, button, image);
        }
        
        /// <summary>
        /// Allow the DeleteFiles button to be selected once an item is highlighted in the FilesDisplayed property
        /// </summary>
        private void FilesDisplayed_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            DeleteFiles.IsEnabled = true;
        }

        /// <summary>
        /// Create a full backup of all files monitored by the program
        /// </summary>
        private void CreateFullBackup_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Create a sequential backup for files that have changed since the last backup
        /// </summary>
        private void CreateSequentialBackup_Click(object sender, RoutedEventArgs e)
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


