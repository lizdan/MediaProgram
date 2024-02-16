using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaApp.BLL;
using MediaApp.EL;
using Microsoft.EntityFrameworkCore;

namespace MediaApp.DAL
{
    /// <summary>
    /// Provides the primary database context for the Media Application, facilitating the connection, 
    /// querying, and manipulation of data in relation to media files, albums, and their associations.
    /// </summary>
    /// <remarks>
    /// The MediaAppDbContext class extends the DbContext to manage the entity set up, database configurations, 
    /// and relationships. It also provides DbSet properties for entities to be queried and saved to the database. 
    /// This context establishes a connection to the 'MediaAppDb' on the local SQL server instance.
    /// Specific configuration options, such as discriminators and relationship mappings, are defined 
    /// within the OnModelCreating method.
    /// </remarks>
    /// @Author Liza Danielsson
    public class MediaAppDbContext : DbContext
    {
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<MediaFileAlbum> MediaFileAlbums { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MediaAppDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MediaFile>()
                .HasDiscriminator<string>("MediaType")
                .HasValue<Photo>("Photo")
                .HasValue<Video>("Video");

            // Change the foreign key from MediaFileName to MediaFileID
            modelBuilder.Entity<MediaFileAlbum>()
                .HasOne(mfa => mfa.MediaFile)
                .WithMany(mf => mf.MediaFileAlbums)
                .HasForeignKey(mfa => mfa.MediaFileId); // Updated this line

            modelBuilder.Entity<MediaFileAlbum>()
                .HasOne(mfa => mfa.Album)
                .WithMany(a => a.MediaFileAlbums)
                .HasForeignKey(mfa => mfa.AlbumId);
        }
    }
}
