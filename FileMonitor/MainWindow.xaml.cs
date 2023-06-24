using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System;
using FileMonitor.Dialogs;
using FileMonitor.FileBackups;
using FileMonitor.ViewModels;
using Services;
using Services.Dto;
using Services.Extensions;
using Services.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

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
            using var backupPathService = new BackupPathService(RepositoryHelper.CreateBackupPathRepositoryInstance());

            _viewModel = new MainWindowViewModel(
                new ObservableCollection<BackupPathDto>(backupPathService.GetFilePaths()),
                new ObservableCollection<SourceFileDto>(sourceFileService.GetFilePaths()),
                new ObservableCollection<SourceFileDto>(sourceFileService.GetModifiedFilePaths())
            );

            DataContext = _viewModel;
        }

        /// <summary>
        /// Add a file to be monitored by the program
        /// </summary>
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            string[] newFiles = FileDialogWindow.GetPath();
            using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
            foreach (string newFile in newFiles)
            {
                if (newFile == "" || sourceFileService.PathExists(newFile)) continue;
                SourceFileDto dto = sourceFileService.Add(newFile);
                _viewModel.SourceFiles.Add(dto);
                _viewModel.UpdatedFiles.Add(dto);
            }
        }

        /// <summary>
        /// Add a folder to be monitored by the program
        /// </summary>
        private void AddNewFolder_Click(object sender, RoutedEventArgs e)
        {
            string directory = FolderDialogWindow.GetPath();
            {
                using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
                string[] paths = null;
                try
                {
                    paths = Directory.GetFileSystemEntries(directory, "*", SearchOption.AllDirectories);
                }
                catch(UnauthorizedAccessException ex)
                {
                    MessageBox.Show("Access to files denied.\nRun program as administrator.");
                    return;
                }
                int numberOfDirectories = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).Length;
                int numberOfFiles = paths.Length;
                if (VerifyAddFiles(directory, numberOfDirectories, numberOfFiles))
                {
                    foreach (string path in paths)
                    {
                        try
                        {
                            SourceFileDto dto = sourceFileService.Add(path);
                            _viewModel.SourceFiles.Add(dto);
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            MessageBox.Show("Access to files denied.\nRun program as administrator.");
                            return;
                        }

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
            MessageBoxResult result = ConfirmDeleteFiles();
            if (result == MessageBoxResult.Yes)
            {
                using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
                List<int> ids = new List<int>();
                List<SourceFileDto> selectedFiles = new List<SourceFileDto>();
                foreach (object item in FilesDisplayed.SelectedItems)
                {
                    SourceFileDto dto = (SourceFileDto)item;
                    selectedFiles.Add(dto);
                    ids.Add(dto.Id);
                }
                sourceFileService.Remove(ids);
                _viewModel.SourceFiles = new ObservableCollection<SourceFileDto>(sourceFileService.GetFilePaths());
                _viewModel.UpdatedFiles = new ObservableCollection<SourceFileDto>(sourceFileService.GetModifiedFilePaths());
                //_viewModel.UpdatedFiles = 
            }
        }

        //private List<int> GetFileIndicesById(ObservableCollection<SourceFileDto> list, List<int> ids)
        //{
        //    List<int> result = new List<int>();
        //    foreach (SourceFileDto item in list)
        //    {
        //        if(ids.Contains(item.Id)) result.Add(item.Id);
        //    }
        //    return result;
        //}

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
        private void CopyAllFiles_Click(object sender, RoutedEventArgs e)
        {
            foreach(BackupPathDto dto in _viewModel.BackupPaths)
            {
                if(dto.IsSelected)
                {
                    Backup backup = new Backup(dto.Path);
                    backup.CopyAll(_viewModel.SourceFiles.Select(f => f.Path));
                }
            }
            MessageBox.Show("Backup complete");
        }

        /// <summary>
        /// Create a backup only for files that have been updated or changed
        /// </summary>
        private void CopyUpdatedFiles_Click(object sender, RoutedEventArgs e)
        {
            foreach (BackupPathDto dto in _viewModel.BackupPaths)
            {
                if (dto.IsSelected)
                {
                    Backup backup = new Backup(dto.Path);
                    backup.CopyUpdated(_viewModel.UpdatedFiles.Select(f => f.Path));
                }
            }
            ResetUpdatedFiles();
        }

        private void ResetUpdatedFiles()
        {
            List<int> ids = new List<int>();
            foreach (SourceFileDto dto in _viewModel.UpdatedFiles) ids.Add(dto.Id);
            _viewModel.UpdatedFiles.Clear();
            using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
            sourceFileService.ResetIsModifiedFlag(ids);
        }

        /// <summary>
        /// Add a folder path for a full backup to be copied to
        /// </summary>
        private void AddBackupPath_Click(object sender, RoutedEventArgs e)
        {
            string backupPath = FolderDialogWindow.GetPath();
            using var backupPathService = new BackupPathService(RepositoryHelper.CreateBackupPathRepositoryInstance());
            if (backupPath == "" || backupPathService.PathExists(backupPath)) return;
            BackupPathDto backupPathDto = backupPathService.Add(backupPath);
            _viewModel.BackupPaths.Add(backupPathDto);
        }

        /// <summary>
        /// Event handler to be called when the a BackupPathCheckBox is checked in the UI
        /// </summary>
        /// <remarks>
        /// Update the IsSelected property in the database using a data transfer object. This ensures
        /// that the CheckBox remains selected even after the program exits. 
        /// </remarks>
        private void BackupPathCheckBox_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)sender;
            BackupPathDto backupPathDto = (BackupPathDto)checkBox.DataContext;
            using BackupPathService backupPathService = new BackupPathService(RepositoryHelper.CreateBackupPathRepositoryInstance());
            backupPathService.Update(backupPathDto);
            _viewModel.BackupSelected = _viewModel.IsAnyBackupSelected();
        }
    }
}


