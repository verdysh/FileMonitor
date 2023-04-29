using Services.SourceFiles.Dto;
using System.Collections.Generic;
using System.ComponentModel;

namespace FileMonitor.ViewModels
{
    internal class FilesViewModel : INotifyPropertyChanged
    {
        private List<SourceFileDto> _files;
        public List<SourceFileDto> Files
        {
            get { return _files; } 
            set
            {
                _files = value;
                OnPropertyChanged(nameof(Files));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
