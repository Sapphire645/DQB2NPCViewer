using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;


namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for ComboBoxModel.xaml
    /// </summary>
    public partial class ComboBoxArmour : UserControl
    {
        public ushort ID { get; set; } = 0;
        public Equipment Armour { get; set; }
        public string Image { get; set; }
        public Color Colour { get; set; }

        public Visibility colorV { get; set; } = Visibility.Collapsed; 
        public ComboBoxArmour(bool isArmour)
        {
            if (isArmour) colorV = Visibility.Visible;
            InitializeComponent();
            if (!isArmour) Column.Width = new GridLength(0);
            DataContext = this;
        }

        public void SetImage()
        {
            this.ImageCalc.Source = new BitmapImage(new Uri(Image, UriKind.RelativeOrAbsolute));
            RectangleCol.Fill = new SolidColorBrush(Colour);
        }
    }
}
