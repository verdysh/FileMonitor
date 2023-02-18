using FileMonitor.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FileMonitor.Models 
{
    /// <summary>
    /// Defines a class for MainWindow.xaml.cs to access a list of files that are monitored by the program.
    /// </summary>
    class MonitoredFiles : ViewBase
    {
        private ReadOnlyObservableCollection<string>? readOnlyFilePaths;
        private ObservableCollection<string>? pathsChangedSinceBackup;
        private SourceFile sourceFile;

        public MonitoredFiles()
        {
            this.sourceFile = new SourceFile();
            readOnlyFilePaths = sourceFile.FilePaths;
        }

        public ReadOnlyObservableCollection<string>? AllFilePaths
        {
            get { return readOnlyFilePaths; }
        }

        public void AddFile(string path)
        {
            sourceFile.AddFile(path);
        }
    }
}
