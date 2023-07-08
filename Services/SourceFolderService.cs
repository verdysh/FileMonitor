using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Services.Dto;

namespace Services
{
    /// <summary>
    ///  A service class offering database access to the FolderFileMapping Entity. This class stores a repository, and creates the appropriate mapping between the folders and the files contained within them.
    /// </summary>
    public class SourceFolderService : DisposableService
    {
        ISourceFolderRepository _sourceFolderRepository;
        IFolderFileMappingRepository _folderFileMappingRepository;
        ISourceFileRepository _sourceFileRepository;

        /// <summary>
        /// The <see cref="SourceFolderService"/> class constructor.
        /// </summary>
        /// <param name="sourceFolderRepository"> An instance of the <see cref="ISourceFolderRepository"/> which provides database access. </param>
        /// <param name="folderFileMappingRepository"> An instance of the <see cref="IFolderFileMappingRepository"/> which provides database access. </param>
        /// <param name="sourceFileRepository"> An instance of the <see cref="ISourceFileRepository"/> which provides database access. </param>
        public SourceFolderService(
            ISourceFolderRepository sourceFolderRepository, 
            IFolderFileMappingRepository folderFileMappingRepository,
            ISourceFileRepository sourceFileRepository)
        {
            _sourceFolderRepository = sourceFolderRepository;
            _folderFileMappingRepository = folderFileMappingRepository;
            _sourceFileRepository = sourceFileRepository;
        }

        /// <summary>
        /// Returns all monitored folders from the database.
        /// </summary>
        public List<SourceFolderDto> GetFolders()
        {
            return _sourceFolderRepository.GetRange(
                sf => true,
                // Create a new Dto for each Entity, and assign the Dto property values from the Entity properties
                sf => new SourceFolderDto
                {
                    Id = sf.Id,
                    Path = sf.Path,
                },
                sf => sf.Id);
        }

        /// <summary>
        /// Adds a monitored folder to the database. This method ensures appropriate mapping from the source folder to all child files contained within it. <see cref="SourceFileService.Add(string)"/> must be called first on all new file paths before this method is called. This ensures that the file IDs are created, allowing for the folders and files to be mapped appropriately.
        /// </summary>
        /// <param name="directoryPath"> The folder to add to the database. </param>
        /// <param name="filePaths"> An array of all newly added file paths. </param>
        /// <returns> A source folder DTO object for updating the UI. </returns>
        public SourceFolderDto Add(string directoryPath, string[] filePaths)
        {
            SourceFolder entity = new SourceFolder
            {
                Path = directoryPath
            };
            _sourceFolderRepository.Add(entity);
            _sourceFolderRepository.SaveChanges(); 
            AddFolderFileMapping(filePaths, GetDirectoryId(directoryPath));

            return new SourceFolderDto
            {
                Path = entity.Path,
                Id = entity.Id
            };
        }

        // This method adds the appropriate mapping. It stores the id of the monitored folder (directoryId).
        // Then it stores the id for each file contained within that folder. The mapping can be used to know
        // the exact files contained within a folder during its first addition to the database. So if files
        // are added to that folder in the future then the program has a way to inform the user. 
        private void AddFolderFileMapping(string[] filePaths, int directoryId)
        {
            List<SourceFile> filesInMonitoredFolder = GetMonitoredFolderChildrenFiles(filePaths);
            foreach(SourceFile file in filesInMonitoredFolder)
            {
                _folderFileMappingRepository.Add(
                    new FolderFileMapping
                    {
                        SourceFileId = file.Id,
                        SourceFolderId = directoryId
                    }
                );
            }
            _folderFileMappingRepository.SaveChanges();
        }

        // Get all file paths from the database where the path matches the contents of "filePaths."
        private List<SourceFile> GetMonitoredFolderChildrenFiles(string[] filePaths)
            => _sourceFileRepository.GetRange(s => filePaths.Contains<string>(s.Path));

        private int GetDirectoryId(string directoryPath) 
            => _sourceFolderRepository.FirstOrDefault(f => f.Path == directoryPath).Id;
        

        /// <summary>
        /// Remove a range of source folders from the database. This method also removes any source files contained within that folder, as long as the files were not added individually.
        /// </summary>
        /// <param name="ids"> The Ids for each source folder path to be removed. </param>
        public void Remove(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                List<SourceFile> filesToRemove = GetStoredFilesFromFolder(id);
                _sourceFileRepository.RemoveRange(filesToRemove);
                _sourceFileRepository.SaveChanges();

                _sourceFolderRepository.Remove(new SourceFolder
                { 
                    Id = id 
                });
            }
            _sourceFolderRepository.SaveChanges();
        }

        /// <summary>
        /// Returns true if any monitored folder contains newly added files, false otherwise. 
        /// </summary>
        /// <param name="newFilesFromFolder"> An out parameter of type <see cref="Dictionary{TKey, TValue}"/> using <see cref="SourceFolderDto"/> objects for keys, and a list of strings for values. The values represent all files that have been added to the monitored folder. </param>
        public bool FoldersAreUpdated(
            out Dictionary<SourceFolderDto, List<string>>? newFilesFromFolder)
        {
            List<SourceFolderDto> folders = GetFolders();

            bool foldersAreUpdated = false;
            newFilesFromFolder = null;
            foreach (SourceFolderDto folder in folders)
            {
                string[] currentFiles = GetCurrentFilesFromFolder(folder);
                List<string> storedFiles = GetFilePaths(GetStoredFilesFromFolder(folder.Id));
                foreach(string file in currentFiles)
                {
                    if (storedFiles.Contains(file)) continue;
                    else
                    {
                        if(foldersAreUpdated == false) foldersAreUpdated = true;

                        // Dictionary will be null on the first attempt.
                        if (newFilesFromFolder == null)
                            newFilesFromFolder = new Dictionary<SourceFolderDto, List<string>>();

                        // If TryAdd returns true, add the key/value pair. Else, add the file to the currently existing key.
                        if(!newFilesFromFolder.TryAdd(folder, new List<string>() { file }))
                            newFilesFromFolder[folder].Add(file);
                    }
                }
            }
            return foldersAreUpdated;
        }

        // Get all current files and (including subdirectories) from the provided folder.
        private string[] GetCurrentFilesFromFolder(SourceFolderDto folder) 
            => Directory.GetFileSystemEntries(folder.Path, "*", SearchOption.AllDirectories);

        // Get all files stored in the database if they are mapped to a SourceFolder object. 
        private List<SourceFile> GetStoredFilesFromFolder(int folderId)
        {
            List<SourceFile> result = new List<SourceFile>();
            List<FolderFileMapping> mapping = _folderFileMappingRepository.GetRange(
                ffm => ffm.SourceFolderId == folderId);
            foreach (FolderFileMapping map in mapping)
            {
                SourceFile? file = _sourceFileRepository.FirstOrDefault(
                    f => f.Id == map.SourceFileId,
                    asNoTracking: false);
                if(file != null) result.Add(file);
            }
            return result;
        }

        // Get a list of the specified file paths from a SourceFile list.
        private List<string> GetFilePaths(List<SourceFile> files)
        {
            List<string> result = new List<string>();
            foreach (SourceFile file in files) result.Add(file.Path);
            return result;
        }

        /// <summary>
        /// Ensures that the service objects are properly disposed. Also calls <c>Dispose</c> on the repository objects.
        /// </summary>
        /// <param name="disposing"> Signifies that the object is not being disposed directly from the finalizer. </param>
        protected override void Dispose(bool disposing)
        {
            _sourceFileRepository.Dispose();
            _sourceFolderRepository.Dispose();
            _folderFileMappingRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
