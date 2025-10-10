using DQB2NPCViewer.code;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for ComboBoxModel.xaml
    /// </summary>
    public partial class ComboBoxArmour : UserControl
    {
        public ushort ID { get; private set; } = 0;
        public Equipment Armour { get; private set; }
        public string Image { get; private set; }
        public Color Colour { get; private set; }

        public Visibility colorV { get; private set; } = Visibility.Collapsed; 
        public ComboBoxArmour(ushort ID, Equipment Armour, bool isArmour)
        {
            this.ID = ID;
            this.Armour = Armour;
            Image = Armour.Image;
            Colour = ListText.getColorDyeVal(Armour.ArmourValues.ColourIDMale);
            if (isArmour) colorV = Visibility.Visible;
            InitializeComponent();
            if (!isArmour) Column.Width = new GridLength(0);
            DataContext = this;
            this.ImageCalc.Source = new BitmapImage(new Uri(Image, UriKind.RelativeOrAbsolute));
            RectangleCol.Fill = new SolidColorBrush(Colour);
        }

        public void ChangeGender(bool female)
        {
            Image = Armour.ArmourValues.ImageFem;
            Colour = ListText.getColorDyeVal(Armour.ArmourValues.ColourIDFemale);

            this.ImageCalc.Source = new BitmapImage(new Uri(Image, UriKind.RelativeOrAbsolute));
            RectangleCol.Fill = new SolidColorBrush(Colour);
        }
    }
}
