using MediaApp.Utilities;

namespace MediaApp.BLL
{
    /// <summary>
    /// Provides management functionalities for media files including addition, removal, and updating of media.
    /// </summary>
    /// <remarks>
    /// The MediaManager class interfaces with the DBManager for database operations related to media files. 
    /// It is responsible for determining media types (Photo or Video) and ensuring the validity of media before performing operations.
    /// </remarks>
    /// @Author Liza Danielsson
    public class MediaManager : IMediaManager
    {
        private DBManager DBM = new DBManager();
        public void AddMediaFile(string filePath, string description)
        {
            var type = FileUtils.GetMediaType(filePath);
            string name = Path.GetFileName(filePath);

            // Check for existing media with the same name or filepath
            var existingMedia = DBM.GetMediaByNameOrPath(name, filePath);
            if (existingMedia != null)
            {
                return;
            }

            MediaFile mediaFile;
            if (type is FileUtils.MediaType.Photo)
            {
                mediaFile = new Photo(name, filePath, "");
            }
            else if (type is FileUtils.MediaType.Video)
            {
                mediaFile = new Video(name, filePath, "");
            }
            else
            {
                throw new InvalidOperationException("Unsupported media type.");
            }

            DBM.AddMedia(mediaFile);
        }

        
        public void RemoveMediaFile(MediaFile mediaFile)
        {
            DBM.RemoveMediaFile(mediaFile);
        }

       
        public bool UpdateMediaFileDescription(MediaFile media, string description)
        {
            bool updated = DBM.UpdateMediaFileDescription(media, description);
            return updated;
        }
    }
}
