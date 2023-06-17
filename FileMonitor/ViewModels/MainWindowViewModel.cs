using Services.Dto;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace FileMonitor.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BackupPathDto> _backupPaths;
        private ObservableCollection<SourceFileDto> _files;
        private bool _backupSelected;

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

        public ObservableCollection<BackupPathDto> BackupPaths
        {
            get { return _backupPaths; }
            set { _backupPaths = value; }
        }

        public ObservableCollection<SourceFileDto> SourceFiles
        {
            get { return _files; }
            set { _files = value; }
        }

        public MainWindowViewModel(ObservableCollection<BackupPathDto> backups, ObservableCollection<SourceFileDto> sourceFiles)
        {
            _backupPaths = backups;
            _files = sourceFiles;
            _backupSelected = IsAnyBackupSelected();
        }

        public void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool IsAnyBackupSelected()
        {
            foreach (BackupPathDto dto in  _backupPaths)
            {
                if (dto.IsSelected == true) return true;
            }
            return false;
        }
    }
}
