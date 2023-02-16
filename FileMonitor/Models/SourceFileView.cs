using FileMonitor.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FileMonitor.Models 
{
    /// <summary>
    /// Defines a class for MainWindow.xaml.cs to access a list of files that are monitored by the program.
    /// </summary>
    class SourceFileView : ViewBase
    {
        private ObservableCollection<string>? allFiles;
        private SourceFile sourceFile;

        public SourceFileView()
        {
            this.sourceFile = new SourceFile();
            allFiles = sourceFile.Paths;
        }

        public ObservableCollection<string> AllFiles
        {
            get { return allFiles; }
            set 
            { 
                SetProperty(ref allFiles, value); 
            }
        }
    }
}
