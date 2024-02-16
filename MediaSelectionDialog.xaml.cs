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

namespace MediaApp.GUI
{
    /// <summary>
    /// Interaction logic for MediaSelectionDialog.xaml
    /// 
    /// @Author Liza Danielsson.
    /// </summary>
    public partial class MediaSelectionDialog : Window
    {
        public List<MediaFile> SelectedMediaFiles { get; private set; } = new List<MediaFile>();
        public Album CurrentAlbum { get; set; }

        public MediaSelectionDialog(List<MediaFile> mediaFiles, Album currentAlbum)
        {
            InitializeComponent();
            lstMediaFiles.ItemsSource = mediaFiles;
            this.CurrentAlbum = currentAlbum;
        }


        /// <summary>
        /// Handles the click event of the "OK" button. Selects the media files from the list,
        /// adds them to the current album, and closes the dialog with a positive result.
        /// </summary>
        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            SelectedMediaFiles = lstMediaFiles.Items.Cast<MediaFile>().Where(m => m.IsSelected).ToList();
            this.DialogResult = true;
        }


        /// <summary>
        /// Handles the click event of the "Cancel" button, closing the dialog without making any changes.
        /// </summary>
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
