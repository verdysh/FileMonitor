using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class BackupPathRepository: RepositoryBase<BackupPath>, IFullBackupPathRepository
    {
        public BackupPathRepository(FileMonitorDbContext db) : base(db)
        {
        }
    }
}
