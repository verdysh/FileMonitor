using FileMonitor.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace FileMonitor.Models 
{
    /// <summary>
    /// Defines a class for MainWindow.xaml.cs to access a list of files that are monitored by the program.
    /// </summary>
    class MonitoredFiles : BaseNotify
    {
        private ReadOnlyObservableCollection<string>? readOnlyFilePaths;
        private string allFilePaths;
        private SourceFile sourceFile;

        public MonitoredFiles()
        {
            this.sourceFile = new SourceFile();
            readOnlyFilePaths = sourceFile.FilePaths;
            allFilePaths = Convert(readOnlyFilePaths);
        }

        public string? AllFilePaths
        {
            get { return allFilePaths; }
        }

        public void AddFile(string path)
        {
            sourceFile.AddFile(path);
        }

        private string Convert(ReadOnlyObservableCollection<string> collection) 
        {
            string result = "";
            foreach(string item in collection)
            {
                result += item + "\n";
            }
            return result;
        }
    }
}
