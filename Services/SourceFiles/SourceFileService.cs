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
            }).OrderBy(s => s.Id);
            return query.ToList();
        }

        public SourceFileDto Add(string path)
        {
            var entity = new SourceFile
            {
                Path = path,
                Hash = EncryptionHelper.GetHash(path)
            };

            _db.SourceFiles.Add(entity);
            _db.SaveChanges();

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

        /// <summary>
        /// Return true if the source file exists in the database
        /// </summary>
        public bool PathExists(string path)
        {
            return _db.SourceFiles.Any(s => s.Path == path);
        }
    }
}
