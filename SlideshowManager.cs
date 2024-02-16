using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.BLL
{
    /// <summary>
    /// Handles operations related to creating and managing slideshows.
    /// </summary>
    /// <remarks>
    /// The SlideshowManager class offers a mechanism to construct slideshows based on provided albums. 
    /// It facilitates the creation of slideshows by including all media files from a selected album and setting a specified duration for the slideshow.
    /// </remarks>
    /// @Author Liza Danielsson
    public class SlideshowManager
    {
        private static DBManager DBM = new DBManager();
        public static Slideshow CreateSlideshow(Album selectedAlbum, int duration)
        {
            var slideshow = new Slideshow(TimeSpan.FromSeconds(duration));
            List<MediaFile> mediaFilesInAlbum = new List<MediaFile>();
            if (selectedAlbum.Name.Equals("All Media"))
            { 
                mediaFilesInAlbum = DBM.GetAllMedia();
            }
            else
            {
                mediaFilesInAlbum = DBM.GetMediaFilesForAlbum(selectedAlbum);
            }

            foreach (var mediaFile in mediaFilesInAlbum)
            {
                slideshow.AddMedia(mediaFile);
            }
            return slideshow;
        }
    }
}
