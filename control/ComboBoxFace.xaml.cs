
using System.Windows.Controls;


namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for ComboBoxFace.xaml
    /// </summary>
    public partial class ComboBoxFace : UserControl
    {
        public ushort ID { get; set; } = 0;
        public FaceModelClass FaceModelClassV { get; set; }
        public ComboBoxFace()
        {
            InitializeComponent();
            DataContext = this;

        }
    }
}
