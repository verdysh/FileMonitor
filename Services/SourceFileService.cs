using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Services.Dto;
using Services.Helpers;

namespace Services
{
    public class SourceFileService : IDisposable
    {
        private ISourceFileRepository _sourceFileRepository;
        private bool disposedValue;

        public SourceFileService(ISourceFileRepository sourceFileRepository)
        {
            _sourceFileRepository = sourceFileRepository;
        }

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

        public void ResetIsModifiedFlag(IEnumerable<int> ids)
        {
            _sourceFileRepository.Update(
                s => ids.Contains(s.Id), 
                s => s.IsModified = false); 
            _sourceFileRepository.SaveChanges();
        }

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

        protected virtual void Dispose(bool disposing)
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
