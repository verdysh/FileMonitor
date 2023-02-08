using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class SourceFileHashRel
    {
        private List<int> _sourceFileIDs;
        private List<int> _sourceHashIDs;

        public List<int> SourceFileIDs { get => _sourceFileIDs; }
        public List<int> SourceHashIDs { get => _sourceHashIDs; }

        public SourceFileHashRel() { }
    }
}
