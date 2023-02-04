using System;
using System.Collections.Generic;

namespace FileMonitor.Models
{
    /// <summary>
    /// Pass a list of old files and a list of new files as event arguments
    /// </summary>
    public class FilesChangedEventArgs : EventArgs
    {
        public List<string> OldFiles;
        public List<string> NewFiles;

        /// <summary>
        /// Defines the class constructor
        /// </summary>
        /// <param name="oldFiles"> Old list of monitored files </param>
        /// <param name="newFiles"> Updated list of monitored files </param>
        public FilesChangedEventArgs(List<string> oldFiles, List<string> newFiles)
        {
            OldFiles = oldFiles;
            NewFiles = newFiles;
        }
    }
}
