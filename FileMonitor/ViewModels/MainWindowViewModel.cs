using Services.Dto;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace FileMonitor.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BackupPathDto> _backupPaths;
        private ObservableCollection<SourceFileDto> _sourceFiles;
        private ObservableCollection<SourceFileDto> _updatedFiles;
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
            get { return _sourceFiles; }
            set { _sourceFiles = value; }
        }

        public ObservableCollection<SourceFileDto> UpdatedFiles
        {
            get { return _updatedFiles; }
            set { _updatedFiles = value; }
        }

        public MainWindowViewModel(
            ObservableCollection<BackupPathDto> backupPaths, 
            ObservableCollection<SourceFileDto> sourceFiles,
            ObservableCollection<SourceFileDto> updatedFiles)
        {
            _backupPaths = backupPaths;
            _sourceFiles = sourceFiles;
            _updatedFiles = updatedFiles;
            _sourceFiles.CollectionChanged += SourceFileCollectionChanged;
            _backupSelected = IsAnyBackupSelected();
        }

        public void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SourceFileCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                IEnumerable newSourceFiles = e.NewItems;
                foreach(SourceFileDto sourceFile in newSourceFiles) _updatedFiles.Add(sourceFile);
            }
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
