using System.Collections.ObjectModel;
using System.IO;

namespace FileMonitor.Models
{
    internal class Backup
    {
        private DriveInfo[] allLogicalDrives;
        public DriveInfo[] AllLogicalDrives
        {
            get => allLogicalDrives;
        }

        public Backup()
        {
            allLogicalDrives = DriveInfo.GetDrives();
        }

        public static void Run(ReadOnlyObservableCollection<string> files)
        {
            
        }
    }
}
