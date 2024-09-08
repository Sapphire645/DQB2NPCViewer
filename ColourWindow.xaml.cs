using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DQB2NPCViewer
{
    public partial class Window1 : Window
    {
        public string TextAdd { get; set; } = "N/A";
        public ushort ColourPicked { get; set; }
        public Window1()
        {
            InitializeComponent();
            DataContext = this;
            CreateButtons();
            this.SizeChanged += OnWindowSizeChanged;
        }

        private void CreateButtons()
        {
            for (int i = 0; i < MainWindow.ColorList.Count; i++)
            {
                // Create a new Button
                Button button = new Button
                {
                    Background = (SolidColorBrush)new BrushConverter().ConvertFromString(MainWindow.ColorList[i].color),
                    Tag = MainWindow.ColorList[i].ID,
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

                ColorText.Text = "Selected colour: " + clickedButton.Background;
                Confirm.Visibility = Visibility.Visible;
                ColorSelection.Fill = clickedButton.Background;
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
    }
}
