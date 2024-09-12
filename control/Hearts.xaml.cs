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

namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for Hearts.xaml
    /// </summary>
    public partial class Hearts : UserControl
    {
        private ushort ID { get; set; } = 0;
        public Hearts()
        {
            InitializeComponent();
        }

        public void HeartsCommand(ushort i, string type){
            ushort ie;
            ID = i;
            TextBlockHeart.Text = ID.ToString() + " - ";
            for (ie = 0; ie < ID; ie++) //ID
            {
                StackPanelHearts.Children.Add(new Image
                {
                    Source = new BitmapImage(new Uri("/images/resource/" + type + "Heart.png", UriKind.RelativeOrAbsolute)),
                    Height = 15,
                    Width = 15,
                    Margin = new Thickness(2,0,2,0)
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
