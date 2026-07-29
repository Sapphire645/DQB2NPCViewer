using DQB2NPCViewer.SaveData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DQB2NPCViewer.control
{
    /// <summary>
    /// Interaction logic for TileList.xaml
    /// </summary>
    public partial class MenuListType : UserControl
    {
        public event EventHandler ButtonClicked;

        private List<Button> BlockListFilter1;
        private List<Button> BlockListFilter2;
        private List<Button> BlockListFilter3;
        private List<Button> BlockListFull;
        public MenuListType(List<CHARlock> ListFilter1, string Filter1,
List<CHARlock> ListFilter2, string Filter2,
List<CHARlock> ListFilter3, string Filter3)
        {
            BlockListFilter1 = new List<Button>();
            BlockListFilter2 = new List<Button>();
            BlockListFilter3 = new List<Button>();
            BlockListFull = new List<Button>();
            InitializeComponent();
            FilterOne.Content = Filter1;
            FilterTwo.Content = Filter2;
            FilterThree.Content = Filter3;
            CreateButtons(BlockListFilter1, ListFilter1);
            CreateButtons(BlockListFilter2, ListFilter2);
            CreateButtons(BlockListFilter3, ListFilter3);
            AddBlocks(BlockListFilter1);
            AddBlocks(BlockListFilter2);
            AddBlocks(BlockListFilter3);
        }
        private void CreateButtons(List<Button> ButtonList, List<CHARlock> BlockList)
        {
            Brush Colour = new SolidColorBrush(Colors.White);
            for (int i = 0; i < BlockList.Count; i++)
            {
                if(BlockList[i].Models.Count == 0)
                {
                    Colour = new SolidColorBrush(Colors.Orange);
                }
                else
                {
                    Colour = new SolidColorBrush(Colors.White);
                }
                var ComboBoxColour = new Button()
                {
                    Content = new ComboBoxColour(BlockList[i], BlockList[i].ID),
                    Background = Colour
                };
                ComboBoxColour.Click += Button_Click;
                ButtonList.Add(ComboBoxColour);
                
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button Button = sender as Button;
            ComboBoxColour ComboBox = Button.Content as ComboBoxColour;
            ButtonClicked?.Invoke(ComboBox, EventArgs.Empty);
        }
        private void FilterOne_Checked(object sender, RoutedEventArgs e){AddBlocks(BlockListFilter1);}
        private void FilterOne_Unchecked(object sender, RoutedEventArgs e){ RemoveBlocks(BlockListFilter1); }
        private void FilterTwo_Checked(object sender, RoutedEventArgs e) { AddBlocks(BlockListFilter2); }
        private void FilterTwo_Unchecked(object sender, RoutedEventArgs e) { RemoveBlocks(BlockListFilter2); }
        private void FilterThree_Checked(object sender, RoutedEventArgs e) { AddBlocks(BlockListFilter3); }
        private void FilterThree_Unchecked(object sender, RoutedEventArgs e) { RemoveBlocks(BlockListFilter3); }

        private void AddBlocks(List<Button> List)
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
        private void RemoveBlocks(List<Button> List)
        {
            foreach (var block in List)
            {
                Grid.Children.Remove(block);
                BlockListFull.Remove(block);
            }
        }
        private void SortItems()
        {
            BlockListFull = BlockListFull.OrderBy(child => ((ComboBoxColour)((Button)child).Content).ID).ToList();

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
                var a = ((ComboBoxColour)child.Content).TypeListing.Name.ToLower();
                if (a.Contains(Filter))
                    Grid.Children.Add(child);
                else
                {
                    var b = ((ComboBoxColour)child.Content).ID;
                    if (b.ToString().Contains(Filter))
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
