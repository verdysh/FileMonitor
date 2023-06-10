using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class FullBackupPathRepository: RepositoryBase<BackupPath>, IFullBackupPathRepository
    {
        public FullBackupPathRepository(FileMonitorDbContext db) : base(db)
        {
        }
    }
}
