using DQB2NPCViewer.code;
using DQB2NPCViewer.control;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Ionic.Zlib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Numerics;
using System.Xml.Linq;
using HelixToolkit.Wpf;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Reflection;
using System.Windows.Media.Media3D;

namespace DQB2NPCViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableProperty<CharacterButton> SelectedNPC { get; set; } = new ObservableProperty<CharacterButton>();
        public ObservableProperty<NPCData> EditingNPC { get; set; } = new ObservableProperty<NPCData>();

        public ObservableProperty<BuilderData> EditingBuilder { get; set; } = new ObservableProperty<BuilderData>();


        public ObservableProperty<SolidColorBrush> ConsoleColour { get; set; } = new ObservableProperty<SolidColorBrush>() { Value = Brushes.Green };
        public ObservableProperty<String> ConsoleText { get; set; } = new ObservableProperty<String>();
        public ObservableProperty<String> NameDescText { get; set; } = new ObservableProperty<String>();
        public ObservableProperty<String> DescText { get; set; } = new ObservableProperty<String>();

        public ObservableProperty<Brush> EyeColour { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
        public ObservableProperty<Brush> SkinColour { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
        public ObservableProperty<Brush> SkinColourFilter { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
        public ObservableProperty<Brush> HairColour { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };

        public ObservableProperty<Brush> EyeColourBuilder { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
        public ObservableProperty<Brush> SkinColourBuilder { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
        public ObservableProperty<Brush> SkinColourFilterBuilder { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
        public ObservableProperty<Brush> HairColourBuilder { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
        public ObservableProperty<Brush> ClothColour { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
        private bool Loaded => EditingNPC.Value != null;
        private bool LoadedBuilder => EditingBuilder.Value != null;

        private bool _isUserInitiated;

        private bool builder = false;

        private void VisualChangeCheck(object sender, MouseButtonEventArgs e)
        {
            _isUserInitiated = true;
        }
        private void FullModelUpdate(object sender, SelectionChangedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    PriorityCodeSetModelBuilder(true, true, true);
                else
                    PriorityCodeSetModel(true, true, true);
            }
        }
        private void FullModelUpdate(object sender, RoutedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    PriorityCodeSetModelBuilder(true, true, true);
                else
                    PriorityCodeSetModel(true, true, true);
            }
        }
        private void BodyModelUpdate(object sender, RoutedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    PriorityCodeSetModelBuilder(false, false, true);
                else
                    PriorityCodeSetModel(false, false, true);
            }
        }
        private void FaceModelUpdate(object sender, SelectionChangedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    PriorityCodeSetModelBuilder(true, false, false);
                else
                    PriorityCodeSetModel(true, false, false);
            }
        }

        private void HairModelUpdate(object sender, SelectionChangedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    PriorityCodeSetModelBuilder(false, true, false);
                else
                    PriorityCodeSetModel(false, true, false);
            }
        }

        private void BodyModelUpdate(object sender, SelectionChangedEventArgs e) //Armour change
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;

                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                {
                    try
                    {
                        if (EditingBuilder.Value.mirrorClothes == 0)
                            if (EditingBuilder.Value.armour == 0)
                                DQB2ModelRendering.ClothImage = ListText.ArmourBuilderList.FirstOrDefault(x => x.Armour.ModelIDMale == 1).Colour;
                            else
                                DQB2ModelRendering.ClothImage = ((ComboBoxArmour)(ComboBuilderArmour.SelectedItem)).Colour;
                        else
                            DQB2ModelRendering.ClothImage = ((ComboBoxArmour)(ComboBuilderMirrorArmour.SelectedItem)).Colour;
                    }
                    catch (Exception ex)
                    {
                        ConsoleText.Value = "Cannot find color on 'Clothes Colour'.";
                        ConsoleColour.Value = Brushes.Orange;
                    }
                    PriorityCodeSetModelBuilder(false, true, true);
                }

                else
                {
                    try
                    {
                        DQB2ModelRendering.ClothImage = ((ComboBoxArmour)(ComboArmour.SelectedItem)).Colour;
                        ClothColour.Value = new SolidColorBrush(DQB2ModelRendering.ClothImage);
                    }
                    catch (Exception ex)
                    {
                        ConsoleText.Value = "Cannot find color on 'Clothes Colour'.";
                        ConsoleColour.Value = Brushes.Orange;
                    }
                    PriorityCodeSetModel(false, true, true);
                }

            }
        }
        public MainWindow()
        {
            DataContext = this;
            ListText.setList("body", "color", "face", "hair", "islands", "jobs", "ambiance", "typelock", "weapon", "armour", "place", "builderHair", "accesories", "tools", "shield");

            InitializeComponent();
            CharacterTabList();
            this.SizeChanged += OnWindowSizeChanged;
            SelectionList.ReturnSelectedTile += SelectedNPC_OnClick;
            DQB2ModelRendering.ModelCodeC();
            DQB2ModelRendering.Rotate();
            DQB2ModelRendering.RotateAccesory();
            ConsoleText.Value = "Hello World!";
            DescText.Value = "Open a CMNDAT.BIN file to continue.";
        }

        private void CharacterTabList()
        {
            var Mon = new List<TypeSet>();
            var An = new List<TypeSet>();
            var Hum = new List<TypeSet>();
            foreach (var a in ListText.TypeLockList)
            {
                if (a.TypeListing.Monster == true)
                {
                    Mon.Add(a.TypeListing);
                }
                else
                {
                    Hum.Add(a.TypeListing);
                }
            }
            this.TabListToGo.Children.Add(new MenuListType(Hum, "Human", An, "Animal", Mon, "Monster"));
        }
        private async void CMNDAT_Open_Click(object sender, RoutedEventArgs e)
        {

            var openFileDialog = new OpenFileDialog
            {
                Filter = "*CMNDAT.BIN|*CMNDAT.BIN"
            };

            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }

            if (System.IO.File.Exists(openFileDialog.FileName) == false) return;
            LoadingImage.Visibility = Visibility.Visible;
            MainGrid.IsEnabled = false;
            await Task.Run(() => FullLoad(openFileDialog.FileName));
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SelectionList.createTabList();
                SelectionList.ScrollViewUpdate(this.ActualHeight - 110);
                SelectionList.StoryMenu.TextBoxFilter.Width = 350;
                SelectionList.GenericMenu.TextBoxFilter.Width = 350;
            });

            if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
            {
                SwapToBuilder();
                PriorityCodeSetModelBuilder(true, true, true);
            }
            swapGender(EditingBuilder.Value.sex, ListText.ArmourBuilderList);
            swapGender(EditingBuilder.Value.sex, ListText.ArmourBuilderListMirror);
            EditingBuilder.NotifyValue();

            MainGrid.IsEnabled = true;
            LoadingImage.Visibility = Visibility.Collapsed;

        }
        private void FullLoad(string FileName)
        {
            byte[] CMNDAT = System.IO.File.ReadAllBytes(FileName);

            String prefix = Encoding.UTF8.GetString(CMNDAT, 0, 4);
            if (prefix != "aerC") return;

            DQB2DataEditor.LoadFile(CMNDAT);
            SelectionList.StoryChar = DQB2DataEditor.LoadCMNDATStory();
            SelectionList.GenericChar = DQB2DataEditor.LoadCMNDATGeneric();
            EditingBuilder.Value = DQB2DataEditor.LoadCMNDATBuilder();

        }
        private async void CMNDAT_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DQB2DataEditor.CMNDATfileBytes == null)
                {
                    return;
                }

                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "*.BIN|*.BIN",
                    FileName = "CMNDAT"
                };
                if (saveFileDialog.ShowDialog() == false)
                {
                    return;
                }
                Saving.Visibility = Visibility.Visible;
                MainGrid.IsEnabled = false;
                await Task.Run(() => DQB2DataEditor.SaveFile(saveFileDialog.FileName, EditingBuilder.Value));
                MainGrid.IsEnabled = true;
                Saving.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show(ex.Message, "Failed to save file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
        protected void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                SelectionList.ScrollViewUpdate(e.NewSize.Height - 110);
                foreach (var a in TabListToGo.Children)
                {
                    if (a.GetType() == typeof(MenuListType))
                    {
                        ((MenuListType)a).ScrollView.Height = EditingGrid.ActualHeight - 85;
                        ((MenuListType)a).TextBoxFilter.Width = TabListToGo.ActualWidth;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void SelectedNPC_OnClick(NPCDataMinimum NewSelectedNPC)
        {
            SelectedNPC.Value = new CharacterButton(NewSelectedNPC);

        }
        private void SwapToNPC()
        {
            if (Loaded)
            {
                builder = false;
                DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingNPC.Value.eyeColour).color);
                EyeColour.Value = new SolidColorBrush(DQB2ModelRendering.EyeImage);

                DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingNPC.Value.hairColour).color);
                HairColour.Value = new SolidColorBrush(DQB2ModelRendering.HairImage);

                DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingNPC.Value.skinColour).color);
                SkinColour.Value = new SolidColorBrush(DQB2ModelRendering.SkinImage);
                SkinColourFilter.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DQB2ModelRendering.Multiply(ListText.getColorVal(EditingNPC.Value.skinColour).color)));

                try
                {
                    DQB2ModelRendering.ClothImage = ((ComboBoxArmour)(ComboArmour.SelectedItem)).Colour;
                }
                catch (Exception ex)
                {
                    var ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingNPC.Value.armour);
                    ushort IDColour = ArmourClass.Armour.ArmourValues.ColourIDFemale;
                    if (EditingNPC.Value.sex == 1)
                        IDColour = ArmourClass.Armour.ArmourValues.ColourIDMale;
                    DQB2ModelRendering.ClothImage = ListText.getColorDyeVal(IDColour);
                }
                ClothColour.Value = new SolidColorBrush(DQB2ModelRendering.ClothImage);
            }
        }

        private void SwapToBuilder() //Made spaguetti code back then, gotta work with this now...
        {
            if (LoadedBuilder)
            {
                builder = true;
                DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingBuilder.Value.eyeColour).color);
                EyeColourBuilder.Value = new SolidColorBrush(DQB2ModelRendering.EyeImage);

                DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingBuilder.Value.hairColour).color);
                HairColourBuilder.Value = new SolidColorBrush(DQB2ModelRendering.HairImage);

                DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingBuilder.Value.skinColour).color);
                SkinColourBuilder.Value = new SolidColorBrush(DQB2ModelRendering.SkinImage);
                SkinColourFilterBuilder.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DQB2ModelRendering.Multiply(ListText.getColorVal(EditingBuilder.Value.skinColour).color)));
                try
                {
                    if (EditingBuilder.Value.mirrorClothes == 0)
                        if (EditingBuilder.Value.armour == 0)
                            DQB2ModelRendering.ClothImage = ListText.ArmourBuilderList.FirstOrDefault(x => x.Armour.ModelIDMale == 1).Colour;
                        else
                            DQB2ModelRendering.ClothImage = ((ComboBoxArmour)(ComboBuilderArmour.SelectedItem)).Colour;
                    else
                        DQB2ModelRendering.ClothImage = ((ComboBoxArmour)(ComboBuilderMirrorArmour.SelectedItem)).Colour;
                }
                catch (Exception ex)
                {
                    ComboBoxArmour ArmourClass;
                    if (EditingBuilder.Value.mirrorClothes == 0)
                        if (EditingBuilder.Value.armour == 0)
                            ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.Armour.ModelIDMale == 1);
                        else
                            ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingBuilder.Value.armour);
                    else
                        ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingBuilder.Value.mirrorClothes);
                    ushort IDColour = ArmourClass.Armour.ArmourValues.ColourIDFemale;
                    if (EditingBuilder.Value.sex == 1)
                        IDColour = ArmourClass.Armour.ArmourValues.ColourIDMale;
                    DQB2ModelRendering.ClothImage = ListText.getColorDyeVal(IDColour);
                }
            }

        }
        private void LoadSelectedNPC_Click(object sender, RoutedEventArgs e)
        {
            EditingNPC.Value = DQB2DataEditor.LoadCMNDATOffset(SelectedNPC.Value.NPC.Value.offset);

            SwapToNPC();
            swapGender(EditingNPC.Value.sex, ListText.ArmourList);

            PriorityCodeSetModel(true, true, true);
            MyHelixViewport.ZoomExtents();
        }
        private void SaveSelectedNPC_Click(object sender, RoutedEventArgs e)
        {
            EditingNPC.Value.offset = SelectedNPC.Value.NPC.Value.offset;
            DQB2DataEditor.SaveCMNDATOffset(EditingNPC.Value);
            SelectedNPC.Value = new CharacterButton(SelectionList.UpdateCharButton(EditingNPC.Value.offset, EditingNPC.Value));
        }
        private void PriorityCodeSetModel(bool Face, bool Hair, bool Body)
        {
            if (Loaded == true)
            {
                var HairVisual = EditingNPC.Value.hairModel;
                var FaceVisual = EditingNPC.Value.faceModel;
                var BodyVisual = EditingNPC.Value.bodyModel;
                if (EditingNPC.Value.typeLock == true)
                {
                    TypeSet TypeLockCurrent = null;
                    try
                    {
                        TypeLockCurrent = (ComboBoxCharType.SelectedItem as ComboBoxColour).TypeListing;
                    }
                    catch (Exception ex)
                    {
                        var a = ListText.TypeLockList.FirstOrDefault(x => x.ID == EditingNPC.Value.charType);
                        if (a != null)
                            TypeLockCurrent =a.TypeListing;
                    }
                    if (TypeLockCurrent != null)
                    {
                        if (TypeLockCurrent.faceID != 0)
                            FaceVisual = TypeLockCurrent.faceID;
                        if (TypeLockCurrent.bodyID != 0)
                            BodyVisual = TypeLockCurrent.bodyID;
                        if (TypeLockCurrent.hairID != 0)
                            HairVisual = TypeLockCurrent.hairID;
                    }
                }
                if (EditingNPC.Value.hasRags == true)
                {
                    if (EditingNPC.Value.sex == 1)
                        BodyVisual = 31;
                    else
                        BodyVisual = 32;
                }
                else
                {
                    if (EditingNPC.Value.armour != 0 && EditingNPC.Value.hasClothes == true)
                    {
                        var ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingNPC.Value.armour);
                        if (EditingNPC.Value.sex == 1)
                            BodyVisual = ArmourClass.Armour.ModelIDMale;
                        else
                            BodyVisual = ArmourClass.Armour.ArmourValues.ModelIDFemale;
                    }

                }
                ModelGroupVisualName.Content = DQB2ModelRendering.GroupModels(FaceVisual, HairVisual, BodyVisual, Face, Hair, Body);
            }

        }
        private void PriorityCodeSetModelBuilder(bool Face, bool Hair, bool Body)
        {
            if (LoadedBuilder == true)
            {

                DQB2ModelRendering.RotateAccesory();
                var HairVisual = EditingBuilder.Value.hairModelBase;
                var FaceVisual = EditingBuilder.Value.faceModelBase;
                var BodyVisual = EditingBuilder.Value.bodyModelBase;

                var Accesory1 = EditingBuilder.Value.mirrorAccesory1;
                var Accesory2 = EditingBuilder.Value.mirrorAccesory2;
                var Accesory3 = EditingBuilder.Value.mirrorAccesory3;

                ushort AccesoryExtra = 0;
                if (EditingBuilder.Value.mirrorClothes != 0 || EditingBuilder.Value.armour != 0)
                {
                    ComboBoxArmour ArmourClass;
                    if (EditingBuilder.Value.mirrorClothes == 0)
                        ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingBuilder.Value.armour);
                    else
                        ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingBuilder.Value.mirrorClothes);
                    if (EditingBuilder.Value.sex == 1)
                        BodyVisual = ArmourClass.Armour.ModelIDMale;
                    else
                        BodyVisual = ArmourClass.Armour.ArmourValues.ModelIDFemale;
                }
                if (Hair)
                {
                    if (Accesory1 != 0)
                    {
                        var AccClass = ListText.AccesoryList.FirstOrDefault(x => x.ItemID == EditingBuilder.Value.mirrorAccesory1);
                        Accesory1 = AccClass.ModelAccesoryID;
                    }
                    if (Accesory2 != 0)
                    {
                        var AccClass = ListText.AccesoryList.FirstOrDefault(x => x.ItemID == EditingBuilder.Value.mirrorAccesory2);
                        Accesory2 = AccClass.ModelAccesoryID;
                    }
                    if (Accesory3 != 0)
                    {
                        var AccClass = ListText.AccesoryList.FirstOrDefault(x => x.ItemID == EditingBuilder.Value.mirrorAccesory3);
                        Accesory3 = AccClass.ModelAccesoryID;
                    }
                    if (EditingBuilder.Value.mirrorHair != 0)
                    {
                        var AccClass = ListText.HairBuilderList.FirstOrDefault(x => x.ItemID == EditingBuilder.Value.mirrorHair);
                        HairVisual = AccClass.ModelHairID;
                        if (HairVisual == 0)
                        {
                            AccesoryExtra = AccClass.ModelAccesoryID;
                            HairVisual = EditingBuilder.Value.isMale ? (ushort)53 : (ushort)52;
                        }
                    }
                }
                ModelGroupVisualBuilder.Content = DQB2ModelRendering.GroupModelsBuilder(FaceVisual, HairVisual, BodyVisual,
                    Accesory1, Accesory2, Accesory3, AccesoryExtra,
                    Face, Hair, Body,
                    Hair, Hair, Hair, Hair);
            }

        }
        private void InfoPanel_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            // Check which button was clicked and handle accordingly
            if (clickedButton.Tag != null)
            {
                var Index = ushort.Parse(clickedButton.Tag.ToString());
                Index = (ushort)(Index * 2);
                NameDescText.Value = ListText.InfoText[Index];
                DescText.Value = ListText.InfoText[Index + 1];
            }
            else
            {
                NameDescText.Value = clickedButton.Content.ToString();
                DescText.Value = "No info.";
            }

        }

        private void EditingTabChange(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            var selectedTab = tabControl.SelectedItem as TabItem;
            if (selectedTab != null && builder && (selectedTab.Header.ToString() == "Visual" || selectedTab.Name.ToString() == "TabEditing"))
            {
                SwapToNPC();
                PriorityCodeSetModel(true, true, true);
                MyHelixViewport.ZoomExtents();
            }
            else if (selectedTab != null && !builder && selectedTab.Name.ToString() == "TabBuilder")
            {
                SwapToBuilder();
                PriorityCodeSetModelBuilder(true, true, true);
                MyHelixViewport.ZoomExtents();
            }
        }
        private void swapGender(byte gender, ObservableCollection<ComboBoxArmour> List)
        {
            for (int i = 0; i < List.Count; i++)
            {
                ComboBoxArmour BoxCheck = List[i];
                if (BoxCheck.Armour.ImageID != BoxCheck.Armour.ArmourValues.ImageIDFem)
                {
                    if (gender == 1)
                    {
                        BoxCheck.Image = BoxCheck.Armour.Image;
                        BoxCheck.Colour = ListText.getColorDyeVal(BoxCheck.Armour.ArmourValues.ColourIDMale);
                    }
                    else
                    {
                        BoxCheck.Image = BoxCheck.Armour.ArmourValues.ImageFem;
                        BoxCheck.Colour = ListText.getColorDyeVal(BoxCheck.Armour.ArmourValues.ColourIDFemale);
                    }
                    BoxCheck.SetImage();
                }
            }
        }
        private void test(object sender, RoutedEventArgs e)
        {
            if (Loaded)
            {
                EditingNPC.NotifyValue();
                swapGender(EditingNPC.Value.sex, ListText.ArmourList);
                PriorityCodeSetModel(false, false, true);
            }

        }
        private void testBuilder(object sender, RoutedEventArgs e)
        {
            if (LoadedBuilder)
            {
                EditingBuilder.NotifyValue();
                swapGender(EditingBuilder.Value.sex, ListText.ArmourBuilderList);
                swapGender(EditingBuilder.Value.sex, ListText.ArmourBuilderListMirror);
                if (EditingBuilder.Value.mirrorClothes == 0 && EditingBuilder.Value.armour == 0)
                {
                    ComboBoxArmour ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.Armour.ModelIDMale == 1);
                    ushort IDColour = ArmourClass.Armour.ArmourValues.ColourIDFemale;
                    if (EditingBuilder.Value.sex == 1)
                        IDColour = ArmourClass.Armour.ArmourValues.ColourIDMale;
                    DQB2ModelRendering.ClothImage = ListText.getColorDyeVal(IDColour);
                }
                PriorityCodeSetModelBuilder(true, true, true);
            }
        }

        private void ChangeColour(object sender, RoutedEventArgs e)
        {
            ushort Colour = 0;
            string Text = null;
            bool skin = false;
            switch (((Button)sender).Tag)
            {
                case "0": //eye
                    Colour = EditingNPC.Value.eyeColour;
                    Text = "eye";
                    break;
                case "1": //hair
                    Colour = EditingNPC.Value.hairColour;
                    Text = "hair";
                    break;
                case "2": //skin
                    Colour = EditingNPC.Value.skinColour;
                    Text = "skin";
                    skin = true;
                    break;
            }
            Window1 ColorWindow = new Window1()
            {
                TextAdd = Text,
                ColourPicked = Colour,
                Skin = skin
            };
            ColorWindow.CreateButtons();

            if (ColorWindow.ShowDialog() == false)
            {
                return;
            }
            switch (((Button)sender).Tag)
            {
                case "0": //eye
                    EditingNPC.Value.eyeColour = ColorWindow.ColourPicked;
                    DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingNPC.Value.eyeColour).color); ;
                    EyeColour.Value = new SolidColorBrush(DQB2ModelRendering.EyeImage);
                    PriorityCodeSetModel(true, false, false);
                    break;
                case "1": //hair
                    EditingNPC.Value.hairColour = ColorWindow.ColourPicked;
                    DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingNPC.Value.hairColour).color);
                    HairColour.Value = new SolidColorBrush(DQB2ModelRendering.HairImage);
                    PriorityCodeSetModel(true, true, false);
                    break;
                case "2": //skin
                    EditingNPC.Value.skinColour = ColorWindow.ColourPicked;
                    DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingNPC.Value.skinColour).color);
                    SkinColour.Value = new SolidColorBrush(DQB2ModelRendering.SkinImage);
                    SkinColourFilter.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DQB2ModelRendering.Multiply(ListText.getColorVal(EditingNPC.Value.skinColour).color)));
                    PriorityCodeSetModel(true, false, true);
                    break;
            }
            EditingNPC.NotifyValue();
            //TextBoxConsole.Text = "Eye colour changed to " + ColorList + "!";
            //TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void ChangeColourBuilder(object sender, RoutedEventArgs e)
        {
            ushort Colour = 0;
            string Text = null;
            bool skin = false;
            switch (((Button)sender).Tag)
            {
                case "0": //eye
                    Colour = EditingBuilder.Value.eyeColour;
                    Text = "eye";
                    break;
                case "1": //hair
                    Colour = EditingBuilder.Value.hairColour;
                    Text = "hair";
                    break;
                case "2": //skin
                    Colour = EditingBuilder.Value.skinColour;
                    Text = "skin";
                    skin = true;
                    break;
            }
            Window1 ColorWindow = new Window1()
            {
                TextAdd = Text,
                ColourPicked = Colour,
                Skin = skin
            };
            ColorWindow.CreateButtons();

            if (ColorWindow.ShowDialog() == false)
            {
                return;
            }
            switch (((Button)sender).Tag)
            {
                case "0": //eye
                    EditingBuilder.Value.eyeColour = ColorWindow.ColourPicked;
                    DQB2ModelRendering.EyeImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingBuilder.Value.eyeColour).color); ;
                    EyeColourBuilder.Value = new SolidColorBrush(DQB2ModelRendering.EyeImage);
                    PriorityCodeSetModelBuilder(true, false, false);
                    break;
                case "1": //hair
                    EditingBuilder.Value.hairColour = ColorWindow.ColourPicked;
                    DQB2ModelRendering.HairImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingBuilder.Value.hairColour).color);
                    HairColourBuilder.Value = new SolidColorBrush(DQB2ModelRendering.HairImage);
                    PriorityCodeSetModelBuilder(true, true, false);
                    break;
                case "2": //skin
                    EditingBuilder.Value.skinColour = ColorWindow.ColourPicked;
                    DQB2ModelRendering.SkinImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(EditingBuilder.Value.skinColour).color);
                    SkinColourBuilder.Value = new SolidColorBrush(DQB2ModelRendering.SkinImage);
                    SkinColourFilterBuilder.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DQB2ModelRendering.Multiply(ListText.getColorVal(EditingBuilder.Value.skinColour).color)));
                    PriorityCodeSetModelBuilder(true, false, true);
                    break;
            }
            EditingBuilder.NotifyValue();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}