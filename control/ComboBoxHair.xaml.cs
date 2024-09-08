
using System.Windows.Controls;


namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for ComboBoxHair.xaml
    /// </summary>
    public partial class ComboBoxHair : UserControl
    {
        public ushort ID { get; set; } = 0;
        public HairModelClass HairModelClassV { get; set; }
        public ComboBoxHair()
        {
            InitializeComponent();
            DataContext = this;

        }
    }
}
