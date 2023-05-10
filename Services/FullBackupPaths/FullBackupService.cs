using DataAccessLayer;
using DataAccessLayer.Entities;
using Services.FullBackupPaths.Dto;
using System.Configuration;

namespace Services.FullBackupPaths
{
    public class FullBackupService : DisposableService
    {
        private readonly FileMonitorDbContext _db;
        public FullBackupService()
        {
            _db = new FileMonitorDbContext(ConfigurationManager.ConnectionStrings[nameof(FileMonitorDbContext)].ConnectionString);
        }

        public List<FullBackupDto> GetPaths()
        {
            IQueryable<FullBackupDto> query = _db.FullBackupPaths.Select(obj => new FullBackupDto
            {
                Id = obj.Id,
                Path = obj.Path
            });

            return query.ToList();
        }

        public FullBackupDto Add(string path)
        {
            var entity = new FullBackupPath
            {
                Path = path
            };

            _db.FullBackupPaths.Add(entity);
            _db.SaveChanges();

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
                _db.FullBackupPaths.Remove(new FullBackupPath
                {
                    Id = id
                });
            }
            _db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Return true if the path exists in the database
        /// </summary>
        public bool PathExists(string path)
        {
            return _db.FullBackupPaths.Any(obj => obj.Path == path);
        }
    }
}
