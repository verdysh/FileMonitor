using FileMonitor.Database;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileMonitor.Models 
{
    /// <summary>
    /// Defines a class for MainWindow.xaml.cs to access a list of files that are monitored by the program.
    /// </summary>
    class AppViewModel : INotifyPropertyChanged
    {
        private ReadOnlyObservableCollection<string>? allFilePaths;
        private ReadOnlyObservableCollection<string>? recentlyChangedFiles;
        private SourceFile sourceFile;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ReadOnlyObservableCollection<string>? AllFilePaths 
        { 
            get => allFilePaths; 
        }

        public ReadOnlyObservableCollection<string>? RecentlyChangedFiles 
        { 
            get => recentlyChangedFiles; 
        }

        public AppViewModel()
        {
            sourceFile = new SourceFile();
            allFilePaths = new ReadOnlyObservableCollection<string>(sourceFile.FilePaths);
            //sourceFile.FilePaths.CollectionChanged += FilePaths_CollectionChanged;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Adds a file path from the UI
        /// </summary>
        /// <param name="path"> Path to add </param>
        public void AddFile(string path)
        {
            sourceFile.AddFile(path);
        }

        /// <summary>
        /// Removes a file path as specified in the UI
        /// </summary>
        /// <param name="path"> Path to remove </param>
        public void RemoveFile(string path)
        {
            sourceFile.RemoveFile(path);
        }

        private string Format(ReadOnlyObservableCollection<string> collection) 
        {
            string result = "";
            foreach(string item in collection)
            {
                result += $"{item}\n";
            }
            return result;
        }

        ///// <summary>
        ///// A method for subscribing to the CollectionChanged event in SourceFile.FilePaths
        ///// </summary>
        //protected void FilePaths_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
                
        //        if (e.NewItems.Count == 0) return;
        //        else 
        //        {
        //            string newValue = allFilePaths;
        //            foreach (var item in e.NewItems) newValue += $"{item}\n";
        //            allFilePaths = newValue;
        //            OnPropertyChanged("AllFilePaths");
        //        }
        //    }

        //    if(e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        string newValue = allFilePaths;
        //    }
        //}
    }
}
