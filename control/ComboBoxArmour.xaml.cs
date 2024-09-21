using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;


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
        public ComboBoxArmour()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void SetImage()
        {
            this.ImageCalc.Source = new BitmapImage(new Uri(Image, UriKind.RelativeOrAbsolute));
            RectangleCol.Fill = new SolidColorBrush(Colour);
        }
    }
}
