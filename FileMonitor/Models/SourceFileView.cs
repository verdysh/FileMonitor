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
        private ObservableCollection<string> allFiles;

        public SourceFileView() 
        {
        }
    }
}
