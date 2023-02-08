using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class BackupHash
    {
        private List<int> _iDs;
        private List<int> _hashCodes;

        public List<int> IDs { get => _iDs; }
        public List<int> HashCodes { get => _hashCodes; }
        public BackupHash() { }
    }
}
