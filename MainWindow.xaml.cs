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
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;
using DQB2NPCViewer.control;
using DQB2NPCViewer;

namespace DQB2NPCViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string NameNPC { get; set; } = "Name";
        public static string ConsoleText { get; set; } = "Hello world";
        public static ushort Sex { get; set; }
        public static ushort HP { get; set; }
        public static ushort Job { get; set; }
        public static ushort Type { get; set; }
        public static ushort Home { get; set; }
        public static ushort Island { get; set; }
        public static ushort Place { get; set; }
        public static ushort FaceModel { get; set; }
        public static ushort HairModel { get; set; }
        public static ushort BodyModel { get; set; }
        public static ushort EyeColor { get; set; }
        public static ushort HairColor { get; set; }
        public static ushort SkinColor { get; set; }
        public static ushort Dialogue { get; set; }
        public static ushort Voice { get; set; }
        public static ushort RoomSize { get; set; }
        public static ushort RoomFanciness { get; set; }
        public static ushort RoomAmbience { get; set; }
        public static List<Colour> ColorList { get; set; }

        public static ObservableCollection<ComboBoxBody> BodyList { get; set; }
        public static BodyModelClass SelectedBody { get; set; }
        public static ObservableCollection<ComboBoxHair> HairList { get; set; }
        public static BodyModelClass SelectedHair { get; set; }
        public static ObservableCollection<ComboBoxFace> FaceList { get; set; }
        public static BodyModelClass SelectedFace { get; set; }

        public MainWindow()
        {
                DataContext = this;
                InitializeComponent();
                TextBoxConsole.Text = "Hello World";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Black);
                CreateComboBoxTiles("data/jobs.json", 0);
                CreateComboBoxTiles("data/islands.json", 1);
                CreateComboBoxTiles("data/body.json", 2);
                CreateComboBoxTiles("data/hair.json", 3);
                CreateComboBoxTiles("data/face.json", 4);
                CreateComboBoxTiles("data/color.json", 5);
            DQB2ModelRendering.Rotate();
        }

        private void CreateComboBoxTiles(string Json,ushort Case)
        {
            try
            {
                var json = System.IO.File.ReadAllText(Json);
                switch (Case)
                {
                    case 0:
                        var jlist = JsonConvert.DeserializeObject<List<Job>>(json);
                        ComboBoxJob.ItemsSource = jlist;
                        ComboBoxJob.SelectedValuePath = "JobId";
                        break;
                    case 1:
                        var ilist = JsonConvert.DeserializeObject<List<Island>>(json);
                        ComboBoxIsland.ItemsSource = ilist;
                        ComboBoxIsland.SelectedValuePath = "IslandId";
                        ComboBoxHome.ItemsSource = ilist;
                        ComboBoxHome.SelectedValuePath = "IslandId";
                        break;
                    case 2:
                        var olist = JsonConvert.DeserializeObject<List<BodyModelClass>>(json);
                        BodyList = new ObservableCollection<ComboBoxBody>();
                        for (ushort i = 0; i < olist.Count; i++)
                        {
                            BodyList.Add(new ComboBoxBody
                            {
                                ID = i,
                                BodyModelClassV = olist[i]
                            });
                        }
                        break;
                    case 3:
                        var oolist = JsonConvert.DeserializeObject<List<HairModelClass>>(json);
                        HairList = new ObservableCollection<ComboBoxHair>();
                        for (ushort i = 0; i < oolist.Count; i++)
                        {
                            HairList.Add(new ComboBoxHair
                            {
                                ID = i,
                                HairModelClassV = oolist[i]
                            });
                        }
                        break;
                    case 4:
                        var ooolist = JsonConvert.DeserializeObject<List<FaceModelClass>>(json);
                        FaceList = new ObservableCollection<ComboBoxFace>();
                        for (ushort i = 0; i < ooolist.Count; i++)
                        {
                            FaceList.Add(new ComboBoxFace
                            {
                                ID = i,
                                FaceModelClassV = ooolist[i]
                            });
                        }
                        break;
                    default:
                        ColorList = JsonConvert.DeserializeObject<List<Colour>>(json);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error loading ComboBoxTiles!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = ""
                };

                if (openFileDialog.ShowDialog() == false)
                {
                    return;
                }

                DQB2DataEditor.LoadFile(openFileDialog.FileName);

                //I tried to do the weird binding thing but it doesn't work?? So wohoo copypaste time
                TextBoxName.Text = NameNPC;
                ComboBoxGender.SelectedIndex = (int)Sex-1;
                TextBoxHP.Text = Convert.ToString(HP);
                TextBoxVoice.Text = Convert.ToString(Voice);
                TextBoxDialogue.Text = Convert.ToString(Dialogue);
                ComboBoxJob.SelectedValue = Job;
                ComboBoxHome.SelectedValue = Home;
                ComboBoxIsland.SelectedValue = Island;
                ComboBoxPlace.SelectedIndex = Place;

                ButtonEye.Content = EyeColor;
                RectangleEye.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList[EyeColor].color);
                DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ColorList[EyeColor].color);
                
                ButtonSkin.Content = SkinColor;
                RectangleSkin.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList[SkinColor].color);
                DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ColorList[SkinColor].color);

                ButtonHair.Content = HairColor;
                RectangleHair.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList[HairColor].color);
                DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ColorList[HairColor].color);

                TextBoxConsole.Text = "Loaded File!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);

                ComboBoxBody.SelectedIndex = BodyModel;
                ComboBoxFace.SelectedIndex = FaceModel;
                ComboBoxHair.SelectedIndex = HairModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Failed to open file)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
                MessageBox.Show(ex.Message, "Failed to open file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (string.IsNullOrWhiteSpace(DQB2DataEditor.LoadedFile))
            {
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "",
                FileName = NameNPC
            };
            if (saveFileDialog.ShowDialog() == false)
            {
                return;
            }
            DQB2DataEditor.SaveFile(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
            Console.WriteLine(ex);
            MessageBox.Show(ex.Message, "Failed to save file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                NameNPC = TextBoxName.Text;
                TextBoxConsole.Text = "Changed Name to "+ NameNPC+ "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Name Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void TextBoxHP_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ushort.TryParse(TextBoxHP.Text, out var value))
            {
                TextBoxConsole.Text = "Invalid HP!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                HP = value;
                TextBoxConsole.Text = "Changed HP to " + TextBoxHP.Text + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        private void TextBoxVoice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ushort.TryParse(TextBoxVoice.Text, out var value))
            {
                TextBoxConsole.Text = "Invalid Voice!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                Voice = value;
                TextBoxConsole.Text = "Changed Voice to " + TextBoxVoice.Text + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        private void TextBoxDialogue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!ushort.TryParse(TextBoxDialogue.Text, out var value))
            {
                TextBoxConsole.Text = "Invalid Dialogue!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else
            {
                Dialogue = value;
                TextBoxConsole.Text = "Changed Dialogue to " + TextBoxDialogue.Text + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        private void ComboBoxGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Sex = (ushort)(ComboBoxGender.SelectedIndex + 1);
                TextBoxConsole.Text = "Changed sex to " + (ComboBoxGender.SelectedItem as ComboBoxItem).Content.ToString() + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Gender Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void ComboBoxJob_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Job = (ushort)ComboBoxJob.SelectedValue;
                TextBoxConsole.Text = "Changed job to " + ((Job)ComboBoxJob.SelectedItem).JobName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Job Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void ComboBoxHome_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Home = (ushort)ComboBoxHome.SelectedValue;
                TextBoxConsole.Text = "Changed native home to " + ((Island)ComboBoxHome.SelectedItem).IslandName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                HomeImageBox.Source = ChangeImage("images/island/" + Home + ".png");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Home Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void ComboBoxIsland_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Island = (ushort)ComboBoxIsland.SelectedValue;
                TextBoxConsole.Text = "Changed current home to " + ((Island)ComboBoxIsland.SelectedItem).IslandName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                IslandImageBox.Source = ChangeImage("images/island/"+Island+".png");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Island Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
            private BitmapImage ChangeImage(string imagePath)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.Relative);
                bitmap.EndInit();

                return bitmap;
            }

        private void ComboBoxPlace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Place = (ushort)ComboBoxPlace.SelectedIndex;
                TextBoxConsole.Text = "Changed current place to " + (ComboBoxPlace.SelectedItem as ComboBoxItem).Content.ToString() + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                PlaceImageBox.Source = ChangeImage("images/island/p" + Place + ".png");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Island Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void ButtonEye_Click(object sender, RoutedEventArgs e)
        {
            {
                Window1 ColorWindow = new Window1()
                {
                    TextAdd = "eye",
                    ColourPicked = EyeColor
                };

                if (ColorWindow.ShowDialog() == false)
                {
                    return;
                }
                EyeColor = ColorWindow.ColourPicked;
                ButtonEye.Content = EyeColor;
                RectangleEye.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList[EyeColor].color);
                DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ColorList[EyeColor].color);
                ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, true, false, false);
            }
        }
        private void ButtonSkin_Click(object sender, RoutedEventArgs e)
        {
            {
                Window1 ColorWindow = new Window1()
                {
                    TextAdd = "skin",
                    ColourPicked = SkinColor
                };

                if (ColorWindow.ShowDialog() == false)
                {
                    return;
                }
                SkinColor = ColorWindow.ColourPicked;
                ButtonSkin.Content = SkinColor;
                RectangleSkin.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList[SkinColor].color);
                DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ColorList[SkinColor].color);
                ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, true, false, true);
            }
        }
        private void ButtonHair_Click(object sender, RoutedEventArgs e)
        {
            {
                Window1 ColorWindow = new Window1()
                {
                    TextAdd = "hair",
                    ColourPicked = HairColor
                };

                if (ColorWindow.ShowDialog() == false)
                {
                    return;
                }
                HairColor = ColorWindow.ColourPicked;
                ButtonHair.Content = HairColor;
                RectangleHair.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList[HairColor].color);
                DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ColorList[HairColor].color);
                ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, false, true, false);
            }
        }

        private void ComboBoxBody_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BodyModel = (ushort)ComboBoxBody.SelectedIndex;
                TextBoxConsole.Text = "Changed body model to " + ((ComboBoxBody)ComboBoxBody.SelectedItem).BodyModelClassV.BodyName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, false, false, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (body model change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void ComboBoxFace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FaceModel = (ushort)ComboBoxFace.SelectedIndex;
                TextBoxConsole.Text = "Changed face model to " + ((ComboBoxFace)ComboBoxFace.SelectedItem).FaceModelClassV.FaceName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, true, false, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (face model change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void ComboBoxHair_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                HairModel = (ushort)ComboBoxHair.SelectedIndex;
                TextBoxConsole.Text = "Changed hair model to " + ((ComboBoxHair)ComboBoxHair.SelectedItem).HairModelClassV.HairName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, false, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (face model change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}