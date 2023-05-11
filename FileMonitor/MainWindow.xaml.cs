using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.ObjectModel;
using FileMonitor.Models;
using FileMonitor.Backups;
using FileMonitor.ViewModels;
using Services.SourceFiles;
using Services.FullBackupPaths;
using Services.SourceFiles.Dto;
using Services.Extensions;
using Services.FullBackupPaths.Dto;
using DataAccessLayer;
using System.Linq;

namespace FileMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FilesViewModel _sourceFileViewModel;
        private readonly FullBackupViewModel _fullBackupViewModel;

        public MainWindow()
        {
            if (!File.Exists(ConfigurationManager.AppSettings["DatabasePath"]))
            {
                using var _db = new FileMonitorDbContext(ConfigurationManager.ConnectionStrings[nameof(FileMonitorDbContext)].ConnectionString);
                _db.Database.EnsureCreated();
            }

            InitializeComponent();
            using var sourceFileService = new SourceFileService();
            using var fullBackupService = new FullBackupService();

            _sourceFileViewModel = new FilesViewModel();
            _fullBackupViewModel = new FullBackupViewModel();

            _sourceFileViewModel.Files = new ObservableCollection<SourceFileDto>(sourceFileService.GetFiles());
            _fullBackupViewModel.Paths = new ObservableCollection<FullBackupDto>(fullBackupService.GetFullBackupRows());

            FilesDisplayed.DataContext = _sourceFileViewModel;
            FullBackupPaths.DataContext = _fullBackupViewModel;
        }

        /// <summary>
        /// Add a file to be monitored by the program
        /// </summary>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string newFile = FileDialogWindow.GetPath();
            using var service = new SourceFileService();
            if (newFile == "" || service.PathExists(newFile)) return;
            SourceFileDto addedFile = service.Add(newFile);
            _sourceFileViewModel.Files.Add(addedFile);
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
                SourceFileService service = new SourceFileService();
                List<int> ids = new List<int>();
                foreach (var item in selectedFiles)
                {
                    ids.Add(item.Id);
                }
                service.Remove(ids);
                _sourceFileViewModel.Files.RemoveRange<SourceFileDto>(selectedFiles);
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
            foreach(FullBackupDto dto in _fullBackupViewModel.Paths)
            {
                if(dto.IsSelected)
                {
                    FullBackup backup = new FullBackup(dto.Path);
                    backup.Run(_sourceFileViewModel.Files.Select(f => f.Path));
                }
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
                FullBackup backup = new FullBackup(backupFolder);
                //backup.Run(viewModel.RecentlyChangedFiles);
            }
        }

        /// <summary>
        /// Add a folder path for a full backup to be copied to
        /// </summary>
        private void AddFullBackupPath_Click(object sender, RoutedEventArgs e)
        {
            string backupPath = FolderDialogWindow.GetPath();
            using var service = new FullBackupService();
            if (backupPath == "" || service.PathExists(backupPath)) return;
            FullBackupDto backupDto = service.Add(backupPath);
            _fullBackupViewModel.Paths.Add(backupDto);
        }

        private void AddSequentialBackupPath_Click(object sender, RoutedEventArgs e)
        {

        }

        private MessageBoxResult ChooseBackupLocation()
        {
            MessageBoxButton messageBoxButton = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Information;

            string text = "You must choose a backup location. Do you wish to proceed?";
            string caption = "Choose Backup Location";
            return MessageBox.Show(text, caption, messageBoxButton, image);
        }

        private void FullBackupPathSelected_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (System.Windows.Controls.CheckBox)sender;
            var fullBackupDto = (FullBackupDto)checkBox.DataContext;
            using var service = new FullBackupService();
            service.Update(fullBackupDto);
        }
    }
}


