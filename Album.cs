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
    /// Represents a collection of media files grouped under a specific album name.
    /// </summary>
    /// <remarks>
    /// The Album class provides basic functionalities for managing a collection of media files.
    /// It supports adding individual media files or a range of media files to the album. 
    /// Each album is identified by a unique name and contains a list of media files associated with it.
    /// </remarks>
    /// @Author Liza Danielsson
    public class Album
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<MediaFileAlbum> MediaFileAlbums { get; set; }


        public Album (string name)
        {
            Name = name;
        }
    }
}
