using MediaApp.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.EL
{
    /// <summary>
    /// Represents the association between a specific media file and an album.
    /// </summary>
    /// <remarks>
    /// The MediaFileAlbum class is a junction entity designed to create a many-to-many relationship 
    /// between media files and albums. Each instance of this class captures the association of a single 
    /// media file being part of a specific album. This facilitates grouping and organizing media files 
    /// into collections or albums.
    /// </remarks>
    /// @Author Liza Danielsson
    public class MediaFileAlbum
    {
        [Key]
        public int Id { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public int MediaFileId { get; set; }
        public MediaFile MediaFile { get; set; }
    }

}
