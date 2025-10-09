using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for Hearts.xaml
    /// </summary>
    public partial class Hearts : UserControl
    {
        public byte ID { get; set; } = 0;
        public Hearts(byte i, string type)
        {
            ID = i;
            InitializeComponent();
            if (ID != 0)
                HeartsCommand(type);
        }

        private void HeartsCommand(string type)
        {
            ushort ie;
            //TextBlockHeart.Text = ID.ToString() + " - ";
            for (ie = 0; ie < ID; ie++) //ID
            {
                StackPanelHearts.Children.Add(new Image
                {
                    Source = new BitmapImage(new Uri("/images/resource/" + type + "Heart.png", UriKind.RelativeOrAbsolute)),
                    Height = 15,
                    Width = 15,
                    Margin = new Thickness(2, 0, 2, 0)
                });
            }
            for (var a = ie; a < 5; a++) //ID
            {
                StackPanelHearts.Children.Add(new Image
                {
                    Source = new BitmapImage(new Uri("/images/resource/Heart.png", UriKind.RelativeOrAbsolute)),
                    Height = 15,
                    Width = 15,
                    Margin = new Thickness(2, 0, 2, 0)
                });
            }
        }
    }
}
