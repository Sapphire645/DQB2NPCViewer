using DQB2NPCViewer.code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for SelectionList.xaml
    /// </summary>
    public partial class TabList : UserControl
    {
        public event Action<NPCDataMinimum> ReturnSelectedTile;

        public List<NPCDataMinimum> StoryChar;
        public List<NPCDataMinimum> GenericChar;

        public MenuList StoryMenu;
        public MenuList GenericMenu;
        public TabList()
        {
            InitializeComponent();
        }
        public void createTabList()
        {
            StoryMenu = new MenuList(StoryChar);
            this.Story.Children.Add(StoryMenu);
            StoryMenu.ButtonClicked += ButtonClick;
            GenericMenu = new MenuList(GenericChar);
            this.Generic.Children.Add(GenericMenu);
            GenericMenu.ButtonClicked += ButtonClick;
        }
        public void ScrollViewUpdate(double height) {

                foreach (var Menu in Story.Children)
                {
                    ((MenuList)Menu).ScrollView.Height = height - 80;
                }
                foreach (var Menu in Generic.Children)
                {
                    ((MenuList)Menu).ScrollView.Height = height - 80;
                }

        }

        public NPCDataMinimum UpdateCharButton(ushort offset, NPCData NPC)
        {
            CharacterButton update;
            if (offset < 1024)
                update = StoryMenu.FindOffsetNPC(offset, new NPCDataMinimum(NPC.offset, NPC));
            else
                update = GenericMenu.FindOffsetNPC(offset, new NPCDataMinimum(NPC.offset, NPC));
            return update.NPC.Value;
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            var i = sender as NPCDataMinimum;
            ReturnSelectedTile?.Invoke(i);
        }
    }
}
