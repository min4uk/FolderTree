using Microsoft.EntityFrameworkCore;

namespace FolderTree.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Folder> Folders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Folder>()
                .HasOne(f => f.ParrentFolder)
                .WithMany(f => f.SubFolders)
                .HasForeignKey(f => f.ParrentId)
                .IsRequired(false);
        }

    }
}
