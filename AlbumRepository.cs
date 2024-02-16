using MediaApp.BLL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.DAL
{
    /// <summary>
    /// Provides data access and management functionality for albums within the application's database context.
    /// </summary>
    /// <remarks>
    /// The AlbumRepository class facilitates operations related to albums, including retrieval, 
    /// addition, and deletion. It also manages the relationship between media files and albums, 
    /// ensuring data integrity and consistency within the database.
    /// </remarks>
    /// @Author Liza Danielsson
    public class AlbumRepository
    {
        private readonly MediaAppDbContext _context;

        public AlbumRepository(MediaAppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Retrieves all albums from the database.
        /// </summary>
        public List<Album> GetAll()
        {
            using (MediaAppDbContext _dbContext = new MediaAppDbContext())
            {
                return _dbContext.Albums.ToList();
            }     
        }


        /// <summary>
        /// Adds a new album to the database.
        /// </summary>
        public void Add(Album album)
        {
            MediaAppDbContext _dbcontext = new MediaAppDbContext();
            _dbcontext.Albums.Add(album);
            _dbcontext.SaveChanges(); 
        }


        /// <summary>
        /// Retrieves media files associated with a specific album.
        /// </summary>
        public List<MediaFile> GetMediaFilesForAlbum(Album album)
        {
            using (MediaAppDbContext _dbContext = new MediaAppDbContext())
            {
                var mediaFileNames = _dbContext.MediaFileAlbums
                                     .Where(mfa => mfa.AlbumId == album.Id)
                                     .Select(mfa => mfa.MediaFileId)
                                     .ToList();

                var mediaFiles = _dbContext.MediaFiles.Where(m => mediaFileNames.Contains(m.Id)).ToList();
                return mediaFiles;
            }
            
        }


        /// <summary>
        /// Removes a specific media file from an album.
        /// </summary>
        public void RemoveMediaFromAlbum(Album album, MediaFile selectedMediaFile)
        {
            var mediaFileAlbumToRemove = _context.MediaFileAlbums
                                         .FirstOrDefault(mfa => mfa.AlbumId == album.Id && mfa.MediaFileId == selectedMediaFile.Id);

            if (mediaFileAlbumToRemove != null)
            {
                _context.MediaFileAlbums.Remove(mediaFileAlbumToRemove);
                _context.SaveChanges();
            }
        }



        /// <summary>
        /// Deletes a specified album and associated records from the database.
        /// </summary>
        public void Delete(Album album)
        {
            using (MediaAppDbContext _dbContext = new MediaAppDbContext())
            {
                // Find all MediaFileAlbum records associated with the given album
                var mediaFileAlbumsToRemove = _dbContext.MediaFileAlbums
                                                        .Where(mfa => mfa.AlbumId == album.Id)
                                                        .ToList();

                // Remove the MediaFileAlbum records
                _dbContext.MediaFileAlbums.RemoveRange(mediaFileAlbumsToRemove);

                // Remove the album itself
                var albumToRemove = _dbContext.Albums.FirstOrDefault(a => a.Id == album.Id);
                if (albumToRemove != null)
                {
                    _dbContext.Albums.Remove(albumToRemove);
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
