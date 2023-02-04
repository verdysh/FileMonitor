using System.Collections.Generic;

namespace FileMonitor.Database.Tables
{
    internal class SourceBackupHashRel
    {
        private List<int> sourceHashIDs;
        private List<int> backupHashIDs;

        public List<int> SourceHashIDs { get => sourceHashIDs; }
        public List<int> BackupHashIDs { get => backupHashIDs; }
        public SourceBackupHashRel() { }
    }
}
