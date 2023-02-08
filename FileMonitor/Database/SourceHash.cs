using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class SourceHash
    {
        private List<int> _iDs;
        private List<string> _hashCodes;

        public List<int> IDs { get => _iDs; }
        public List<string> HashCodes { get => _hashCodes; }

        public SourceHash() { }
    }
}
