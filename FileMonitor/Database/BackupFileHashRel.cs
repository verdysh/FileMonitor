using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class BackupFileHashRel
    {
        private List<int> _backupFileIDs;
        private List<int> _backupHashIDs;

        public List<int> BackupFileIDs { get => _backupFileIDs; }
        public List<int> BackupHashIDs { get => _backupHashIDs; }

        public BackupFileHashRel() { }
    }
}
