using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    /// <summary>
    /// A DbContext class for accessing the database.
    /// </summary>
    public class FileMonitorDbContext : DbContext
    {
        private readonly string _connectionString;

        /// <summary>
        /// Provides a <see cref="DbSet{SourceFile}"/> for querying and saving changes to the <see cref="SourceFile"/> Entity. 
        /// </summary>
        public DbSet<SourceFile> SourceFiles { get; set; }

        /// <summary>
        /// Provides a <see cref="DbSet{BackupPath}"/> for querying and saving changes to the <see cref="BackupPath"/> Entity.
        /// </summary>
        public DbSet<BackupPath> FullBackupPaths { get; set; }

        /// <summary>
        /// The <see cref="FileMonitorDbContext"/> class constructor.
        /// </summary>
        /// <param name="connectionString"> A connection string for establishing a connection to the database. </param>
        public FileMonitorDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// An overridden method for configuring the database context to work with a SQLite database.
        /// </summary>
        /// <param name="optionsBuilder"> A <see cref="DbContextOptionsBuilder"/> object for configuring the database context. </param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }

        /// <summary>
        /// An overridden method for specifying how the Entities should map to the database.
        /// </summary>
        /// <param name="modelBuilder"> A <see cref="ModelBuilder"/> object for configuring the Entities. This object is used to ensure the <see cref="SourceFile"/> and <see cref="BackupPath"/> Entities are unique in the database. </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure all Path columns to be indexed in the database
            modelBuilder.Entity<SourceFile>().HasIndex(s => new { s.Path }).IsUnique();
            modelBuilder.Entity<BackupPath>().HasIndex(s => new { s.Path }).IsUnique();
        }
    }
}