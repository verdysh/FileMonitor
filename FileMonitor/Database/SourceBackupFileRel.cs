using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class SourceBackupFileRel
    {
        private List<int> _sourceIDs;
        private List<int> _backupIDs;

        public List<int> SourceIDs { get => _sourceIDs; }
        public List<int> BackupIDs { get => _backupIDs; }

        public SourceBackupFileRel() { }
    }
}
