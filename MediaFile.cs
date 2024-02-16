using MediaApp.EL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.BLL
{
    /// <summary>
    /// Represents a generic media file with essential metadata and properties.
    /// </summary>
    /// <remarks>
    /// The MediaFile class serves as a base representation for various media types, encapsulating
    /// common properties such as name, location, description, and selection status. Each media file
    /// is uniquely identified by its ID.
    /// </remarks>
    /// @Author Liza Danielsson
    public class MediaFile
    {
        [Key] 
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<MediaFileAlbum> MediaFileAlbums { get; set; }
    }
}
