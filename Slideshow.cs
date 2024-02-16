using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.BLL
{
    /// <summary>
    /// Represents a slideshow composed of multiple media files.
    /// </summary>
    /// <remarks>
    /// The Slideshow class encapsulates a sequence of media files, primarily designed 
    /// for presentation. Each media within the slideshow can be played/displayed for 
    /// a specified duration. The class provides functionality to add media to the slideshow.
    /// </remarks>
    /// @Author Liza Danielsson
    public class Slideshow
    {
        public List<MediaFile> MediaFiles;
        public TimeSpan PhotoDisplayDuration;
      

        public Slideshow(TimeSpan photoDisplayDuration)
        {
            MediaFiles = new List<MediaFile>();
            PhotoDisplayDuration = photoDisplayDuration;
        }

        public void AddMedia(MediaFile mediaFile)
        {
            MediaFiles.Add(mediaFile);
        }
    }
}
