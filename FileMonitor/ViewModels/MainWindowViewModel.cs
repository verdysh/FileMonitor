using Services.Dto;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace FileMonitor.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FullBackupDto> _backupPaths;
        private ObservableCollection<SourceFileDto> _files;
        private CollectionViewSource _checkedBackupFolders;

        private bool _backupSelected;
        private int _backupWidth;
        private int _fileWidth;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICollectionView CheckedBackupFolders
        {
            get
            {
                return _checkedBackupFolders.View;
            }
        }

        public bool BackupSelected
        {
            get { return _backupSelected; }
            set
            {
                _backupSelected = value;
                OnPropertyChanged(nameof(BackupSelected));
            }
        }

        public int BackupWidth
        {
            get { return _backupWidth; }
            set
            {
                _backupWidth = value * 7;
                OnPropertyChanged(nameof(BackupWidth));
            }
        }

        public ObservableCollection<FullBackupDto> BackupPaths
        {
            get { return _backupPaths; }
            set
            {
                _backupPaths = value;
            }
        }

        public ObservableCollection<SourceFileDto> Files
        {
            get { return _files; }
            set
            {
                _files = value;
            }
        }

        public int FileWidth
        {
            get { return _fileWidth; }
            set
            {
                _fileWidth = value;

            }
        }

        public MainWindowViewModel(ObservableCollection<FullBackupDto> backups, ObservableCollection<SourceFileDto> sourceFiles)
        {
            _backupPaths = backups;
            _files = sourceFiles;
            _backupSelected = IsAnyBackupSelected();

            _checkedBackupFolders = new CollectionViewSource
            {
                Source = _backupPaths,
            };
            _checkedBackupFolders.Filter += (sender, e) => e.Accepted = (e.Item as FullBackupDto).IsSelected;

            BackupPaths.CollectionChanged += (sender, e) => BackupPathsOnChange("CollectionChanged");

            Files.CollectionChanged += (sender, e) => FileWidth = GetColumnWidth(_files);
        }

        // "action" parameter type "string" is not the best choice, I use string type for simplicity
        public void BackupPathsOnChange(string action)
        {
            if(action == "CollectionChanged")
            {
                BackupWidth = GetColumnWidth(_backupPaths);
            }                
            else if(action == "CheckboxClicked")
            {
                BackupSelected = IsAnyBackupSelected();
                _checkedBackupFolders.View.Refresh();
            }
        }

        public void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool IsAnyBackupSelected()
        {
            foreach (FullBackupDto dto in  _backupPaths)
            {
                if (dto.IsSelected == true) return true;
            }
            return false;
        }

        private int GetColumnWidth<T>(ObservableCollection<T> collection) where T : IHasPath
        {
            int maxWidth = 1;
            foreach (T dto in collection)
                maxWidth = Math.Max(maxWidth, dto.Path.Length);
            return maxWidth;
        }

        public void Init()
        {
            BackupWidth = GetColumnWidth(_backupPaths);
            FileWidth = GetColumnWidth(_files);
        }
    }
}
