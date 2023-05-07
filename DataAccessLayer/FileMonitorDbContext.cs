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
        public DbSet<FullBackup> FullBackups { get; set; }
        public DbSet<SequentialBackup> SequentialBackups { get; set; }

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
            modelBuilder.Entity<FullBackup>().HasIndex(s => new { s.Path }).IsUnique();
            modelBuilder.Entity<SequentialBackup>().HasIndex(s => new { s.Path }).IsUnique();
        }
    }
}