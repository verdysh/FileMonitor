using Services.Dto;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FileMonitor.ViewModels
{
    public class FullBackupViewModel
    {
        private ObservableCollection<FullBackupDto> _backupPaths;
        private bool _backupSelected;

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
            private set { }
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
