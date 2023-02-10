using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class SourceBackupFileRel
    {
        private List<int> sourceIDs;
        private List<int> backupIDs;

        public List<int> SourceIDs { get => sourceIDs; }
        public List<int> BackupIDs { get => backupIDs; }

        public SourceBackupFileRel() { }
    }
}
