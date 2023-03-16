using System.Collections.ObjectModel;
using System.IO;

namespace FileMonitor.Models
{
    internal class Backup
    {
        private DriveInfo[] allLogicalDrives;
        private string backupDestination;
        public DriveInfo[] AllLogicalDrives
        {
            get => allLogicalDrives;
        }

        public Backup(string backupDestination)
        {
            allLogicalDrives = DriveInfo.GetDrives();
            this.backupDestination = backupDestination;
        }

        public void Run(ReadOnlyObservableCollection<string> files)
        {
            DirectoryInfo backupFileDirectoryPath;
            foreach(string sourceFile in files)
            {
                //backupFileDirectoryPath = new DirectoryInfo(BackupFileDirectoryPath(sourceFile));
                //if(!backupFileDirectoryPath.Exists) backupFileDirectoryPath.Create();
                File.Copy(sourceFile, backupDestination, true);
            }
        }

        private string GetFullBackupPath(string? sourceFile)
        {
            string oldRoot = Path.GetPathRoot(sourceFile);
            return sourceFile.Replace(oldRoot, backupDestination);
        }

        private string GetFullBackupDirectory(string? backupPath)
        {
            return "";
        }
    }
}
