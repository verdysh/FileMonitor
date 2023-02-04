using System.Collections.Generic;

namespace FileMonitor.Database.Tables
{
    internal class BackupHash
    {
        private List<int> iDs;
        private List<int> hashCodes;

        public List<int> IDs { get => iDs; }
        public List<int> HashCodes { get => hashCodes; } 
        public BackupHash() { }
    }
}
