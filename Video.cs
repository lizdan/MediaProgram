using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaApp.BLL
{
    /// <summary>
    /// Represents a specific video file, extending the generic MediaFile class.
    /// </summary>
    /// <remarks>
    /// The Video class offers a concrete implementation for videos, building upon
    /// the generic MediaFile structure. It encompasses the necessary attributes to 
    /// uniquely identify and describe a video within the system.
    /// </remarks>
    /// @Author Liza Danielsson
    public class Video : MediaFile
    {
        public Video (string name, string location, string description)
        {
            Name = name;
            Location = location;
            Description = description;
            IsSelected = false;
        }
    }
}
