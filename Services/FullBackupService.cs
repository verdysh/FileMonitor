using DataAccessLayer;
using DataAccessLayer.Entities;
using System.Configuration;
using Services.Dto;
using DataAccessLayer.Repositories;

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
            FullBackupPath entity = new FullBackupPath
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
                _repository.Remove(new FullBackupPath
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

        public void UpdateIsSelected(FullBackupDto dto)
        {
            var entity = _repository.FirstOrDefault(f => f.Id == dto.Id);
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
