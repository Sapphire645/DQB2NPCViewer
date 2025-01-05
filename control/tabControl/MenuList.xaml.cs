using DQB2NPCViewer.code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for TileList.xaml
    /// </summary>
    public partial class MenuList : UserControl
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler ButtonClicked;

        private List<CharacterButton> BlockListFilter1;
        private List<CharacterButton> BlockListFilter2;
        private List<CharacterButton> BlockListFilter3;
        private List<CharacterButton> BlockListFilter4;
        private List<CharacterButton> BlockListFull;
        public MenuList(List<NPCDataMinimum> ListFilter1, string Filter1,
List<NPCDataMinimum> ListFilter2, string Filter2,
List<NPCDataMinimum> ListFilter3, string Filter3, List<NPCDataMinimum> ListFilter4, string Filter4)
        {
            BlockListFilter1 = new List<CharacterButton>();
            BlockListFilter2 = new List<CharacterButton>();
            BlockListFilter3 = new List<CharacterButton>();
            BlockListFilter4 = new List<CharacterButton>();
            BlockListFull = new List<CharacterButton>();
            InitializeComponent();
            FilterOne.Content = Filter1;
            FilterTwo.Content = Filter2;
            FilterThree.Content = Filter3;
            FilterFour.Content = Filter4;
            CreateButtons(BlockListFilter1, ListFilter1);
            CreateButtons(BlockListFilter2, ListFilter2);
            CreateButtons(BlockListFilter3, ListFilter3);
            CreateButtons(BlockListFilter4, ListFilter4);
            AddBlocks(BlockListFilter1);
            AddBlocks(BlockListFilter2);
            AddBlocks(BlockListFilter3);
        }
        private void CreateButtons(List<CharacterButton> ButtonList, List<NPCDataMinimum> BlockList)
        {
            for (int i = 0; i < BlockList.Count; i++)
            {
                var CharacterButton = new CharacterButton(BlockList[i]);
                ButtonList.Add(CharacterButton);
                CharacterButton.ClickSend += Button_Click;
            }
        }
        private void Button_Click(NPCDataMinimum sender)
        {
            ButtonClicked?.Invoke(sender, EventArgs.Empty);
        }
        public CharacterButton FindOffsetNPC(ushort offset, NPCDataMinimum New)
        {
            List<List<CharacterButton>> FullList = new List<List<CharacterButton>>();
            FullList.Add(BlockListFilter1); FullList.Add(BlockListFilter2); FullList.Add(BlockListFilter3); FullList.Add(BlockListFilter4);
            CharacterButton temp = null;
            foreach (var List in FullList)
            {
                foreach (var Char in List)
                    if (Char.NPC.Value.offset == offset)
                    {
                        temp = Char;
                        break;
                    }
                    else if (Char.NPC.Value.offset > offset)
                        break;
                if (temp != null) break;
            }
            if (temp != null)
            {
                temp.NPC.Value = New;
                temp.NPC.NotifyValue();
                temp.UpdateImage();
            }
            return temp;
        }

        private void FilterOne_Checked(object sender, RoutedEventArgs e){AddBlocks(BlockListFilter1);}
        private void FilterOne_Unchecked(object sender, RoutedEventArgs e){ RemoveBlocks(BlockListFilter1); }
        private void FilterTwo_Checked(object sender, RoutedEventArgs e) { AddBlocks(BlockListFilter2); }
        private void FilterTwo_Unchecked(object sender, RoutedEventArgs e) { RemoveBlocks(BlockListFilter2); }
        private void FilterThree_Checked(object sender, RoutedEventArgs e) { AddBlocks(BlockListFilter3); }
        private void FilterThree_Unchecked(object sender, RoutedEventArgs e) { RemoveBlocks(BlockListFilter3); }
        private void FilterFour_Checked(object sender, RoutedEventArgs e) { AddBlocks(BlockListFilter4); }
        private void FilterFour_Unchecked(object sender, RoutedEventArgs e) { RemoveBlocks(BlockListFilter4); }

        private void AddBlocks(List<CharacterButton> List)
        {
            foreach (var block in List)
            { 
                BlockListFull.Add(block);
            }
            if (Grid != null)
            {
                SortItems();
                var a = TextBoxFilter.Text.ToLower();
                FilterItems(a); 
            }
        }
        private void RemoveBlocks(List<CharacterButton> List)
        {
            foreach (var block in List)
            {
                Grid.Children.Remove(block);
                BlockListFull.Remove(block);
            }
        }
        private void SortItems()
        {
            BlockListFull = BlockListFull.OrderBy(child => ((CharacterButton)child).NPC.Value.offset).ToList();

            Grid.Children.Clear();

            foreach (var child in BlockListFull)
            {
                Grid.Children.Add(child);
            }
        }
        private void FilterItems(string Filter)
        {
            Grid.Children.Clear();
            foreach (var child in BlockListFull)
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
        private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var a = TextBoxFilter.Text.ToLower();
            FilterItems(a);
        }
    }
}
