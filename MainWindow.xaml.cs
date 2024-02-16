using MediaApp.BLL;
using MediaApp.EL;
using MediaApp.GUI;
using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaProgram.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// @Author Liza Danielsson.
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaManager mm = new MediaManager();
        private DBManager DBM = new DBManager();
        public MainWindow()
        {
            InitializeComponent();
            tvMedia.SelectedItemChanged += TvMedia_SelectedItemChanged;
            RefreshMediaDisplay(null);
        }


        /// <summary>
        /// Handles the "Select Media" button click, allowing the user to select one or multiple media files and 
        /// adding them to the Media Manager. After adding, the media display is refreshed.
        /// </summary>
        private void OnSelectMediaClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Media Files|*.jpg;*.png;*.mp4;*.Wav|All Files|*.*"
            };

            bool? result = openFileDialog.ShowDialog();

            if(result == true)
            {
                var albums = DBM.GetAllAlbums();
                bool allMediaExists = false;

                foreach(var album in albums)
                {
                    if(album.Name.Equals("All Media"))
                    {
                        allMediaExists = true;
                    }
                }
                if(!allMediaExists) { DBM.AddAlbum(new Album("All Media")); }
            }

            // Check if only one file is selected
            if (result == true && openFileDialog.FileNames.Length == 1)
            {
                string selectedFilePath = openFileDialog.FileNames[0];
                mm.AddMediaFile(selectedFilePath, "");
            }
            else if (openFileDialog.FileNames.Length > 1)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    mm.AddMediaFile(file, "");
                }
            }
            RefreshMediaDisplay(null);
        }



        /// <summary>
        /// Handles changes to the selected item in the media tree view. 
        /// Displays photos from the selected album or details of the selected media, as appropriate.
        /// </summary>
        private void TvMedia_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

            var selectedItem = e.NewValue as TreeViewItem;
            if (selectedItem != null)
            {
                if (selectedItem.Tag is Album selectedAlbum)
                {
                    // Display photos from the selected album in the preview area
                    PopulateImagePreviewArea(selectedAlbum);
                }
                else if (selectedItem.Tag is MediaFile selectedMedia)
                {
                    DisplayMediaDetails(selectedMedia.Location);
                }
                else
                {
                    PopulateImagePreviewArea(null);
                }
            }
        }



        /// <summary>
        /// Handles the mouse click on a preview image, displaying its media details when clicked.
        /// </summary>
        private void PreviewImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image selectedImage && selectedImage.Tag is string filePath)
            {
                DisplayMediaDetails(filePath);
            }
        }




        /// <summary>
        /// Handles the removal of media (album or individual media file) based on the selected item in 
        /// the media tree view.
        /// </summary>
        private void OnRemoveMediaClick(object sender, RoutedEventArgs e)
        {
            if (tvMedia.SelectedItem is TreeViewItem selectedItem)
            {
                MediaFile? selectedMediaFile = null;

                if (selectedItem.Tag is Album selectedAlbum)
                {
                    AlbumManager.RemoveAlbum(selectedAlbum);
                    RefreshMediaDisplay(null);
                    return;
                }
                // If the Tag is a string, treat it as a filePath
                else if (selectedItem.Tag is string filePath)
                {
                    selectedMediaFile = DBM.GetMediaDetailByPath(filePath);
                }
                // If the Tag is a MediaFile
                else if (selectedItem.Tag is MediaFile mediaFile)
                {
                    selectedMediaFile = mediaFile;
                }


                if (selectedMediaFile != null)
                {
                    // Check the parent of the selectedItem
                    TreeViewItem? parentItem = selectedItem.Parent as TreeViewItem;
                    if (parentItem != null)
                    {
                        if (parentItem.Header.ToString() == "All Media") // Check if it's the "All Media" node
                        {
                            mm.RemoveMediaFile(selectedMediaFile); // Remove from all media
                            RefreshMediaDisplay(null);
                        }
                        else if (parentItem.Tag is Album album) // Check if the parent represents an album
                        {
                            AlbumManager.RemoveMediaFromAlbum(album, selectedMediaFile); // Remove only from the specific album
                            RefreshMediaDisplay(album);
                        }
                    }
                    return;
                }
            }

            MessageBox.Show("No valid media file selected.");
        }




        /// <summary>
        /// Handles the creation of a new album, prompting the user for a name, 
        /// and then adds the new album to the media tree view.
        /// </summary>
        private void OnCreateAlbumClick(object sender, RoutedEventArgs e)
        {
            // Ask the user to name the album
            string albumName = ShowInputBox("New Album", "Enter the name of the album:");

            // Create a new album
            Album newAlbum = AlbumManager.CreateAlbum(albumName);

            // Add the new album to the TreeView
            TreeViewItem albumItem = CreateAlbumNode(newAlbum);

            tvMedia.Items.Add(albumItem);
            RefreshMediaDisplay(newAlbum);
            MessageBox.Show("Select and right click on an album to add media.");
        }

        /// <summary>
        /// Creates and returns a new TreeViewItem representing the provided album.
        /// </summary>
        private TreeViewItem CreateAlbumNode(Album album)
        {
              return new TreeViewItem
            {
                Header = album.Name,
                Tag = album
            };
        }



        /// <summary>
        /// Initiates the playing of a slideshow based on the selected album in the TreeView.
        /// The duration of the slideshow is determined by the value entered in the txtSlideshowDuration textbox.
        /// </summary>
        private void OnPlaySlideshowClick(object sender, RoutedEventArgs e)
        {
            if (tvMedia.SelectedItem is TreeViewItem selectedItem)
            {
                Album? selectedAlbum = selectedItem.Tag as Album;

                if (selectedAlbum != null)
                {
                    if (int.TryParse(txtSlideshowDuration.Text, out int duration))
                    {
                        var slideshow = SlideshowManager.CreateSlideshow(selectedAlbum, duration);

                        var playerWindow = new SlideshowPlayerWindow(slideshow);
                        playerWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid duration.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select an album to play as a slideshow.");
                }
            }
            else
            {
                MessageBox.Show("Please select an album to play as a slideshow.");
            }
        }






        /// <summary>
        /// Handles the right mouse button click event on the tvMedia TreeView. 
        /// Displays the "Add Media" menu item if the clicked item represents an album.
        /// </summary>
        private void tvMedia_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Check if the clicked item is an album
            if (tvMedia.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Album)
            {
                addMediaMenuItem.Visibility = Visibility.Visible;
            }
            else
            {
                addMediaMenuItem.Visibility = Visibility.Collapsed;
            }
        }




        /// <summary>
        /// Handles the click event for adding media files to a selected album.
        /// Presents a dialog to the user to select media files and then updates the TreeView to reflect the changes.
        /// </summary>
        private void OnAddMediaToAlbumClick(object sender, RoutedEventArgs e)
        {
            // Check if a TreeViewItem is selected and if it represents an Album
            if (tvMedia.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is Album selectedAlbum)
            {
                var allMedia = DBM.GetAllMedia();

                // Exclude media that are already in the album.
                var mediaInAlbum = DBM.GetMediaFilesForAlbum(selectedAlbum);
                var mediaNotInAlbum = allMedia.Except(mediaInAlbum).ToList();

                var mediaSelectionDialog = new MediaSelectionDialog(mediaNotInAlbum, selectedAlbum);

                if (mediaSelectionDialog.ShowDialog() == true)
                {
                    var selectedMediaFiles = mediaSelectionDialog.SelectedMediaFiles;

                    foreach (var media in selectedMediaFiles)
                    {
                        // Update the TreeViewItem to reflect the added media
                        TreeViewItem mediaItem = new TreeViewItem
                        {
                            Header = System.IO.Path.GetFileName(media.Location),
                            Tag = media
                        };

                        selectedItem.Items.Add(mediaItem);

                        var newAssociation = new MediaFileAlbum
                        {
                            MediaFileId = media.Id,
                            AlbumId = selectedAlbum.Id
                        };

                        DBM.AddMediaFileAlbum(newAssociation);
                    }
                    RefreshMediaDisplay(selectedAlbum);
                }
            }
            else
            {
                MessageBox.Show("Please select an album to add media.");
            }
        }



        /// <summary>
        /// Handles the click event for adding a description to a selected media file.
        /// Updates the description of the selected media and displays a success message upon completion.
        /// </summary>
        private void OnAddDescriptionClick(object sender, RoutedEventArgs e)
        {
            if (tvMedia.SelectedItem is TreeViewItem selectedItem)
            {
                // Check if the tag is a mediaFile.
                if (selectedItem.Tag is MediaFile mediaFile)
                {
                    bool updated = mm.UpdateMediaFileDescription(mediaFile, descriptionTextBox.Text);
                    descriptionTextBox.Text = "";
                    if (updated)
                    {
                        MessageBox.Show("Description updated successfully!");
                        DisplayMediaDetails(mediaFile.Location);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a media file to add a description.");
            }
        }



        /// <summary>
        /// Populates the main TreeView with all media files and albums retrieved from the database manager.
        /// The method organizes the items under a primary "All Media" node and individual album nodes.
        /// </summary>
        private void PopulateTreeView()
        {
            List<Album> allAlbums = DBM.GetAllAlbums();
            
            tvMedia.ItemsSource = null;
            tvMedia.Items.Clear();

            // Handle albums and their contents
            foreach (var album in allAlbums)
            {
                TreeViewItem albumNode = new TreeViewItem
                {
                    Header = album.Name,
                    Tag = album
                };

                if (album.Name.Equals("All Media"))
                {
                    var allMedia = DBM.GetAllMedia();

                    foreach (var media in allMedia)
                    {
                        TreeViewItem mediaItem = new TreeViewItem
                        {
                            Header = System.IO.Path.GetFileName(media.Location),
                            Tag = media
                        };
                        albumNode.Items.Add(mediaItem);
                    }

                }
                else
                {
                    var mediaInAblum = DBM.GetMediaFilesForAlbum(album);
                    foreach (var mediaFile in mediaInAblum)
                    {
                        TreeViewItem mediaItem = new TreeViewItem
                        {
                            Header = System.IO.Path.GetFileName(mediaFile.Location),
                            Tag = mediaFile
                        };

                        albumNode.Items.Add(mediaItem);
                    }

                }

                tvMedia.Items.Add(albumNode); // Add album nodes directly to the root
            }
        }



        /// <summary>
        /// Populates the image preview area with thumbnails of media files. 
        /// If a specific album is selected, it will display media from that album. 
        /// Otherwise, it will showcase all available media.
        /// </summary>
        private void PopulateImagePreviewArea(Album? selectedAlbum)
        {
            imagePreviewWrapPanel.Children.Clear();
            IEnumerable<MediaFile>? mediaFilesToDisplay = null;

            if (selectedAlbum != null)
            {
                // Display media from the selected album
                mediaFilesToDisplay = DBM.GetMediaFilesForAlbum(selectedAlbum);
            }
            if(selectedAlbum != null && selectedAlbum.Name.Equals("All Media"))
            {
                // Display all media files
                mediaFilesToDisplay = DBM.GetAllMedia();
            }

            foreach (var media in mediaFilesToDisplay)
            {
                Image previewImage = new Image
                {
                    Width = 100,
                    Height = 100,
                    Margin = new Thickness(5),
                    Tag = media.Location
                };

                // Check the type of media and set the image source accordingly
                if (media is Photo)
                {
                    previewImage.Source = new BitmapImage(new Uri(media.Location));
                }
                else if (media is Video)
                {
                    // Use the placeholder image for videos
                    previewImage.Source = new BitmapImage(new Uri("pack://application:,,,/MediaProgram.GUI;component/Resources/VideoIcon.png"));
                }

                imagePreviewWrapPanel.Children.Add(previewImage);
                previewImage.MouseUp += PreviewImage_MouseUp;

            }
        }



        /// <summary>
        /// Displays details of the specified media file in the data grid based on its file path.
        /// </summary>
        private void DisplayMediaDetails(string filePath)
        {
            var mediaDetail = DBM.GetMediaDetailByPath(filePath);
            dgMediaDetails.ItemsSource = new List<MediaFile> { mediaDetail };
        }



        /// <summary>
        /// Refreshes the display components of the media application, preserving the expanded state of tree nodes.
        /// </summary>
        private void RefreshMediaDisplay(Album? album)
        {
            List<string> previouslyExpandedNodes = GetExpandedNodes();

            PopulateTreeView();
            if(album != null) { PopulateImagePreviewArea(album); }
            dgMediaDetails.ItemsSource = null;

            SetExpandedNodes(previouslyExpandedNodes);
        }



        /// <summary>
        /// Retrieves a list of expanded nodes' headers in the TreeView.
        /// </summary>
        private List<string> GetExpandedNodes()
        {
            List<string> expandedNodes = new List<string>();

            foreach (TreeViewItem item in tvMedia.Items)
            {
                if (item.IsExpanded)
                {
                    expandedNodes.Add(item.Header.ToString());
                }
            }

            return expandedNodes;
        }



        /// <summary>
        /// Sets the expansion state of TreeView nodes based on a list of headers.
        /// </summary>
        private void SetExpandedNodes(List<string> expandedNodes)
        {
            foreach (TreeViewItem item in tvMedia.Items)
            {
                if (expandedNodes.Contains(item.Header.ToString()))
                {
                    item.IsExpanded = true;
                }
            }
        }



        /// <summary>
        /// Displays a modal input box to the user and returns the entered text.
        /// </summary>
        public static string ShowInputBox(string title, string promptText)
        {
            Window inputBox = new Window();
            inputBox.Width = 250;
            inputBox.Height = 150;
            inputBox.Title = title;

            StackPanel sp = new StackPanel();
            Label label = new Label { Content = promptText };
            TextBox textBox = new TextBox();
            Button okButton = new Button { Content = "OK" };
            Button cancelButton = new Button { Content = "Cancel" };

            bool wasCancelled = false;

            okButton.Click += (sender, e) => inputBox.Close();
            cancelButton.Click += (sender, e) =>
            {
                wasCancelled = true;
                inputBox.Close();
            };

            sp.Children.Add(label);
            sp.Children.Add(textBox);
            sp.Children.Add(okButton);
            sp.Children.Add(cancelButton);
            inputBox.Content = sp;

            inputBox.ShowDialog();

            if (wasCancelled)
                return null;

            return textBox.Text;
        }
    }
}
