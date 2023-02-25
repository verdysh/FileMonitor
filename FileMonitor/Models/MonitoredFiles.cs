using FileMonitor.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private string allFilePaths;
        private SourceFile sourceFile;

        public string? AllFilePaths
        {
            get { return allFilePaths; }
        }

        public MonitoredFiles()
        {
            this.sourceFile = new SourceFile();
            this.sourceFile.FilePaths.CollectionChanged += FilePaths_CollectionChanged; 
            this.allFilePaths = Format((ReadOnlyObservableCollection<string>)this.sourceFile.FilePaths);
        }

        /// <summary>
        /// Adds a file path from the UI
        /// </summary>
        /// <param name="path"> Path to add </param>
        public void AddFile(string path)
        {
            sourceFile.AddFile(path);
        }

        private string Format(ReadOnlyObservableCollection<string> collection) 
        {
            string result = "";
            foreach(string item in collection)
            {
                result += item + "\n";
            }
            return result;
        }

        /// <summary>
        /// A method for subscribing to the CollectionChanged event in SourceFile.FilePaths
        /// </summary>
        protected void FilePaths_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems.Count == 0) return;
                else foreach (var item in e.NewItems) this.allFilePaths += $"{item}\n";
            }
        }
    }
}
