using Microsoft.EntityFrameworkCore;
using Portfolio.Models;

namespace Portfolio.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Photo> Photo { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<Section> Section { get; set; }
        public DbSet<AlbumPhoto> AlbumPhoto { get; set; }
        public DbSet<AlbumSection> AlbumSection { get; set; }

    }
}
