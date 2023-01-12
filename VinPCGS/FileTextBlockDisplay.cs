using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VinPCGS
{
    class FileTextBlockDisplay : INotifyPropertyChanged
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
        public void ShowAllFiles()
        {

        }

        /// <summary>
        /// Display file paths only if the files have changed since the last backup
        /// </summary>
        public void ShowRecentlyChangedFiles()
        {

        }
    }
}
