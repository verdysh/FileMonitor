using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Services.Dto;

namespace Services
{
    public class FullBackupService : DisposableService
    {
        private IFullBackupPathRepository _repository;
        public FullBackupService(IFullBackupPathRepository repository)
        {
            _repository = repository;
        }

        public List<FullBackupDto> GetFullBackupRows()
        {
            List<FullBackupDto> result = _repository.GetMany(f => true, f => new FullBackupDto
            {
                Id = f.Id,
                Path = f.Path,
                IsSelected = f.IsSelected
            },
            f => f.Id);
            return result;
        }

        public FullBackupDto Add(string path)
        {
            BackupPath entity = new BackupPath
            {
                Path = path
            };

            _repository.Add(entity);
            _repository.SaveChanges();

            return new FullBackupDto
            {
                Id = entity.Id,
                Path = entity.Path
            };
        }

        public void Remove(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                _repository.Remove(new BackupPath
                {
                    Id = id
                });
            }
            _repository.SaveChanges();
        }

        public bool PathExists(string path)
        {
            return _repository.Exists(obj => obj.Path == path);
        }

        public void Update(FullBackupDto dto)
        {
            BackupPath entity = _repository.FirstOrDefault(f => f.Id == dto.Id, asNoTracking: false);
            if (entity == null) return;
            entity.Path = dto.Path;
            entity.IsSelected = dto.IsSelected;
            _repository.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            _repository.Dispose();
            base.Dispose(disposing);
        }
    }
}
