using Services.Dto;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace FileMonitor.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FullBackupDto> _backupPaths;
        private ObservableCollection<SourceFileDto> _files;
        private bool _backupSelected;
        private int _backupWidth;
        private int _fileWidth;

        public event PropertyChangedEventHandler? PropertyChanged;

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

        public int FileWidth
        {
            get { return _fileWidth; }
            set 
            { 
                _fileWidth = value * 7;
                OnPropertyChanged(nameof(FileWidth));
            }
        }

        public ObservableCollection<FullBackupDto> BackupPaths
        {
            get { return _backupPaths; }
            set { _backupPaths = value; }
        }

        public ObservableCollection<SourceFileDto> SourceFiles
        {
            get { return _files; }
            set { _files = value; }
        }

        public MainWindowViewModel(ObservableCollection<FullBackupDto> backups, ObservableCollection<SourceFileDto> sourceFiles)
        {
            _backupPaths = backups;
            _files = sourceFiles;
            _backupSelected = IsAnyBackupSelected();
            BackupPaths.CollectionChanged += (sender, e) 
                => BackupWidth = GetColumnWidth(_backupPaths);
            SourceFiles.CollectionChanged += (sender, e)
                => FileWidth = GetColumnWidth(_files);
        }

        public void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsAnyBackupSelected()
        {
            foreach (FullBackupDto dto in  _backupPaths)
            {
                if (dto.IsSelected == true) return true;
            }
            return false;
        }

        public int GetColumnWidth<T>(ObservableCollection<T> collection) where T : IHasPath
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
