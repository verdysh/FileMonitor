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
        private ObservableCollection<string>? allFiles;
        private ReadOnlyObservableCollection<string>? allFilesWrapper;
        private ObservableCollection<string>? filesChangedSinceBackup;
        private SourceFile sourceFile;

        public MonitoredFiles()
        {
            this.sourceFile = new SourceFile();
            allFiles = sourceFile.Paths;
            allFilesWrapper = new ReadOnlyObservableCollection<string>(allFiles);
        }

        public ReadOnlyObservableCollection<string>? AllFiles
        {
            get { return allFilesWrapper; }
        }

        public void AddFile(string path)
        {
            allFiles.Add(path);
            // todo: pass value to sourceFile
        }
    }
}
