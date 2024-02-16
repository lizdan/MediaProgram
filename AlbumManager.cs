using MediaApp.DAL;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace MediaApp.BLL
{
    /// <summary>
    /// Manages operations related to albums, including creation and removal.
    /// </summary>
    /// <remarks>
    /// The AlbumManager class provides a set of static methods that interface with the DBManager for database operations related to albums. It handles creating albums, adding media to albums, removing albums, and other related operations.
    /// </remarks>
    /// @Author Liza Danielsson
    public class AlbumManager
    {
        private static DBManager DBM = new DBManager();    
        public static Album CreateAlbum(string albumName)
        {
            Album newAlbum = new Album(albumName);
            DBM.AddAlbum(newAlbum); 
            return newAlbum;
        }

        public static void RemoveMediaFromAlbum(Album album, MediaFile selectedMediaFile)
        {
            DBM.RemoveMediaFromAlbum(album, selectedMediaFile);
        }


        public static void RemoveAlbum(Album album)
        {
            DBM.RemoveAlbum(album);
        }
    }
}
