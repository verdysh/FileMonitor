using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public class FullBackupPathRepository: RepositoryBase<FullBackupPath>, IFullBackupPathRepository
    {
        public FullBackupPathRepository(FileMonitorDbContext db) : base(db)
        {
        }
    }
}
