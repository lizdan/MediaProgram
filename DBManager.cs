using MediaApp.DAL;
using MediaApp.EL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.BLL
{
    /// <summary>
    /// Manages in-memory storage of media files and albums.
    /// </summary>
    /// <remarks>
    /// The DBManager class provides a set of methods for adding, removing, and retrieving media files and albums. 
    /// It acts as an in-memory representation of a database, storing lists of media files and albums. 
    /// It supports operations like updating descriptions, fetching all media or albums, and accessing specific media details.
    /// </remarks>
    /// @Author Liza Danielsson
    public class DBManager
    {
        private readonly MediaFileRepository _mediaFileRepository;
        private readonly AlbumRepository _albumRepository;
        private readonly MediaFileAlbumRepository _mediaFileAlbumRepository;
        private List<MediaFile> mediaFiles;
        private List<Album> albums;

        public DBManager()
        {
            var dbContext = new MediaAppDbContext();
            _mediaFileRepository = new MediaFileRepository(dbContext);
            _albumRepository = new AlbumRepository(dbContext);
            _mediaFileAlbumRepository = new MediaFileAlbumRepository(dbContext); // Initialize it

            mediaFiles = _mediaFileRepository.GetAll();
            albums = _albumRepository.GetAll();
        }

        public void AddMedia(MediaFile mediaFile)
        {
            mediaFiles.Add(mediaFile);
            _mediaFileRepository.Add(mediaFile);
        }

        public void RemoveMediaFile(MediaFile mediaFile)
        {
            mediaFiles.Remove(mediaFile);
            _mediaFileRepository.Delete(mediaFile);
        }

        public bool UpdateMediaFileDescription(MediaFile mediaFile, string description)
        {
            var fileToUpdate = mediaFiles.FirstOrDefault(m => m.Id == mediaFile.Id);
            if (fileToUpdate != null)
            {
                fileToUpdate.Description = description;
                _mediaFileRepository.Update(fileToUpdate);
                return true;
            }
            return false;
            
        }

        public List<MediaFile> GetAllMedia()
        {
            mediaFiles = _mediaFileRepository.GetAll();
            return mediaFiles;
        }

        public MediaFile GetMediaDetailByPath(string filePath)
        {
            return _mediaFileRepository.GetMediaDetailByPath(filePath);
        }

        public MediaFile GetMediaByNameOrPath(string name, string filePath)
        {
            return _mediaFileRepository.GetMediaByNameOrPath(name, filePath);
        }


        public void AddAlbum(Album newAlbum)
        {
            albums.Add(newAlbum);
            _albumRepository.Add(newAlbum);
        }

        public void AddMediaFileAlbum(MediaFileAlbum mediaFileAlbum)
        {
            _mediaFileAlbumRepository.Add(mediaFileAlbum);
        }



        public List<MediaFile> GetMediaFilesForAlbum(Album album)
        {
            return _albumRepository.GetMediaFilesForAlbum(album);
        }


        public void RemoveMediaFromAlbum(Album album, MediaFile selectedMediaFile)
        {
            _albumRepository.RemoveMediaFromAlbum(album, selectedMediaFile);
        }

        public void RemoveAlbum(Album album)
        {
            albums.Remove(album);
            _albumRepository.Delete(album);
        }

        public List<Album> GetAllAlbums()
        {
            albums = _albumRepository.GetAll();
            return albums;
        }
    }

}
