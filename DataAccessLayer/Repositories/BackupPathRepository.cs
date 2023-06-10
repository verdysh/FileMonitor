using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class BackupPathRepository: RepositoryBase<BackupPath>, IBackupPathRepository
    {
        public BackupPathRepository(FileMonitorDbContext db) : base(db)
        {
        }
    }
}
