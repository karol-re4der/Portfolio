using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Portfolio.Models.Models;
using System.Reflection.Metadata;

namespace Portfolio.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Section>()
                .HasMany(e => e.Albums)
                .WithMany(e => e.Sections)
                .UsingEntity<AlbumSection>();

            modelBuilder.Entity<Photo>()
                .HasMany(e => e.Albums)
                .WithMany(e => e.Photos)
            .UsingEntity<AlbumPhoto>();

            modelBuilder.Entity<Album>()
                .HasOne(e => e.CoverPhoto)
                .WithMany(e => e.AlbumCovers)
                .HasForeignKey(e => e.CoverPhotoId);

            modelBuilder.Entity<Photo>()
                .HasOne(e => e.OriginalPhoto)
                .WithMany(e => e.DerivedPhotos)
                .HasForeignKey(e => e.OriginalPhotoId);
        }

        public DbSet<Photo> Photo { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<AlbumPhoto> AlbumPhoto { get; set; }
        public DbSet<AlbumSection> AlbumSection { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<ResolutionConfig> ResolutionConfig { get; set; }

    }
}
