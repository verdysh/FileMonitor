using DataAccessLayer.Entities;


namespace DataAccessLayer.Repositories
{
    public class SourceFileRepository : RepositoryBase<SourceFile>, ISourceFileRepository
    {
        public SourceFileRepository(FileMonitorDbContext db) : base(db)
        {
        }
    }
}
