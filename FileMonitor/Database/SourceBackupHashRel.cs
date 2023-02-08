using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class SourceBackupHashRel
    {
        private List<int> _sourceHashIDs;
        private List<int> _backupHashIDs;

        public List<int> SourceHashIDs { get => _sourceHashIDs; }
        public List<int> BackupHashIDs { get => _backupHashIDs; }
        public SourceBackupHashRel() { }
    }
}
