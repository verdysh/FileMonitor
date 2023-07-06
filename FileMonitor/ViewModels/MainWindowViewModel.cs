using Services.Dto;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FileMonitor.ViewModels
{
    /// <summary>
    /// Provides a view model for binding to the UI. 
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<BackupPathDto> _backupPaths;
        private ObservableCollection<SourceFileDto> _sourceFiles;
        private ObservableCollection<SourceFileDto> _updatedFiles;
        private ObservableCollection<SourceFolderDto> _sourceFolders;
        private bool _backupSelected;

        /// <summary>
        /// A public event handler to notify any data bindings when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Determines whether or not a backup path is selected in the UI. If a path is selected, the "Copy All" button is clickable. If not, the button is greyed out. This property is bound to the <see cref="MainWindow.CopyAllFiles"/> button in the UI.
        /// </summary>
        public bool BackupSelected
        {
            get { return _backupSelected; }
            set
            {
                _backupSelected = value;
                OnPropertyChanged(nameof(BackupSelected));
            }
        }

        /// <summary>
        /// An observable collection of <see cref="BackupPathDto"/> objects. This collection displays all possible backup path locations for the user to select, add, or remove. This property is bound to the <see cref="MainWindow.BackupPathsDisplayed"/> list view in the UI.
        /// </summary>
        public ObservableCollection<BackupPathDto> BackupPaths
        {
            get { return _backupPaths; }
            //set { _backupPaths = value; }
        }

        /// <summary>
        /// An observable collection of <see cref="SourceFileDto"/> objects. This collection displays all files monitored by the program for the user to add or remove. This property is bound to the <see cref="MainWindow.FilesDisplayed"/> list view in the UI.
        /// </summary>
        public ObservableCollection<SourceFileDto> SourceFiles
        {
            get { return _sourceFiles; }
            //set { _sourceFiles = value; }
        }

        /// <summary>
        /// An observable collection of <see cref="SourceFileDto"/> objects. This collection displays only the files that have been updated since the last time they were copied to a backup location. This property is bound to the <see cref="MainWindow.UpdatedFilesDisplayed"/> list view.
        /// </summary>
        public ObservableCollection<SourceFileDto> UpdatedFiles
        {
            get { return _updatedFiles; }
            //set { _updatedFiles = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<SourceFolderDto> SourceFolders 
        {
            get { return _sourceFolders; } 
            //set { _sourceFolders = value; }
        }

        /// <summary>
        /// Defines the <see cref="MainWindowViewModel"/> class constructor.
        /// </summary>
        /// <param name="backupPaths"> All backup paths stored in the database, formatted as data transfer objects. </param>
        /// <param name="sourceFiles"> All files monitored by the program, formatted as data transfer objects. </param>
        /// <param name="updatedFiles"> Only the files that have been updated since the last time they were copied to a backup location, formatted as data transfer objects. </param>
        /// <param name="sourceFolders"> All folders monitored by the program, formatted as data transfer objects. </param>
        public MainWindowViewModel(
            ObservableCollection<BackupPathDto> backupPaths, 
            ObservableCollection<SourceFileDto> sourceFiles,
            ObservableCollection<SourceFileDto> updatedFiles,
            ObservableCollection<SourceFolderDto> sourceFolders)
        {
            _backupPaths = backupPaths;
            _sourceFiles = sourceFiles;
            _updatedFiles = updatedFiles;
            _sourceFolders = sourceFolders;
            _backupSelected = IsAnyBackupSelected();
        }

        /// <summary>
        /// A public method to invoke the <see cref="PropertyChanged"/> delegate.
        /// </summary>
        /// <param name="propertyName"> The name of the property from which the method is called. </param>
        public void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// If at least one backup path is selected in the UI, return true. If none are selected, return false.
        /// </summary>
        public bool IsAnyBackupSelected()
        {
            foreach (BackupPathDto dto in  _backupPaths)
            {
                if (dto.IsSelected == true) return true;
            }
            return false;
        }
    }
}
