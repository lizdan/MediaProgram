using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.BLL
{
    /// <summary>
    /// Represents a specific photo file, extending the generic MediaFile class.
    /// </summary>
    /// <remarks>
    /// The Photo class offers a concrete implementation for photos, building upon
    /// the generic MediaFile structure. It encompasses the necessary attributes to 
    /// uniquely identify and describe a photo within the system.
    /// </remarks>
    /// @Author Liza Danielsson
    public class Photo : MediaFile
    {
        public Photo (string name, string location, string description)
        {
            Name = name;
            Location = location;
            Description = description;
            IsSelected = false;
        }
    }
}
