using Services.Dto;
using System.Collections.ObjectModel;

namespace FileMonitor.ViewModels
{
    public class FullBackupViewModel
    {
        private ObservableCollection<FullBackupDto> _backupPaths;
        public ObservableCollection<FullBackupDto> Paths
        {
            get { return _backupPaths; }
            set
            {
                _backupPaths = value;
            }
        }
    }
}
