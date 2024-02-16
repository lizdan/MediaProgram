using MediaApp.BLL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.DAL
{
    /// <summary>
    /// Provides data access and management functionality for media files within the application's database context.
    /// </summary>
    /// <remarks>
    /// The MediaFileRepository class facilitates CRUD operations for media files, interacting 
    /// directly with the database context. This repository acts as a bridge between the database and 
    /// the application logic, ensuring data integrity and consistency.
    /// </remarks>
    /// @Author Liza Danielsson
    public class MediaFileRepository
    {
        private readonly MediaAppDbContext _context;

        public MediaFileRepository(MediaAppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Retrieves all media files from the database.
        /// </summary>
        public List<MediaFile> GetAll()
        {
            using (MediaAppDbContext _dbContext = new MediaAppDbContext())
            {
                return _dbContext.MediaFiles.ToList();
            } 
        }


        /// <summary>
        /// Adds a new media file entry to the database.
        /// </summary>
        public void Add(MediaFile mediaFile)
        {
            using (MediaAppDbContext _dbContext = new MediaAppDbContext())
            {
                _dbContext.MediaFiles.Add(mediaFile);
                _dbContext.SaveChanges();
            }
        }


        /// <summary>
        /// Searches for, retrieves and returns a media file by its name or location path.
        /// </summary>
        public MediaFile GetMediaByNameOrPath(string name, string filePath)
        {
            using (MediaAppDbContext _dbContext = new MediaAppDbContext())
            {
                return _dbContext.MediaFiles.FirstOrDefault(m => m.Name == name || m.Location == filePath);
            }  
        }


        /// <summary>
        /// Retrieves and returns detailed information of a media file based on its location path.
        /// </summary>
        public MediaFile GetMediaDetailByPath(string filePath)
        {
            using (MediaAppDbContext _dbContext = new MediaAppDbContext())
            {
                return _dbContext.MediaFiles.FirstOrDefault(p => p.Location == filePath);
            }
        }


        /// <summary>
        /// Updates the details of an existing media file in the database.
        /// </summary>
        public void Update(MediaFile mediaFile)
        {
            _context.MediaFiles.Update(mediaFile);
            _context.SaveChanges();
        }


        /// <summary>
        /// Removes a media file entry from the database.
        /// </summary>
        public void Delete(MediaFile mediaFile)
        {
            _context.MediaFiles.Remove(mediaFile);
            _context.SaveChanges();
        }
    }
}
