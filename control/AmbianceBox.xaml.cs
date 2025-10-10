using System.Windows.Controls;


namespace DQB2NPCViewer.control
{
    public partial class AmbianceBox : UserControl
    {
        public byte ID { get; private set; } = 0;
        public string AName { get; set; } = "N/A";

        public string Image => $"/images/resource/{ID:0}.png";
        public AmbianceBox(byte id)
        {
            ID = id;
            InitializeComponent();
            if (ID == 0)
            {
                disable.Visibility = System.Windows.Visibility.Collapsed;
                disable.Source = null;
            }
            
            DataContext = this;
        }
    }
}