using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using FileMonitor.Dialogs;
using FileMonitor.FileBackups;
using FileMonitor.ViewModels;
using Services;
using Services.Dto;
using Services.Extensions;
using Services.Helpers;
using System;

namespace FileMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
            using var fullBackupService = new BackupPathService(RepositoryHelper.CreateFullBackupPathRepositoryInstance());

            _viewModel = new MainWindowViewModel(
                new ObservableCollection<BackupPathDto>(fullBackupService.GetFullBackupRows()), 
                new ObservableCollection<SourceFileDto>(sourceFileService.GetFiles())
            );

            DataContext = _viewModel;
            _viewModel.Init();
        }

        /// <summary>
        /// Add a file to be monitored by the program
        /// </summary>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string[] newFiles = FileDialogWindow.GetPath();
            using var service = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
            foreach (string newFile in newFiles)
            {
                if (newFile == "" || service.PathExists(newFile)) continue;
                SourceFileDto dto = service.Add(newFile);
                _viewModel.SourceFiles.Add(dto);
            }
        }

        /// <summary>
        /// Add a folder to be monitored by the program
        /// </summary>
        private void AddNewFolder_Click(object sender, RoutedEventArgs e)
        {
            string directory = FolderDialogWindow.GetPath();
            {
                using var service = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
                string[] paths = null;
                try
                {
                    paths = Directory.GetFileSystemEntries(directory, "*", SearchOption.AllDirectories);
                }
                catch(UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Access to system files denied.\nRun program as administrator.");
                    return;
                }
                int numberOfDirectories = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).Length;
                int numberOfFiles = paths.Length;
                if (VerifyAddFiles(directory, numberOfDirectories, numberOfFiles))
                {
                    foreach (string path in paths)
                    {
                        SourceFileDto dto = service.Add(path);
                        _viewModel.SourceFiles.Add(dto);
                    }
                }
            }
        }

        private bool VerifyAddFiles(string directory, int numberOfDirectories, int numberOfFiles)
        {
            string text = $@"Do you wish to add the folder {directory}? 

{numberOfFiles} file(s) from {numberOfDirectories} subfolders(s) will be monitored by the program.";
            string caption = "Confirm Add Folder";

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(text, caption, button, image);
            return result == MessageBoxResult.Yes;
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
                SourceFileService service = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
                List<int> ids = new List<int>();
                foreach (var item in selectedFiles)
                {
                    ids.Add(item.Id);
                }
                service.Remove(ids);
                _viewModel.SourceFiles.RemoveRange<SourceFileDto>(selectedFiles);
            }
        }

        /// <summary>
        /// Confirm if the user wants to delete these files from the program
        /// </summary>
        /// <returns> The user's message box selection </returns>
        private MessageBoxResult ConfirmDeleteFiles()
        {
            string text = "Do you wish to delete the selected file(s) from the program? This cannot be undone.";
            string caption = "Delete SourceFiles";

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Warning;
            return MessageBox.Show(text, caption, button, image);
        }

        /// <summary>
        /// Create a full backup of all files monitored by the program
        /// </summary>
        private void CreateFullBackup_Click(object sender, RoutedEventArgs e)
        {
            foreach(BackupPathDto dto in _viewModel.BackupPaths)
            {
                if(dto.IsSelected)
                {
                    FullBackup backup = new FullBackup(dto.Path);
                    backup.Run(_viewModel.SourceFiles.Select(f => f.Path));
                }
            }
            MessageBox.Show("Backup complete");
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
            using var service = new BackupPathService(RepositoryHelper.CreateFullBackupPathRepositoryInstance());
            if (backupPath == "" || service.PathExists(backupPath)) return;
            BackupPathDto backupDto = service.Add(backupPath);
            _viewModel.BackupPaths.Add(backupDto);
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
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)sender;
            BackupPathDto fullBackupDto = (BackupPathDto)checkBox.DataContext;
            using BackupPathService service = new BackupPathService(RepositoryHelper.CreateFullBackupPathRepositoryInstance());
            service.Update(fullBackupDto);
            _viewModel.BackupSelected = _viewModel.IsAnyBackupSelected();
        }
    }
}


