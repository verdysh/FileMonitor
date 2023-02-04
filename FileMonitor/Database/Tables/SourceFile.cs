using System.Collections.Generic;

namespace FileMonitor.Database.Tables
{
    internal class SourceFile
    {
        private List<int> iDs;
        private List<string> paths;

        public List<int> IDs { get => iDs; }
        public List<string> Paths { get => paths; }

        public SourceFile() { }
    }
}
