using System;
using System.Collections.Generic;

namespace FileMonitor.Models
{
    /// <summary>
    /// Defines a class for accessing a list of files that are monitored by the program.
    /// </summary>
    class MonitoredFiles
    {
        private List<string>? allFiles;
        private List<string>? filesChangedSinceBackup;
        public event EventHandler<FilesChangedEventArgs>? FilesChangedEventHandler;

        /// <summary>
        /// Defines a public property for getting and setting all files monitored by the program
        /// </summary>
        /// <remarks> Call OnPropertyChanged() whenever the files have changed.</remarks>
        public List<string> AllFiles
        {
            get { return allFiles; }
            set
            {
                if (value != allFiles)
                {
                    List<string> oldFiles = allFiles;
                    OnFilesChanged(new FilesChangedEventArgs(oldFiles, value));
                    allFiles = value;
                }
                else return;
            }
        }

        protected virtual void OnFilesChanged(FilesChangedEventArgs e)
        {
            FilesChangedEventHandler?.Invoke(this, e);
        }
    }
}
