using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DQB2NPCViewer.code;
using System.Linq;

namespace DQB2NPCViewer
{
    public partial class Window1 : Window
    {
        public string TextAdd { get; set; } = "N/A";
        public ushort ColourPicked { get; set; }
        public Brush ColorPickedB { get; set; }
        public bool Skin { get; set; }
        private ushort filter = 0;
        public Window1()
        {
            InitializeComponent();
            DataContext = this;
            this.SizeChanged += OnWindowSizeChanged;
        }

        public void CreateButtons()
        {
            ButtonGrid.Children.Clear();
            List<Colour> SortedList = null;
            switch (filter)
            {
                case 0:
                    SortedList = ListText.ColorList;
                    break;
                case 1:
                    SortedList = ListText.ColorList
                .Select(colour => new { Hex = colour.color, HSL = HexToHSL(colour.color), ID = colour.ID })
                .OrderBy(color => color.HSL.H)
                .ThenBy(color => color.HSL.S)
                .ThenBy(color => color.HSL.L)
                .Select(color => new Colour { color = color.Hex, ID = color.ID })
                .ToList();
                    break;
                case 2:
                    SortedList = ListText.ColorList
                .Select(colour => new { Hex = colour.color, HSL = HexToHSL(colour.color), ID = colour.ID })
                .OrderBy(color => color.HSL.S)
                .ThenBy(color => color.HSL.L)
                .ThenBy(color => color.HSL.H)
                .Select(color => new Colour { color = color.Hex, ID = color.ID })
                .ToList();
                    break;
                case 3:
                    SortedList = ListText.ColorList
                .Select(colour => new { Hex = colour.color, HSL = HexToHSL(colour.color), ID = colour.ID })
                .OrderBy(color => color.HSL.L)
                .ThenBy(color => color.HSL.S)
                .ThenBy(color => color.HSL.H)
                .Select(color => new Colour { color = color.Hex, ID = color.ID })
                .ToList();
                    break;
                case 4:
                    SortedList = ListText.ColorList
                .Select(colour => new { Hex = colour.color, HSL = HexToHSL(colour.color), ID = colour.ID})
                .OrderBy(color => (ushort)(color.HSL.S*3))
                .ThenBy(color => (ushort)(color.HSL.L*3))
                .ThenBy(color => (ushort)(color.HSL.H / 5))
                .ThenBy(color => color.HSL.L)
                .ThenBy(color => color.HSL.S)
                .Select(color => new Colour { color = color.Hex, ID = color.ID })
                .ToList();
                    break;
                case 5:
                    SortedList = ListText.ColorList
                .Select(colour => new { Hex = colour.color, HSL = HexToHSL(colour.color), ID = colour.ID })
                .OrderBy(color => (ushort)(color.HSL.H / 45))
                .ThenBy(color => color.HSL.S)
                .ThenBy(color => color.HSL.L)
                .Select(color => new Colour { color = color.Hex, ID = color.ID })
                .ToList();
                    break;
            }
            for (ushort i = 0; i < 999; i++)
            {
                //var color = ListText.getColorVal(i).color;
                var color = SortedList[i].color;
                if (Skin)
                {
                    color = HelixViewportModel.Multiply(color);
                }
                if (color.Equals("#000000") && SortedList[i].ID !=0) continue;
                // Create a new Button
                Button button = new Button
                {
                    Background = (SolidColorBrush)new BrushConverter().ConvertFromString(color),
                    //Tag = ListText.getColorVal(i).ID,
                    Tag = SortedList[i].ID,
                    Width = 20,
                    Height = 20,
                    ToolTip = SortedList[i].ID
                };

                // Attach the Click event handler
                button.Click += Button_Click;

                // Add the button to the UniformGrid
                ButtonGrid.Children.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                ColourPicked = (ushort)clickedButton.Tag;
                ColorText.Text = "Selected colour: {" + ColourPicked + "} " + clickedButton.Background;
                Confirm.Visibility = Visibility.Visible;
                ColorSelection.Fill = clickedButton.Background;
                ColorPickedB = clickedButton.Background;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            double newWindowHeight = e.NewSize.Height;
            double newWindowWidth = e.NewSize.Width;
            ButtonGrid.Columns = ((int)newWindowWidth - 50) / 20;
            ButtonGrid.Rows = 1000 / (((int)newWindowWidth - 50) / 20) + 1;
        }

        private (double H, double S, double L) HexToHSL(string hex)
        {
            Color color = (Color)ColorConverter.ConvertFromString(hex);
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));
            double delta = max - min;

            // Lightness
            double l = (max + min) / 2.0;

            // Saturation
            double s = (delta == 0) ? 0 : delta / (1 - Math.Abs(2 * l - 1));

            // Hue
            double h = 0;
            if (delta != 0)
            {
                if (max == r)
                    h = 60 * (((g - b) / delta + 6) % 6);
                else if (max == g)
                    h = 60 * (((b - r) / delta) + 2);
                else if (max == b)
                    h = 60 * (((r - g) / delta) + 4);
            }
            Console.WriteLine((ushort)(s * 20));
            return (H: h, S: s, L: l);
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            filter = ushort.Parse((sender as RadioButton).Tag.ToString());
            CreateButtons();
        }
    }
}
