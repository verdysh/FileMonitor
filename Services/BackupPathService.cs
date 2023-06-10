using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using Services.Dto;

namespace Services
{
    public class BackupPathService : DisposableService
    {
        private IBackupPathRepository _repository;
        public BackupPathService(IBackupPathRepository repository)
        {
            _repository = repository;
        }

        public List<BackupPathDto> GetFullBackupRows()
        {
            List<BackupPathDto> result = _repository.GetRange(f => true, f => new BackupPathDto
            {
                Id = f.Id,
                Path = f.Path,
                IsSelected = f.IsSelected
            },
            f => f.Id);
            return result;
        }

        public BackupPathDto Add(string path)
        {
            BackupPath entity = new BackupPath
            {
                Path = path
            };

            _repository.Add(entity);
            _repository.SaveChanges();

            return new BackupPathDto
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

        public void Update(BackupPathDto dto)
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
