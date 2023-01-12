using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VinPCGS
{
    class FileTextBlockDisplay : INotifyPropertyChanged
    {
        // Private field to store all file paths
        private List<string> files; 

        // Declare PropertyChanged event
        public event PropertyChangedEventHandler PropertyChanged;

        // Public property to expose the private field
        // Call OnPropertyChanged() whenever the files have changed.
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
