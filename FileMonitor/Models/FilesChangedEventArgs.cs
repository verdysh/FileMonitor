using System;
using System.Collections.Generic;

namespace FileMonitor.Models
{
    public class FilesChangedEventArgs : EventArgs
    {
        public List<string> OldFiles;
        public List<string> NewFiles;

        public FilesChangedEventArgs(List<string> oldFiles, List<string> newFiles)
        {
            OldFiles = oldFiles;
            NewFiles = newFiles;
        }
    }
}
