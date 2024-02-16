using MediaApp.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MediaApp.GUI
{
    /// <summary>
    /// Represents a dedicated window for playing through photo slideshows.
    /// The SlideshowPlayerWindow provides views of a series of images in a slideshow format.
    /// 
    /// @Author Liza Danielsson.
    /// </summary>

    public partial class SlideshowPlayerWindow : Window
    {
        private readonly Slideshow _slideshow;
        private int _currentMediaIndex = 0;

        public SlideshowPlayerWindow(Slideshow slideshow)
        {
            InitializeComponent();
            _slideshow = slideshow;
            PlayNextMedia();
        }



        /// <summary>
        /// Advances the slideshow to the next media item, handling different types of media accordingly. 
        /// Closes the slideshow window if all media items have been displayed.
        /// </summary>
        private void PlayNextMedia()
        {
            if (_currentMediaIndex < _slideshow.MediaFiles.Count)
            {
                var currentMedia = _slideshow.MediaFiles[_currentMediaIndex];

                // Set the media source for the MediaElement
                mediaElement.Source = new Uri(currentMedia.Location);
                mediaElement.Play();

                if (currentMedia is Photo)
                {
                    // If it's a photo, set a timer for the display duration
                    var timer = new DispatcherTimer
                    {
                        Interval = _slideshow.PhotoDisplayDuration
                    };
                    timer.Tick += (s, e) =>
                    {
                        timer.Stop();
                        NextMedia();
                    };
                    timer.Start();
                }

                _currentMediaIndex++;
            }
            else
            {
                // End of slideshow logic, for instance close the window
                this.Close();
            }
        }


        /// <summary>
        /// Handles the event when the current media has finished playing. If the previous media was not a photo, 
        /// it advances the slideshow to the next media item.
        /// </summary>
        private void OnMediaEnded(object sender, RoutedEventArgs e)
        {
            if (_slideshow.MediaFiles[_currentMediaIndex - 1] is Photo) // ensure last media wasn't a photo
            {
                
            }
            else
            {
                PlayNextMedia();
            }
        }


        /// <summary>
        /// Stops the current media from playing and initiates the playback of the next media item.
        /// </summary>
        private void NextMedia()
        {
            mediaElement.Stop();
            PlayNextMedia();
        }
    }

}
