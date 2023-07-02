using Microsoft.EntityFrameworkCore;

namespace FolderTree.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Folder> Folders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name);
                entity.HasOne(x => x.ParrentFolder)
                    .WithMany(x => x.SubFolders)
                    .HasForeignKey(x => x.ParrentId)
                    .IsRequired(false);
            });
        }

    }
}
