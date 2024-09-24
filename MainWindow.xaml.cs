using DQB2NPCViewer.code;
using DQB2NPCViewer.control;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DQB2NPCViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string NameNPC { get; set; } = "Name";
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
        public static ListText Lists { get; set; } = new ListText();
        public static bool TypeVisual { get; set; }
        public static bool ClothVisual { get; set; }
        public static bool RagVisual { get; set; }
        public static bool Loading { get; set; } = false;
        public static Equipment ColorBackup { get; set; } = new Equipment();

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            DQB2ModelRendering.ModelCodeC();
            TextBoxConsole.Text = "Hello World";
            TextBoxConsole.Foreground = new SolidColorBrush(Colors.Black);
            InitializeComboBoxTiles();
            CreateComboBoxHearts();
            DQB2ModelRendering.Rotate();
        }

        private void InitializeComboBoxTiles()
        {
            Lists.setList("body", "color", "face", "hair", "islands", "jobs", "ambiance", "typelock", "weapon", "armour");

            ComboBoxIsland.ItemsSource = Lists.IslandList;
            ComboBoxIsland.SelectedValuePath = "IJId";

            ComboBoxHome.ItemsSource = Lists.IslandList;
            ComboBoxHome.SelectedValuePath = "IJId";

            ComboBoxJob.ItemsSource = Lists.JobList;
            ComboBoxJob.SelectedValuePath = "IJId";

            AmbianceBox.ItemsSource = Lists.AmbianceList;

            ComboBoxTypeLock.ItemsSource = Lists.TypeLockList;
            ComboBoxTypeLock.SelectedValuePath = "ID";

            ComboBoxBody.ItemsSource = Lists.BodyList;
            ComboBoxBody.SelectedValuePath = "ModelClassV.ID";
            ComboBoxFace.ItemsSource = Lists.FaceList;
            ComboBoxFace.SelectedValuePath = "ModelClassV.ID";
            ComboBoxHair.ItemsSource = Lists.HairList;
            ComboBoxHair.SelectedValuePath = "ModelClassV.ID";

            ArmourBox.ItemsSource = Lists.ArmourList;
            ArmourBox.SelectedValuePath = "Armour.ID";

            WeaponBox.ItemsSource = Lists.WeaponList;
            WeaponBox.SelectedValuePath = "ID";

        }
        private void CreateComboBoxHearts()
        {
            try
            {
                for (ushort i = 1; i < 6; i++)
                {
                    var HeartsVar = new Hearts();
                    HeartsVar.HeartsCommand(i, "size");
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
                Loading = true;
                if (DQB2DataEditor.LoadFile(openFileDialog.FileName))
                {
                    //I tried to do the weird binding thing but it doesn't work?? So wohoo copypaste time
                    TextBoxName.Text = NameNPC;
                    ComboBoxGender.SelectedIndex = (int)Sex - 1;
                    if (ComboBoxGender.SelectedItem == null) { ComboBoxGender.SelectedIndex = 0; }
                    TextBoxHP.Text = Convert.ToString(HP);
                    TextBoxVoice.Text = Convert.ToString(Voice);
                    TextBoxDialogue.Text = Convert.ToString(Dialogue);
                    ComboBoxJob.SelectedValue = Job;
                    if (ComboBoxJob.SelectedItem == null) { ComboBoxJob.SelectedIndex = 0; }
                    ComboBoxHome.SelectedValue = Home;
                    if (ComboBoxHome.SelectedItem == null) { ComboBoxHome.SelectedIndex = 0; }
                    ComboBoxIsland.SelectedValue = Island;
                    if (ComboBoxIsland.SelectedItem == null) { ComboBoxIsland.SelectedIndex = 0; }
                    ComboBoxPlace.SelectedIndex = Place;
                    if (ComboBoxPlace.SelectedItem == null) { ComboBoxPlace.SelectedIndex = 0; }
                    ComboBoxTypeLock.SelectedValue = Type;
                    if (ComboBoxTypeLock.SelectedItem == null) { ComboBoxTypeLock.SelectedIndex = 0; }

                    ArmourBox.SelectedValue = Armour;
                    if (ArmourBox.SelectedItem == null) { ArmourBox.SelectedIndex = 0; }
                    WeaponBox.SelectedValue = Weapon;
                    if (WeaponBox.SelectedItem == null) { WeaponBox.SelectedIndex = 0; }

                    var TypeBackup = TypeVisual;

                    SizeBox.SelectedIndex = RoomSize - 1;
                    if (SizeBox.SelectedItem == null) { SizeBox.SelectedIndex = 0; }
                    AmbianceBox.SelectedIndex = RoomAmbience;
                    if (AmbianceBox.SelectedItem == null) { AmbianceBox.SelectedIndex = 0; }
                    FancinessBox.SelectedIndex = RoomFanciness - 1;
                    if (FancinessBox.SelectedItem == null) { FancinessBox.SelectedIndex = 0; }

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


                    ComboBoxBody.SelectedValue = BodyModel;
                    if (ComboBoxBody.SelectedItem == null) { ComboBoxBody.SelectedIndex = 0; }
                    ComboBoxFace.SelectedValue = FaceModel;
                    if (ComboBoxFace.SelectedItem == null) { ComboBoxHair.SelectedIndex = 0; }
                    ComboBoxHair.SelectedValue = HairModel;
                    if (ComboBoxHair.SelectedItem == null) { ComboBoxHair.SelectedIndex = 0; }

                    LockCheck.IsChecked = TypeVisual;
                    ClothCheck.IsChecked = ClothVisual;
                    RaggedCheck.IsChecked = RagVisual;

                    TextBoxConsole.Text = "Loaded File!";
                    TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                    Loading = false;
                    PriorityCodeSetModel(true, true, true);
                }
                else
                {
                    TextBoxConsole.Text = "Not a valid file";
                    TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
                }
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
                NameNPC = TextBoxName.Text.Length > 30 ? TextBoxName.Text.Substring(0, 30) : TextBoxName.Text; 
                TextBoxConsole.Text = "Changed Name to " + NameNPC + "!";
                TextBoxName.Text = NameNPC;
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
                if (ColorBackup.ArmourValues != null) {
                    if (Sex == 1)
                    {
                        DQB2ModelRendering.ClothImage = Lists.getColorDyeVal(ColorBackup.ArmourValues.ColourIDMale);
                    }
                    else
                    {
                        DQB2ModelRendering.ClothImage = Lists.getColorDyeVal(ColorBackup.ArmourValues.ColourIDFemale);
                    }
                }
                PriorityCodeSetModel(false, true, true);
                for (int i = 0; i < Lists.ArmourList.Count; i++)
                {
                    ComboBoxArmour BoxCheck = Lists.ArmourList[i];
                    if(BoxCheck.Armour.ImageID != BoxCheck.Armour.ArmourValues.ImageIDFem)
                    {
                        if (Sex == 1)
                        {
                            BoxCheck.Image = BoxCheck.Armour.Image;
                            BoxCheck.Colour = Lists.getColorDyeVal(BoxCheck.Armour.ArmourValues.ColourIDMale);
                        }
                        else
                        {
                            BoxCheck.Image = BoxCheck.Armour.ArmourValues.ImageFem;
                            BoxCheck.Colour = Lists.getColorDyeVal(BoxCheck.Armour.ArmourValues.ColourIDFemale);
                        }
                        BoxCheck.SetImage();
                    }
                }
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
                TextBoxConsole.Text = "Changed job to " + ((IslandJob)ComboBoxJob.SelectedItem).IJName + "!";
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
                TextBoxConsole.Text = "Changed native home to " + ((IslandJob)ComboBoxHome.SelectedItem).IJName + "!";
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
                TextBoxConsole.Text = "Changed current home to " + ((IslandJob)ComboBoxIsland.SelectedItem).IJName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                IslandImageBox.Source = ChangeImage("images/island/" + Island + ".png");
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

        private void PriorityCodeSetModel(bool Face, bool Hair, bool Body)
        {
            if(Loading == false) {
                var HairVisual = HairModel;
                var FaceVisual = FaceModel;
                var BodyVisual = BodyModel;
                if (TypeVisual == true)
                {
                    var TypeLockCurrent = (ComboBoxTypeLock.SelectedItem as ComboBoxColour).TypeListing;
                    if (TypeLockCurrent.faceID != 0)
                    {
                        FaceVisual = TypeLockCurrent.faceID;
                    }
                    if (TypeLockCurrent.bodyID != 0)
                    {
                        BodyVisual = TypeLockCurrent.bodyID;
                    }
                    if (TypeLockCurrent.hairID != 0)
                    {
                        HairVisual = TypeLockCurrent.hairID;
                    }
                }
                if (RagVisual == true)
                {
                    BodyVisual = 31;
                }
                else {
                    if (Armour != 0 && ClothVisual == true)
                    {
                        var ArmourClass = Lists.ArmourList.FirstOrDefault(x => x.ID == Armour);
                        if (Sex == 1)
                        {
                            BodyVisual = ArmourClass.Armour.ModelIDMale;
                        }
                        else
                        {
                            BodyVisual = ArmourClass.Armour.ArmourValues.ModelIDFemale;
                        }
                    }

                }
                ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceVisual, HairVisual, BodyVisual, Face, Hair, Body);
            }
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
                ColorWindow.CreateButtons();

                if (ColorWindow.ShowDialog() == false)
                {
                    return;
                }
                EyeColor = ColorWindow.ColourPicked;
                ButtonEye.Content = EyeColor;
                var ColorList = Lists.getColorVal(EyeColor);
                RectangleEye.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList.color);
                DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ColorList.color);
                PriorityCodeSetModel(true, false, false);
                TextBoxConsole.Text = "Eye colour changed to " + ColorList + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
        }
        private void ButtonSkin_Click(object sender, RoutedEventArgs e)
        {
            {
                Window1 ColorWindow = new Window1()
                {
                    TextAdd = "skin",
                    ColourPicked = SkinColor,
                    Skin = true
                };
                ColorWindow.CreateButtons();

                if (ColorWindow.ShowDialog() == false)
                {
                    return;
                }
                SkinColor = ColorWindow.ColourPicked;
                ButtonSkin.Content = SkinColor;
                var ColorList = Lists.getColorVal(SkinColor);
                RectangleSkin.Fill = ColorWindow.ColorPickedB;
                DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ColorList.color);
                PriorityCodeSetModel(true, false, true);
                TextBoxConsole.Text = "Skin colour changed to " + ColorList + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
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
                ColorWindow.CreateButtons();

                if (ColorWindow.ShowDialog() == false)
                {
                    return;
                }
                HairColor = ColorWindow.ColourPicked;
                ButtonHair.Content = HairColor;
                var ColorList = Lists.getColorVal(HairColor);
                RectangleHair.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(ColorList.color);
                DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ColorList.color);
                PriorityCodeSetModel(true, true, false);
                TextBoxConsole.Text = "Hair colour changed to " + ColorList + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
        }

        private void ComboBoxBody_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                BodyModel = ((ComboBoxModel)ComboBoxBody.SelectedItem).ID;
                TextBoxConsole.Text = "Changed body model to " + ((ComboBoxModel)ComboBoxBody.SelectedItem).ModelClassV.ModelName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                PriorityCodeSetModel(false, false, true);
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
                FaceModel = ((ComboBoxModel)ComboBoxFace.SelectedItem).ID;
                TextBoxConsole.Text = "Changed face model to " + ((ComboBoxModel)ComboBoxFace.SelectedItem).ModelClassV.ModelName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                PriorityCodeSetModel(true, false, false);
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
                HairModel = ((ComboBoxModel)ComboBoxHair.SelectedItem).ID;
                TextBoxConsole.Text = "Changed hair model to " + ((ComboBoxModel)ComboBoxHair.SelectedItem).ModelClassV.ModelName + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                PriorityCodeSetModel(false, true, false);
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
                TextBoxConsole.Text = "Changed room size to " + (SizeBox.SelectedIndex + 1).ToString() + "!";
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
                FancyImage.Source = new BitmapImage(new Uri("/images/resource/fancy" + RoomFanciness.ToString() + ".png", UriKind.Relative));

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
                Type = (ComboBoxTypeLock.SelectedItem as ComboBoxColour).ID;
                TextBoxConsole.Text = "Changed type to '" + (ComboBoxTypeLock.SelectedItem as ComboBoxColour).TypeListing.name + " type'!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                PriorityCodeSetModel(true, true, true);
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
                if ((ComboBoxTypeLock.SelectedItem as ComboBoxColour).TypeListing.Tier == 3)
                {
                    TypeVisual = true;
                }
                PriorityCodeSetModel(true, true, true);
                TextBoxConsole.Text = "Lock checked!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch {
                TextBoxConsole.Text = "Error (Lock Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }

        }
        private void LockCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                TypeVisual = false;
                PriorityCodeSetModel(true, true, true);
                TextBoxConsole.Text = "Lock unchecked!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch {
                TextBoxConsole.Text = "Error (Lock Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void ClothCheck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClothVisual = true;
                PriorityCodeSetModel(false, true, true);
                TextBoxConsole.Text = "Cloths added!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch {
                TextBoxConsole.Text = "Error (Cloth Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }

        }
        private void ClothCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                ClothVisual = false;
                PriorityCodeSetModel(false, true, true);
                TextBoxConsole.Text = "Clothes removed!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch {
                TextBoxConsole.Text = "Error (Cloth Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
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
                var Test = (ArmourBox.SelectedItem);
                if (Test != null)
                {

                    ColorBackup = (Test as ComboBoxArmour).Armour;
                }
                Armour = ColorBackup.ID;
                TextBoxConsole.Text = "Changed armour to " + ColorBackup.Name + "!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
                    if (Sex == 1)
                    {
                        DQB2ModelRendering.ClothImage = Lists.getColorDyeVal(ColorBackup.ArmourValues.ColourIDMale);
                    }
                    else
                    {
                        DQB2ModelRendering.ClothImage = Lists.getColorDyeVal(ColorBackup.ArmourValues.ColourIDFemale);
                }
                PriorityCodeSetModel(false, true, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TextBoxConsole.Text = "Error (Armour Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void InfoPanel_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Check which button was clicked and handle accordingly
            if (clickedButton == BName)
            {
                InfoName.Text = Lists.InfoText[0];
                InfoBox.Text = Lists.InfoText[1];
            }
            else if (clickedButton == BSex)
            {
                InfoName.Text = Lists.InfoText[2];
                InfoBox.Text = Lists.InfoText[3];
            }
            else if (clickedButton == BHP)
            {
                InfoName.Text = Lists.InfoText[4];
                InfoBox.Text = Lists.InfoText[5];
            }
            else if (clickedButton == BJob)
            {
                InfoName.Text = Lists.InfoText[6];
                InfoBox.Text = Lists.InfoText[7];
            }
            else if (clickedButton == BType)
            {
                InfoName.Text = Lists.InfoText[8];
                InfoBox.Text = Lists.InfoText[9];
            }
            else if (clickedButton == BHome)
            {
                InfoName.Text = Lists.InfoText[10];
                InfoBox.Text = Lists.InfoText[11];
            }
            else if (clickedButton == BIsland)
            {
                InfoName.Text = Lists.InfoText[12];
                InfoBox.Text = Lists.InfoText[13];
            }
            else if (clickedButton == BPlace)
            {
                InfoName.Text = Lists.InfoText[14];
                InfoBox.Text = Lists.InfoText[15];
            }
            else if (clickedButton == BVoice)
            {
                InfoName.Text = Lists.InfoText[16];
                InfoBox.Text = Lists.InfoText[17];
            }
            else if (clickedButton == BFlags)
            {
                InfoName.Text = Lists.InfoText[18];
                InfoBox.Text = Lists.InfoText[19];
            }
            else if (clickedButton == BFaceModel)
            {
                InfoName.Text = Lists.InfoText[20];
                InfoBox.Text = Lists.InfoText[21];
            }
            else if (clickedButton == BHairModel)
            {
                InfoName.Text = Lists.InfoText[22];
                InfoBox.Text = Lists.InfoText[23];
            }
            else if (clickedButton == BBodyModel)
            {
                InfoName.Text = Lists.InfoText[24];
                InfoBox.Text = Lists.InfoText[25];
            }
            else if (clickedButton == BEyeColour)
            {
                InfoName.Text = Lists.InfoText[26];
                InfoBox.Text = Lists.InfoText[27];
            }
            else if (clickedButton == BHairColour)
            {
                InfoName.Text = Lists.InfoText[28];
                InfoBox.Text = Lists.InfoText[29];
            }
            else if (clickedButton == BSkinColour)
            {
                InfoName.Text = Lists.InfoText[30];
                InfoBox.Text = Lists.InfoText[31];
            }
            else if (clickedButton == BWeapon)
            {
                InfoName.Text = Lists.InfoText[32];
                InfoBox.Text = Lists.InfoText[33];
            }
            else if (clickedButton == BArmour)
            {
                InfoName.Text = Lists.InfoText[34];
                InfoBox.Text = Lists.InfoText[35];
            }
            else if (clickedButton == BDialogue)
            {
                InfoName.Text = Lists.InfoText[36];
                InfoBox.Text = Lists.InfoText[37];
            }
        }

        private void RaggedCheck_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RagVisual = true;
                PriorityCodeSetModel(false, false, true);
                TextBoxConsole.Text = "Raggs added!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch
            {
                TextBoxConsole.Text = "Error (Ragg Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
        private void RaggedCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                RagVisual = false;
                PriorityCodeSetModel(false, false, true);
                TextBoxConsole.Text = "Raggs removed!";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch
            {
                TextBoxConsole.Text = "Error (Ragg Change)";
                TextBoxConsole.Foreground = new SolidColorBrush(Colors.Red);
            }
        }
    }
}