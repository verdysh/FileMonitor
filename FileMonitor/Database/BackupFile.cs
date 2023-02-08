using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class BackupFile
    {
        private List<int> _iDs;
        private List<string> _paths;

        public List<int> IDs { get => _iDs; }
        public List<string> Paths { get => _paths; }

        public BackupFile() { }
    }
}
