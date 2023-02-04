using System.Collections.Generic;

namespace FileMonitor.Database
{
    internal class SourceFileHashRel
    {
        private List<int> sourceFileIDs;
        private List<int> sourceHashIDs;

        public List<int> SourceFileIDs { get => sourceFileIDs; }
        public List<int> SourceHashIDs { get => sourceHashIDs; }

        public SourceFileHashRel() { }
    }
}
