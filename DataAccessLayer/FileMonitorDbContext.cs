using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    /// <summary>
    /// A DbContext class for accessing the database
    /// </summary>
    public class FileMonitorDbContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<SourceFile> SourceFiles { get; set; }
        public DbSet<BackupPath> FullBackupPaths { get; set; }
        public DbSet<BackupFile> BackupFiles { get; set; }

        public FileMonitorDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure all Path columns to be indexed in the database
            modelBuilder.Entity<SourceFile>().HasIndex(s => new { s.Path }).IsUnique();
            modelBuilder.Entity<BackupPath>().HasIndex(s => new { s.Path }).IsUnique();
        }
    }
}