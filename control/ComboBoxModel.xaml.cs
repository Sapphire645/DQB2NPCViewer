
using System.Windows.Controls;


namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for ComboBoxModel.xaml
    /// </summary>
    public partial class ComboBoxModel : UserControl
    {
        public ushort ID { get; set; } = 0;
        public ModelClass ModelClassV { get; set; }
        public ComboBoxModel()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
