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

namespace FileMonitor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        /// <summary>
        /// Defines the <see cref="MainWindow"/> class constructor. Uses <see cref="SourceFileService"/> and <see cref="BackupPathService"/> to initialize the data bindings in the view model. ALso sets the data context for the UI.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
            using var backupPathService = new BackupPathService(RepositoryHelper.CreateBackupPathRepositoryInstance());

            _viewModel = new MainWindowViewModel(
                new ObservableCollection<BackupPathDto>(backupPathService.GetDirectoryPaths()),
                new ObservableCollection<SourceFileDto>(sourceFileService.GetFilePaths()),
                new ObservableCollection<SourceFileDto>(sourceFileService.GetModifiedFilePaths())
            );
            DataContext = _viewModel;
        }


        // A button click event handler for adding a file to be monitored by the program. Newly added files are also added
        // to the MainWindowViewModel.UpdatedFiles collection. This assumes that the file must first be copied to a backup
        // location. 
        private void AddNewFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] newFiles = FileDialogWindow.GetPath();
                if (newFiles.Length == 0) return;
                using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
                foreach (string newFile in newFiles)
                {
                    if (newFile == "" || sourceFileService.PathExists(newFile)) continue;
                    SourceFileDto dto = sourceFileService.Add(newFile);
                    _viewModel.SourceFiles.Add(dto);
                    _viewModel.UpdatedFiles.Add(dto);
                }
            }
            catch(UnauthorizedAccessException)
            {
                MessageBox.Show("UnauthorizedAccessException\n Access to system files denied.");
                return;
            }
        }


        // A button click event handler for adding a folder, which effectively adds all files in any subfolders to the program.
        // Newly added files are also added to the MainWindowViewModel.UpdatedFiles collection. This assumes that the file must
        // first be copied to a backup location.
        private void AddNewFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string directory = FolderDialogWindow.GetPath();
                if (directory.Equals("")) return;
                {
                    using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
                    string[] paths = null;
                    paths = Directory.GetFileSystemEntries(directory, "*", SearchOption.AllDirectories);

                    int numberOfDirectories = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).Length;
                    int numberOfFiles = paths.Length;
                    if (VerifyAddFolder(directory, numberOfDirectories, numberOfFiles))
                    {
                        foreach (string path in paths)
                        {
                            SourceFileDto dto = sourceFileService.Add(path);
                            _viewModel.SourceFiles.Add(dto);
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("UnauthorizedAccessException\n Access to system files denied.");
                return;
            }
        }

        // Verifies that the user wants to add an entire folder. Displays the number of files that will be added by doing so.
        private bool VerifyAddFolder(string directory, int numberOfDirectories, int numberOfFiles)
        {
            string text = $@"Do you wish to add the folder {directory}? 

{numberOfFiles} file(s) from {numberOfDirectories} subfolders(s) will be monitored by the program.";
            string caption = "Confirm Add Folder";

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(text, caption, button, image);
            return result == MessageBoxResult.Yes;
        }

        // A button click event handler to remove a file or files from the collection of monitored files. Deleted
        // files are also removed from the MainWindowViewModel.UpdatedFiles collection.
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
                _viewModel.SourceFiles.RemoveRange<SourceFileDto>(selectedFiles);
                _viewModel.UpdatedFiles.RemoveRange<SourceFileDto>(selectedFiles);
            }
        }

        // Confirms that the user wants to delete the selected files, and informs the user that this cannot be undone.
        private MessageBoxResult ConfirmDeleteFiles()
        {
            string text = "Do you wish to delete the selected file(s) from the program? This cannot be undone.";
            string caption = "Delete SourceFiles";

            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage image = MessageBoxImage.Warning;
            return MessageBox.Show(text, caption, button, image);
        }

        // A button click event handler to create a full backup of all files monitored by the program. The full backup
        // copies every file to the backup location, and stores them in a unique directory. The name of the directory
        // uses the current date and time.
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

        // A button click event handler to copy only the files that have been updated or changed since the last backup.
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

        // The main goal here is to reset the collection of updated files, both in the UI and in the database. To do so, the 
        // method retrieves a list of Ids for each file in the UpdatedFiles collection. Then it resets the IsModified checkbox 
        // to be false. Doing so informs the program that the copied version of the files is effectively the most up-to-date
        // version. Finally, it updates the hash for each file to the current hash. Doing so ensures that the next time hashes
        // are compared, these files will be ignored because they represent the latest version of the file.
        private void ResetUpdatedFiles()
        {
            List<int> ids = new List<int>();
            foreach (SourceFileDto dto in _viewModel.UpdatedFiles) ids.Add(dto.Id);
            _viewModel.UpdatedFiles.Clear();
            using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
            sourceFileService.ResetIsModifiedFlag(ids);
            sourceFileService.UpdateHashesToCurrent(ids);
        }

        // A button click event handler for adding a backup folder path. 
        private void AddBackupPath_Click(object sender, RoutedEventArgs e)
        {
            string backupPath = FolderDialogWindow.GetPath();
            using var backupPathService = new BackupPathService(RepositoryHelper.CreateBackupPathRepositoryInstance());
            if (backupPath == "" || backupPathService.PathExists(backupPath)) return;
            BackupPathDto backupPathDto = backupPathService.Add(backupPath);
            _viewModel.BackupPaths.Add(backupPathDto);
        }

        // An event handler to be called when the BackupPathCheckBox is checked in the UI. This method updates the IsSelected property
        // in the database using a data transfer object. This ensures that the CheckBox remains selected even after the program exits. 
        private void BackupPathCheckBox_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox)sender;
            BackupPathDto backupPathDto = (BackupPathDto)checkBox.DataContext;
            using BackupPathService backupPathService = new BackupPathService(RepositoryHelper.CreateBackupPathRepositoryInstance());
            backupPathService.Update(backupPathDto, updatePath: false, updateIsSelected: true);
            _viewModel.BackupSelected = _viewModel.IsAnyBackupSelected();
        }

        // A button click event handler to refresh the UpdatedFilesDisplayed ListView in the UI.
        private void RefreshUpdatedFiles_Click(object sender, RoutedEventArgs e)
        {
            using var sourceFileService = new SourceFileService(RepositoryHelper.CreateSourceFileRepositoryInstance());
            List<SourceFileDto> sourceFileDtos = sourceFileService.GetModifiedFilePaths();
            foreach(SourceFileDto sourceFileDto in sourceFileDtos)
            {
                _viewModel.UpdatedFiles.Add(sourceFileDto);
            }
        }
    }
}


