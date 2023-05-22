using Services.Dto;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace FileMonitor.ViewModels
{
    public class FullBackupViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<FullBackupDto> _backupPaths;
        private bool _backupSelected;
        public event PropertyChangedEventHandler? PropertyChanged;

        public FullBackupViewModel(ObservableCollection<FullBackupDto> collection)
        {
            _backupPaths = collection;
            _backupSelected = IsAnyBackupSelected();
        }

        public ObservableCollection<FullBackupDto> BackupPaths
        {
            get { return _backupPaths; }
            set
            {
                _backupPaths = value;
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
    }
}
