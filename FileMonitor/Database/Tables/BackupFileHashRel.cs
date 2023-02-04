using System.Collections.Generic;

namespace FileMonitor.Database.Tables
{
    internal class BackupFileHashRel
    {
        private List<int> backupFileIDs;
        private List<int> backupHashIDs;

        public List<int> BackupFileIDs { get => backupFileIDs; }
        public List<int> BackupHashIDs { get => backupHashIDs; }

        public BackupFileHashRel() { }
    }
}
