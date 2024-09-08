
using System.Windows.Controls;


namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for cOMBObOXbODY.xaml
    /// </summary>
    public partial class ComboBoxBody : UserControl
    {
        public ushort ID { get; set; } = 0;
        public BodyModelClass BodyModelClassV { get; set; }
        public ComboBoxBody()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
