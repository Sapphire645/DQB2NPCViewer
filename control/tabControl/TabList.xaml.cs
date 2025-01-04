using DQB2NPCViewer.code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for SelectionList.xaml
    /// </summary>
    public partial class TabList : UserControl
    {
        public event Action<NPCDataMinimum> ReturnSelectedTile;

        public List<List<NPCDataMinimum>> StoryChar;
        public List<List<NPCDataMinimum>> GenericChar;

        private List<NPCDataMinimum> FullList;

        public MenuList StoryMenu;
        public MenuList GenericMenu;
        public TabList()
        {
            InitializeComponent();
        }
        public void createTabList()
        {
            StoryMenu = new MenuList(StoryChar.ElementAt(0), "Human", StoryChar.ElementAt(1), "Animal", StoryChar.ElementAt(2), "Monster", StoryChar.ElementAt(3), "NULL");
            this.Story.Children.Add(StoryMenu);
            StoryMenu.ButtonClicked += ButtonClick;
            GenericMenu = new MenuList(GenericChar.ElementAt(0), "Human", GenericChar.ElementAt(1), "Animal", GenericChar.ElementAt(2), "Monster", GenericChar.ElementAt(3), "NULL");
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
