
using DQB2NPCViewer.code;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using DQB2NPCViewer.SaveData;


namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for ComboBoxModel.xaml
    /// </summary>
    public partial class ComboBoxColour : UserControl
    {
        public ushort ID { get; private set; } = 0;
        public CHARlock TypeListing { get; private set; }
        //public ComboBoxColour()
        //{
        //    InitializeComponent();
        //    ImageToChange.Visibility = Visibility.Collapsed;
        //    DataContext = this;
        //}
        public ComboBoxColour(CHARlock TypeListing, ushort ID)
        {
            this.ID = ID;
            this.TypeListing = TypeListing;
            InitializeComponent();
            ImageToChange.Source = Image;
            DataContext = this;
        }
        public CroppedBitmap Image //Do not question my methods they are foolproof /j
        {
            get
            {
                var TypeVar = TypeListing.ID;
                if (TypeVar < 1 || TypeVar > 900) // Validate number range for a 5x5 grid
                {
                    Int32Rect iconRect2 = new Int32Rect(0, 0, 124, 124);
                    return new CroppedBitmap(ListText.NullImage, iconRect2);
                }
                try
                {
                    BitmapImage gridImage = ListText.IconImage;
                    // Calculate row and column in the 5x5 grid
                    int row = (TypeVar - 1) / 30;
                    int col = (TypeVar - 1) % 30;

                    // Calculate the x and y coordinates of the top-left corner of the icon
                    int x = col * 124;
                    int y = row * 124;

                    // Ensure the icon is within the grid's bounds
                    if (x + 124 > 3720 || y + 124 > 3720)
                    {
                        Int32Rect iconRect2 = new Int32Rect(0, 0, 124, 124);
                        return new CroppedBitmap(ListText.AnonImage, iconRect2);
                    }
                    // Crop the icon from the grid
                    Int32Rect iconRect = new Int32Rect(x, y, 124, 124);
                    var croppedIcon = new CroppedBitmap(gridImage, iconRect);
                    return croppedIcon;
                }
                catch
                {
                    Int32Rect iconRect2 = new Int32Rect(0, 0, 124, 124);
                    return new CroppedBitmap(ListText.AnonImage, iconRect2);
                }
            }
        }
    }
}
