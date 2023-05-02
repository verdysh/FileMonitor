using Services.SourceFiles.Dto;
using System.Collections.ObjectModel;

namespace FileMonitor.ViewModels
{
    internal class FilesViewModel 
    {
        private ObservableCollection<SourceFileDto> _files;
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
