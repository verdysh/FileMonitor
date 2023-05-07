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
        public DbSet<FullBackupPath> FullBackupPaths { get; set; }
        public DbSet<SequentialBackupPath> SequentialBackupPaths { get; set; }

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
            /// Make all paths unique
            modelBuilder.Entity<SourceFile>().HasIndex(s => new { s.Path}).IsUnique();
            modelBuilder.Entity<FullBackupPath>().HasIndex(s => new { s.Path }).IsUnique();
            modelBuilder.Entity<SequentialBackupPath>().HasIndex(s => new { s.Path }).IsUnique();
        }
    }
}