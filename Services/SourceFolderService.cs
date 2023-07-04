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
        /// Adds a monitored folder to the database. This method ensures appropriate mapping from the source folder to all child files contained within it. <see cref="SourceFileService.Add(string)"/> must be called first on all new file paths before this method is called. This ensures that the file IDs are created, allowing for the folders and files to be mapped appropriately.
        /// </summary>
        /// <param name="directoryPath"> The folder to add to the database. </param>
        /// <param name="filePaths"> An array of all newly added file paths. </param>
        public void Add(string directoryPath, string[] filePaths)
        {
            _sourceFolderRepository.Add(new SourceFolder
            {
                Path = directoryPath,
            });
            _sourceFolderRepository.SaveChanges(); 
            AddFolderFileMapping(filePaths, GetDirectoryId(directoryPath));
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
        {
            List<SourceFolder> result = _sourceFolderRepository.GetRange(f => f.Path == directoryPath);
            return result[0].Id;
        }

        /// <summary>
        /// Remove a range of source folders from the database.
        /// </summary>
        /// <param name="ids"> The Ids for each source folder path to be removed. </param>
        public void Remove(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
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
        /// <param name="newFilesFromMonitoredFolder"> A <see cref="Dictionary{TKey, TValue}"/> using <see cref="SourceFolderDto"/> objects for keys, and a list of strings for values. The values represent all files that have been added to the monitored folder. </param>
        public bool FoldersAreUpdated(
            out Dictionary<SourceFolderDto, List<string>>? newFilesFromMonitoredFolder)
        {
            List<SourceFolderDto> folders = _sourceFolderRepository.GetRange(
                f => true,
                f => new SourceFolderDto
                {
                    Id = f.Id,
                    Path = f.Path
                },
                f => f.Id);

            bool foldersAreUpdated = false;
            newFilesFromMonitoredFolder = null;
            foreach (SourceFolderDto folder in folders)
            {
                string[] currentFiles = GetCurrentFiles(folder);
                List<string> storedFiles = GetStoredFiles(folder);
                foreach(string file in currentFiles)
                {
                    if (storedFiles.Contains(file)) continue;
                    else
                    {
                        foldersAreUpdated = true;
                        if(!newFilesFromMonitoredFolder.TryAdd(folder, new List<string>() { file }))
                            newFilesFromMonitoredFolder[folder].Add(file);
                    }
                }
            }
            return foldersAreUpdated;
        }

        // Get all current files and (including subdirectories) from the provided folder.
        private string[] GetCurrentFiles(SourceFolderDto folder) 
            => Directory.GetFileSystemEntries(folder.Path, "*", SearchOption.AllDirectories);

        // Get all files stored in the database if they are mapped to a SourceFolder object. 
        private List<string> GetStoredFiles(SourceFolderDto folder)
        {
            List<string> result = new List<string>();
            List<FolderFileMapping> mapping = _folderFileMappingRepository.GetRange(
                ffm => ffm.SourceFolderId == folder.Id);
            foreach (FolderFileMapping map in mapping)
            {
                SourceFileDto? file = _sourceFileRepository.FirstOrDefault(
                    f => f.Id == map.Id,
                    f => new SourceFileDto
                    {
                        Path = f.Path
                    });
                if(file != null) result.Add(file.Path);
            }
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
