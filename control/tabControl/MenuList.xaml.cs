using DQB2NPCViewer.code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for TileList.xaml
    /// </summary>
    public partial class MenuList : UserControl
    {
        public event EventHandler ButtonClicked;

        private List<CharacterButton> BlockListFull;

        private List<CharacterButton> CurrentListFull = new List<CharacterButton>();

        private List<CharacterButton> CurrentList = new List<CharacterButton>();

        private byte mIsland = 0;
        public byte CurrentIsland { get { return mIsland; } set {
                if (mIsland != value) {
                    if(value == 0)
                        AddBlocksFull((bool)FilterOne.IsChecked);
                    else
                        FilterItemsByIsland(value);
                    mIsland = value;
                }}}
        public MenuList(List<NPCDataMinimum> ListFilterFull)
        {
            BlockListFull = new List<CharacterButton>();
            InitializeComponent();
            CreateButtons(BlockListFull, ListFilterFull);
            AddBlocks(false);
        }
        private void CreateButtons(List<CharacterButton> ButtonList, List<NPCDataMinimum> BlockList)
        {
            for (int i = 0; i < BlockList.Count; i++)
            {
                var CharacterButton = new CharacterButton(BlockList[i]);
                ButtonList.Add(CharacterButton);
                CharacterButton.ClickSend += Button_Click;
                CurrentListFull.Add(CharacterButton);
            }
        }
        private void Button_Click(NPCDataMinimum sender)
        {
            ButtonClicked?.Invoke(sender, EventArgs.Empty);
        }
        public CharacterButton FindOffsetNPC(ushort offset, NPCDataMinimum New)
        {
            CharacterButton temp = null;
            foreach (var Char in BlockListFull)
                if (Char.NPC.Value.offset == offset)
                {
                    temp = Char;
                    break;
                }
                else if (Char.NPC.Value.offset > offset)
                    break;
            if (temp != null)
            {
                temp.NPC.Value = New;
                temp.NPC.NotifyValue();
                temp.UpdateImage();
            }
            return temp;
        }

        private void AddBlocks(bool addEmpty)
        {
            CurrentList.Clear();
            foreach (var block in CurrentListFull)
            {
                if (addEmpty || block.NPC.Value.charType != 0)
                    CurrentList.Add(block);
            }
            SortItems();
            var a = TextBoxFilter.Text.ToLower();
            FilterItems(a);
        }
        private void AddBlocksFull(bool addEmpty)
        {
            CurrentList.Clear();
            CurrentListFull.Clear();
            foreach (var block in BlockListFull)
            {
                CurrentListFull.Add(block);
                if (addEmpty || block.NPC.Value.charType != 0)
                    CurrentList.Add(block);
            }
            SortItems();
            var a = TextBoxFilter.Text.ToLower();
            FilterItems(a);
        }
        private void SortItems()
        {
            CurrentList = CurrentList.OrderBy(child => ((CharacterButton)child).NPC.Value.offset).ToList();

            Grid.Children.Clear();

            foreach (var child in CurrentList)
            {
                Grid.Children.Add(child);
            }
        }

        private void FilterItems(string Filter)
        {
            Grid.Children.Clear();
            foreach (var child in CurrentList)
            {
                var a = child.NPC.Value.name.ToLower();
                if (a.Contains(Filter))
                    Grid.Children.Add(child);
                else
                {
                    a = ListText.getTypeCharVal(child.NPC.Value.charType).name;
                    if (a.Contains(Filter))
                        Grid.Children.Add(child);
                }
            }
        }
        private void FilterItemsByIsland(ushort island)
        {
            CurrentListFull.Clear();
            foreach (var child in BlockListFull)
            {
                if (child.NPC.Value.island == island)
                    CurrentListFull.Add(child);
            }
            AddBlocks((bool)FilterOne.IsChecked);
        }
        private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var a = TextBoxFilter.Text.ToLower();
            FilterItems(a);
        }

        private void Empty_Checked(object sender, RoutedEventArgs e)
        {
            AddBlocks(true);
        }

        private void Empty_Unchecked(object sender, RoutedEventArgs e)
        {
            AddBlocks(false);
        }
    }
}
