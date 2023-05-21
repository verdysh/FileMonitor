using Services.Dto;
using System.Collections.ObjectModel;

namespace FileMonitor.ViewModels
{
    internal class FilesViewModel 
    {
        private ObservableCollection<SourceFileDto> _files;
        public FilesViewModel(ObservableCollection<SourceFileDto> collection)
        {
            _files = collection;
        }
        public ObservableCollection<SourceFileDto> Files
        {
            get { return _files; } 
            set
            {
                _files = value;
            }
        }
    }
}
