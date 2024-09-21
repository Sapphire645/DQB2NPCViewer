using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DQB2NPCViewer
{
    public partial class Window1 : Window
    {
        public string TextAdd { get; set; } = "N/A";
        public ushort ColourPicked { get; set; }
        public Brush ColorPickedB { get; set; }
        public bool Skin { get; set; }
        public Window1()
        {
            InitializeComponent();
            DataContext = this;
            this.SizeChanged += OnWindowSizeChanged;
        }

        public void CreateButtons()
        {
            for (ushort i = 0; i < 999; i++)
            {
                var color = MainWindow.Lists.getColorVal(i).color;
                if (Skin)
                {
                    color = Multiply(color);
                }
                // Create a new Button
                Button button = new Button
                {
                    Background = (SolidColorBrush)new BrushConverter().ConvertFromString(color),
                    Tag = MainWindow.Lists.getColorVal(i).ID,
                    Width = 20,
                    Height = 20
                };

                // Attach the Click event handler
                button.Click += Button_Click;

                // Add the button to the UniformGrid
                ButtonGrid.Children.Add(button);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the button that was clicked
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                // Get the button number from its Tag property
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


        private string Multiply(string hexColor2)
        {
            string hexColor1 = "#EFC294";

            // Convert the hex strings to RGB components
            (int r1, int g1, int b1) = HexToRGB(hexColor1);
            (int r2, int g2, int b2) = HexToRGB(hexColor2);

            // Apply the multiply filter
            int rResult = MultiplyColors(r1, r2);
            int gResult = MultiplyColors(g1, g2);
            int bResult = MultiplyColors(b1, b2);

            // Convert the result back to a hex color
            return RGBToHex(rResult, gResult, bResult);
        }

        private (int, int, int) HexToRGB(string hex)
        {
            // Remove the # if present
            hex = hex.TrimStart('#');

            // Convert hex to integer for R, G, B
            int r = Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = Convert.ToInt32(hex.Substring(4, 2), 16);

            return (r, g, b);
        }

        private int MultiplyColors(int component1, int component2)
        {
            // Multiply the components and divide by 255 to normalize the result
            return (component1 * component2) / 255;
        }

        private string RGBToHex(int r, int g, int b)
        {
            // Convert the RGB values back to hex
            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
