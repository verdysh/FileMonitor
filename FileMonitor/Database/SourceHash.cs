using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class SourceHash
    {
        private List<int> iDs;
        private List<string> hashCodes;

        public List<int> IDs { get => iDs; }
        public List<string> HashCodes { get => hashCodes; }

        public SourceHash() { }
    }
}
