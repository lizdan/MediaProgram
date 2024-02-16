

namespace MediaApp.BLL
{
    /// <summary>
    /// Defines methods for managing media files. This includes adding media from a file path,
    /// removing media entities, and updating descriptions of media files.
    /// Implementations of this interface should provide specific logic for media file operations.
    /// </summary>
    public interface IMediaManager
    {
        void AddMediaFile(string filePath, string description);
        void RemoveMediaFile(MediaFile mediaFile);
        bool UpdateMediaFileDescription(MediaFile media, string description);
    }
}
