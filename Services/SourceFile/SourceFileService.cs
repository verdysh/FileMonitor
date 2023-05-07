using DataAccessLayer;
using DataAccessLayer.Entities;
using Services.SourceFiles.Dto;
using System.Configuration;

namespace Services.SourceFiles
{
    public class SourceFileService: DisposableService
    {
        private readonly FileMonitorDbContext _db;
        public SourceFileService()
        {
            _db = new FileMonitorDbContext(ConfigurationManager.ConnectionStrings[nameof(FileMonitorDbContext)].ConnectionString);
        }

        public List<SourceFileDto> GetFiles()
        {
            var query = _db.SourceFiles.Select(s => new SourceFileDto
            {
                Id = s.Id,
                Path = s.Path
            });
            return query.ToList();
        }

        public SourceFileDto Add(string path)
        {
            /// Return null if value exists in the database
            if (_db.SourceFiles.Any(obj => obj.Path == path)) return null;
            else
            {
                var entity = new SourceFile
                {
                    Path = path
                };

                _db.SourceFiles.Add(entity);
                _db.SaveChanges();

                return new SourceFileDto
                {
                    Id = entity.Id,
                    Path = entity.Path
                };
            }
        }

        public void Remove(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                _db.SourceFiles.Remove(new SourceFile 
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
    }
}
