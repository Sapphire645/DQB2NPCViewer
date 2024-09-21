using System.Windows.Controls;


namespace DQB2NPCViewer.control
{
    public partial class AmbianceBox : UserControl
    {
        public ushort ID { get; set; } = 0;
        public string AName { get; set; } = "N/A";

        public string Image => $"/images/resource/{ID:0}.png";
        public AmbianceBox()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}