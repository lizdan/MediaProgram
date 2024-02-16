using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.Utilities
{
    /// <summary>
    /// Provides utility methods for working with file operations, primarily to determine 
    /// the type of media based on its file extension.
    /// </summary>
    /// <remarks>
    /// The FileUtils class offers a way to detect if a file is a photo or a video by 
    /// inspecting its file extension. It recognizes standard extensions for photos and videos 
    /// and categorizes unrecognized types as unknown.
    /// </remarks>
    /// @Author Liza Danielsson
    public static class FileUtils
    {
        public static MediaType GetMediaType(string filePath)
        {
            string extension = System.IO.Path.GetExtension(filePath).ToLower();

            if (extension == ".jpg" || extension == ".png")
                return MediaType.Photo;

            if (extension == ".mp4" || extension == ".Wav")
                return MediaType.Video;

            return MediaType.Unknown;
        }

        public enum MediaType
        {
            Photo,
            Video,
            Unknown
        }
    }
}
