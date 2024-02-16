using MediaApp.EL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.DAL
{
    /// <summary>
    /// Provides data access and management functionality for media file and album associations within the application's database context.
    /// </summary>
    /// <remarks>
    /// The MediaFileAlbumRepository class enables operations for creating associations 
    /// between media files and albums, interacting directly with the database context. 
    /// This repository ensures proper linking of media files to specific albums.
    /// </remarks>
    /// @Author Liza Danielsson
    public class MediaFileAlbumRepository
    {
        private readonly MediaAppDbContext _context;

        public MediaFileAlbumRepository(MediaAppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Adds a new media file and album association entry to the database.
        /// </summary>
        public void Add(MediaFileAlbum mediaFileAlbum)
        {
            // Add and save the MediaFileAlbum association
            _context.MediaFileAlbums.Add(mediaFileAlbum);
            _context.SaveChanges();
        }
    }
}
