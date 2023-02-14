using FileMonitor.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FileMonitor.Models 
{
    /// <summary>
    /// Defines a view model class for accessing a list of files that are monitored by the program.
    /// </summary>
    class SourceFileView : ModelBase
    {
        private ObservableCollection<string>? allFiles;
        private SourceFile dataAccess;

        public SourceFileView()
        {
            this.dataAccess = new SourceFile();
            allFiles = dataAccess.Paths;
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
