
using System.Windows.Controls;


namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for ComboBoxModel.xaml
    /// </summary>
    public partial class ComboBoxModel : UserControl
    {
        public ushort ID { get; private set; } = 0;
        public ModelClass ModelClassV { get; private set; }
        public ComboBoxModel(ushort ID, ModelClass ModelClassV)
        {
            this.ID = ID;
            this.ModelClassV = ModelClassV;
            InitializeComponent();
            DataContext = this;
        }
    }
}
