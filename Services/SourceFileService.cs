using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Services.Dto;
using Services.Helpers;

namespace Services
{
    /// <summary>
    /// A service class offering database access to the SourceFile Entity. This class stores a repository, and offers data transfer objects for updating the ViewModel.
    /// </summary>
    public class SourceFileService : DisposableService
    {
        private ISourceFileRepository _sourceFileRepository;
        private bool disposedValue;

        /// <summary>
        /// The <see cref="SourceFileService"/> class constructor. 
        /// </summary>
        /// <param name="sourceFileRepository"> An instance of <see cref="ISourceFileRepository"/> which provides database access. </param>
        public SourceFileService(ISourceFileRepository sourceFileRepository)
        {
            _sourceFileRepository = sourceFileRepository;
        }

        /// <summary>
        /// Returns all source file paths from the database.
        /// </summary>
        public List<SourceFileDto> GetFilePaths()
        {
            List<SourceFileDto> result = _sourceFileRepository.GetRange(
                s => true, 
                // Select all Entities where its properties match the Dto properties
                s => new SourceFileDto
                {
                    Id = s.Id,
                    Path = s.Path
                },
                s => s.Id
            );
            return result;
        }

        /// <summary>
        /// Returns all source file paths from the database if the IsModified property is set to true.
        /// </summary>
        public List<SourceFileDto> GetModifiedFilePaths()
        {
            RefreshModifiedFilePaths();
            List<SourceFileDto> result = _sourceFileRepository.GetRange(
                s => s.IsModified == true,
                s => new SourceFileDto
                {
                    Id = s.Id,
                    Path = s.Path
                },
                s => s.Id
            );
            return result;
        }

        /// <summary>
        /// Adds a source file path to the database.
        /// </summary>
        /// <param name="path"> The source file path to add to the database. </param>
        /// <returns> A source file DTO object for updating the UI. </returns>
        public SourceFileDto Add(string path)
        {
            SourceFile entity = new SourceFile
            {
                Path = path,
                Hash = EncryptionHelper.GetHash(path),
                IsModified = true
            };

            _sourceFileRepository.Add(entity);
            _sourceFileRepository.SaveChanges();

            return new SourceFileDto
            {
                Id = entity.Id,
                Path = entity.Path
            };
        }

        /// <summary>
        /// Remove a range of source file paths from the database.
        /// </summary>
        /// <param name="ids"> The Ids for each source file path to be removed. </param>
        public void Remove(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                _sourceFileRepository.Remove(
                    new SourceFile
                    {
                        Id = id
                    }
                );
            }
            _sourceFileRepository.SaveChanges();
        }

        /// <summary>
        /// Set the <see cref="SourceFile.IsModified"/> property to false on all the specified files. This class should be called when the files are first copied to a backup location.
        /// </summary>
        /// <param name="ids"> The Ids for each file where the <see cref="SourceFile.IsModified"/> property should be set to false. </param>
        public void ResetIsModifiedFlag(IEnumerable<int> ids)
        {
            _sourceFileRepository.Update(
                s => ids.Contains(s.Id), 
                s => s.IsModified = false); 
            _sourceFileRepository.SaveChanges();
        }

        /// <summary>
        /// Returns true if the path exists in the database, false otherwise.
        /// </summary>
        public bool PathExists(string path)
        {
            return _sourceFileRepository.Exists(s => s.Path == path);
        }

        // If FileIsUpdated() returns true, then set IsModified to true, and call SaveChanges on the repository.
        private void RefreshModifiedFilePaths()
        {
            List<SourceFile> files = _sourceFileRepository.GetRange(s => true);
            foreach (SourceFile file in files) 
            {
                if (FileIsUpdated(file.Path))
                {
                    file.IsModified = true;
                }
            }
            _sourceFileRepository.SaveChanges();
        }

        // Compare the stored hash code to the current hash code. If they are different, return true. Otherwise return false.
        private bool FileIsUpdated(string path)
        {
            SourceFile? sourceFile = _sourceFileRepository.FirstOrDefault(s => s.Path == path);
            if (sourceFile == null) return false;
            return EncryptionHelper.GetHash(path) != sourceFile.Hash;
        }

        public void UpdateHashesToCurrent(List<int> ids)
        {
            _sourceFileRepository.Update(
                s => ids.Contains(s.Id),
                s => s.Hash = EncryptionHelper.GetHash(s.Path));
            _sourceFileRepository.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _sourceFileRepository.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~FileService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
