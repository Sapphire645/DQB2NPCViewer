using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using DQB2NPCViewer.control;
using DQB2NPCViewer.code;
using System.Collections;
using System.Windows.Documents;
using System.Xml.Linq;

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
        public static ushort Weapon { get; set; }
        public static ushort Armour { get; set; }
        public static ListText Lists  { get; set; } = new ListText();
        public static bool TypeVisual { get; set; }

    public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            TextBoxConsole.Text = "Hello World";
            TextBoxConsole.Foreground = new SolidColorBrush(Colors.Black);
            InitializeComboBoxTiles();
            CreateComboBoxHearts();
            DQB2ModelRendering.Rotate();
        }

        private void InitializeComboBoxTiles()
        {
            Lists.setList("body","color","face","hair","islands","jobs","ambiance","typelock","weapon","armour");

            ComboBoxIsland.ItemsSource = Lists.IslandList;
            ComboBoxIsland.SelectedValuePath = "IslandId";

            ComboBoxHome.ItemsSource = Lists.IslandList;
            ComboBoxHome.SelectedValuePath = "IslandId";

            ComboBoxJob.ItemsSource = Lists.JobList;
            ComboBoxJob.SelectedValuePath = "JobId";

            AmbianceBox.ItemsSource = Lists.AmbianceList;

            ComboBoxTypeLock.ItemsSource = Lists.TypeLockList;
            ComboBoxTypeLock.SelectedValuePath = "typeID";

            ComboBoxBody.ItemsSource = Lists.BodyList;
            ComboBoxFace.ItemsSource = Lists.FaceList;
            ComboBoxHair.ItemsSource = Lists.HairList;

            WeaponBox.ItemsSource = Lists.WeaponList;
            WeaponBox.SelectedValuePath = "ID";
            ArmourBox.ItemsSource = Lists.ArmourList;
            ArmourBox.SelectedValuePath = "ID";

        }
            private void CreateComboBoxHearts()
        {
            try
            {
                        for (ushort i = 1; i < 6; i++)
                        {
                            var HeartsVar = new Hearts();
                            HeartsVar.HeartsCommand(i,"size");
                            SizeBox.Items.Add(HeartsVar);
                            HeartsVar = new Hearts();
                            HeartsVar.HeartsCommand(i, "fancy");
                            FancinessBox.Items.Add(HeartsVar);
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
                ComboBoxTypeLock.SelectedValue = Type;

                ArmourBox.SelectedValue = Armour;
                WeaponBox.SelectedValue = Weapon;

                var TypeBackup = TypeVisual;

                if ((ComboBoxTypeLock.SelectedItem as TypeSet).Tier == 3 && TypeVisual == true)
                {
                    TypeVisual = true;
                }else TypeVisual = false;


                SizeBox.SelectedIndex = RoomSize - 1;
                AmbianceBox.SelectedIndex = RoomAmbience - 1;
                FancinessBox.SelectedIndex = RoomFanciness - 1;

                ButtonEye.Content = EyeColor;
                var ColorList = Lists.getColorVal(EyeColor);
                RectangleEye.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList.color);
                DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ColorList.color);

                ButtonSkin.Content = SkinColor;
                ColorList = Lists.getColorVal(SkinColor);
                RectangleSkin.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList.color);
                DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ColorList.color);

                ButtonHair.Content = HairColor;
                ColorList = Lists.getColorVal(HairColor);
                RectangleHair.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList.color);
                DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ColorList.color);

                TextBoxConsole.Text = "Loaded File!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);

                ComboBoxBody.SelectedIndex = BodyModel;
                ComboBoxFace.SelectedIndex = FaceModel;
                ComboBoxHair.SelectedIndex = HairModel;

                LockCheck.IsChecked = TypeVisual;
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
            var TypeVisualBackup = TypeVisual;
            TypeVisual = (bool)LockCheck.IsChecked;
            DQB2DataEditor.SaveFile(saveFileDialog.FileName);
            TypeVisual = TypeVisualBackup;
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
                var ColorList = Lists.getColorVal(EyeColor);
                RectangleEye.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList.color);
                DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ColorList.color);
                if (TypeVisual == false) ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, true, false, false);
                else ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels((ComboBoxTypeLock.SelectedItem as TypeSet).faceID, HairModel, BodyModel, true, false, false);
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
                var ColorList = Lists.getColorVal(SkinColor);
                RectangleSkin.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList.color);
                DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ColorList.color);
                if (TypeVisual == false) ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, true, false, true);
                else ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels((ComboBoxTypeLock.SelectedItem as TypeSet).faceID, HairModel, (ComboBoxTypeLock.SelectedItem as TypeSet).bodyID, true, false, true);
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
                var ColorList = Lists.getColorVal(HairColor);
                RectangleHair.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList.color);
                DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ColorList.color);
                if (TypeVisual == false) ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, false, true, false);
                else ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, (ComboBoxTypeLock.SelectedItem as TypeSet).hairID, BodyModel, false, true, false);
            }
        }

        private void ComboBoxBody_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BodyModel = ((ComboBoxBody)ComboBoxBody.SelectedItem).ID;
                TextBoxConsole.Text = "Changed body model to " + ((ComboBoxBody)ComboBoxBody.SelectedItem).BodyModelClassV.BodyName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                if (TypeVisual == false) ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, false, false, true);
                else ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, (ComboBoxTypeLock.SelectedItem as TypeSet).bodyID, false, false, true);
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
                FaceModel = ((ComboBoxFace)ComboBoxFace.SelectedItem).ID;
                TextBoxConsole.Text = "Changed face model to " + ((ComboBoxFace)ComboBoxFace.SelectedItem).FaceModelClassV.FaceName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                if (TypeVisual == false) ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, true, false, false);
                else ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels((ComboBoxTypeLock.SelectedItem as TypeSet).faceID, HairModel, BodyModel, true, false, false);
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
                HairModel = ((ComboBoxHair)ComboBoxHair.SelectedItem).ID;
                TextBoxConsole.Text = "Changed hair model to " + ((ComboBoxHair)ComboBoxHair.SelectedItem).HairModelClassV.HairName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                if (TypeVisual == false) ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, HairModel, BodyModel, false, true, false);
                else ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceModel, (ComboBoxTypeLock.SelectedItem as TypeSet).hairID, BodyModel, false, true, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (face model change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void ComboBoxAmbiance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RoomAmbience = (AmbianceBox.SelectedItem as AmbianceBox).ID;
                TextBoxConsole.Text = "Changed room ambiance to " + (AmbianceBox.SelectedItem as AmbianceBox).AName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Room ambiance Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void SizeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RoomSize = (ushort)(SizeBox.SelectedIndex + 1);
                TextBoxConsole.Text = "Changed room size to " + (SizeBox.SelectedIndex+1).ToString()+ "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                SizeImage.Source = new BitmapImage(new Uri("/images/resource/size" + RoomSize.ToString() + ".png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Size Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void FancinessBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RoomFanciness = (ushort)(FancinessBox.SelectedIndex + 1);
                TextBoxConsole.Text = "Changed room fanciness to " + (FancinessBox.SelectedIndex + 1).ToString() + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                FancyImage.Source = new BitmapImage(new Uri("/images/resource/fancy" + RoomFanciness.ToString() + ".png",UriKind.Relative));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Fancy Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void ComboBoxTypeLock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Type = (ComboBoxTypeLock.SelectedItem as TypeSet).typeID;
                TextBoxConsole.Text = "Changed type to '" + (ComboBoxTypeLock.SelectedItem as TypeSet).name + " type'!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                if ((ComboBoxTypeLock.SelectedItem as TypeSet).Tier == 3)
                {
                    TypeVisual = true;
                }
                else TypeVisual = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Type Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void LockCheck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((ComboBoxTypeLock.SelectedItem as TypeSet).Tier == 3)
                {
                    TypeVisual = true;
                }
                ComboBoxBody_SelectionChanged(null, null);
                ComboBoxFace_SelectionChanged(null, null);
                ComboBoxHair_SelectionChanged(null, null);
            }
            catch { }

        }
        private void LockCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                TypeVisual = false;
                ComboBoxBody_SelectionChanged(null, null);
                ComboBoxFace_SelectionChanged(null, null);
                ComboBoxHair_SelectionChanged(null, null);
            }
            catch { }
        }

        private void WeaponBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Weapon = (WeaponBox.SelectedItem as Equipment).ID;
                TextBoxConsole.Text = "Changed weapon to " + (WeaponBox.SelectedItem as Equipment).Name + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Weapon Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void ArmourBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Armour = (ArmourBox.SelectedItem as Equipment).ID;
                TextBoxConsole.Text = "Changed armour to " + (ArmourBox.SelectedItem as Equipment).Name + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Armour Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}