using Microsoft.EntityFrameworkCore;
using Portfolio.Models;
using System.Reflection.Metadata;

namespace Portfolio.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>()
                .HasMany(e => e.Albums)
                .WithMany(e => e.Sections)
                .UsingEntity<AlbumSection>();

            modelBuilder.Entity<Photo>()
                .HasMany(e => e.Albums)
                .WithMany(e => e.Photos)
            .UsingEntity<AlbumPhoto>();

            modelBuilder.Entity<Album>()
                .HasOne(e=>e.CoverPhoto)
                .WithMany(e=>e.AlbumCovers)
                .HasForeignKey(e => e.CoverPhotoId);
        }

        public DbSet<Photo> Photo { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<AlbumPhoto> AlbumPhoto { get; set; }
        public DbSet<AlbumSection> AlbumSection { get; set; }
    }
}
