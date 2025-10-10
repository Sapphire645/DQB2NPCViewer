using DQB2NPCViewer.code;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for CharacterButton.xaml
    /// </summary>
    public partial class CharacterButton : UserControl
    {
        public ObservableProperty<NPCDataMinimum> NPC { get; private set; } = new ObservableProperty<NPCDataMinimum>();
        private event Action<NPCDataMinimum> ClickSend;

        public CharacterButton(NPCDataMinimum NPC, Action<NPCDataMinimum> ClickSend)
        {
            this.ClickSend = ClickSend;
            this.NPC.Value = NPC;
            InitializeComponent();
            ImageToChange.Source = NPC.Image;
        }

        public CharacterButton(NPCDataMinimum NPC)
        {
            this.NPC.Value = NPC;
            InitializeComponent();
            ImageToChange.Source = NPC.Image;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ClickSend.Invoke(NPC.Value);
        }
        public void UpdateImage()
        {
            ImageToChange.Source = NPC.Value.Image;
        }
    }
}
