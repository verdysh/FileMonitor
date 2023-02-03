using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FileMonitor.Models
{
    class MonitoredFiles : INotifyPropertyChanged
    {
        /// <summary>
        /// Private field to store all file paths
        /// </summary>
        private List<string> files;

        /// <summary>
        /// Declare PropertyChanged event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Public property to expose the private field
        /// Call OnPropertyChanged() whenever the files have changed.
        /// </summary>
        public List<string> Files
        {
            get { return files; }
            set
            {
                if (value != files)
                {
                    files = value;
                    OnPropertyChanged();
                }
                else return;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string files = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(files));
        }

        /// <summary>
        /// Display all currently stored file paths
        /// </summary>
        /// <param name="mw"> MainWindow object. Used to update the XAML TextBlocks </param>
        public void ShowAll(MainWindow mw)
        {
            List<string> files = JsonFile.GetDeserializedList();
            string result = "";
            for (int i = 0; i < files.Count; i++)
            {
                result += files[i] + "\n";
            }
            mw.FilesDisplayed.Text = result;
        }

        /// <summary>
        /// Display file paths only if the files have changed since the last backup
        /// </summary>
        public void ShowChangedSinceBackup(MainWindow mw)
        {
            mw.RecentlyChangedFiles.Text = JsonFile.GetDeserializedList().ToString();
        }
    }
}
