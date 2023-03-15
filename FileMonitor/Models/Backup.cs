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

        public void Run(ReadOnlyObservableCollection<string> files, string destination)
        {
            foreach(string file in files)
            {
                File.Copy(file, destination, true);
            }
        }
    }
}
