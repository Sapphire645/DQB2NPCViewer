using DQB2NPCViewer.code;
using DQB2NPCViewer.control;
using DQB2NPCViewer.SaveData;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

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

        public ObservableProperty<Brush> EyeColour => NPCModel.EyeColour;
        public ObservableProperty<Brush> SkinColour => NPCModel.SkinColour;
        public ObservableProperty<Brush> SkinColourFilter => NPCModel.SkinColourFilter;
        public ObservableProperty<Brush> HairColour => NPCModel.HairColour;

        public ObservableProperty<Brush> EyeColourBuilder => BuilderModel.EyeColour;
        public ObservableProperty<Brush> SkinColourBuilder => BuilderModel.SkinColour;
        public ObservableProperty<Brush> SkinColourFilterBuilder => BuilderModel.SkinColourFilter;
        public ObservableProperty<Brush> HairColourBuilder => BuilderModel.HairColour;
        public ObservableProperty<Brush> ClothColour => NPCModel.ClothColour;

        public ObservableProperty<String> FaceDisplay => NPCModel.faceModelDName;
        public ObservableProperty<String> HairDisplay => NPCModel.hairModelDName;
        public ObservableProperty<String> BodyDisplay => NPCModel.bodyModelDName;

        public ObservableCollection<Place> StyleList { get; set; } = new ObservableCollection<Place>();
        private void UpdateStyleList()
        {
            StyleList.Clear();
            if (EditingNPC.Value == null || ListText.getTypeCharVal(EditingNPC.Value.charType).Models.Count == 0)
            {
                StyleList.Add(ListText.DefaultState[0]);
            }
            else
            {
                foreach (var a in ListText.getTypeCharVal(EditingNPC.Value.charType).Models)
                {
                    StyleList.Add(new Place() { Id = a.Key, Name = a.Value.Name });
                }
            }
            if (EditingNPC.Value == null) return;
            Style.SelectedIndex = EditingNPC.Value.style;
        }
        private new bool Loaded => EditingNPC.Value != null;
        private bool LoadedBuilder => EditingBuilder.Value != null;

        private bool _isUserInitiated;

        private HelixViewportModel NPCModel;
        private HelixViewportModel BuilderModel;
        public ObservableProperty<Visibility> CircleVisible { get; set; } = new ObservableProperty<Visibility>() { Value = Visibility.Visible };

        public bool CircleBool
        {
            get { return CircleVisible.Value == Visibility.Collapsed; }
            set { if (CircleVisible.Value == Visibility.Collapsed) CircleVisible.Value = Visibility.Visible; else CircleVisible.Value = Visibility.Collapsed; }
        }

        public MainWindow()
        {
            NPCModel = new ModelDisplayNPC();
            BuilderModel = new ModelDisplayBuilder();
            DataContext = this;
            ListText.setList("body", "color", "face", "hair", "islands", "jobs", "ambiance", "npc_info", "weapon", "armour", "place", "builderHair", "accesories", "tools", "shield", "coordinatemap");
            

            InitializeComponent();
            CharacterTabList();
            this.SizeChanged += OnWindowSizeChanged;
            SelectionList.ReturnSelectedTile += SelectedNPC_OnClick;
            UpdateStyleList();
            Style.SelectedIndex = 0;

            ConsoleText.Value = "Hello World!";
            DescText.Value = "Open a CMNDAT.BIN file to continue.";
        }

        private void ConsoleCommand(string text, bool error, bool warning)
        {
            ConsoleText.Value = text;
            if (error)
                ConsoleColour.Value = Brushes.Red;
            else if (warning)
                ConsoleColour.Value = Brushes.Orange;
            else
                ConsoleColour.Value = Brushes.Green;

        }
        private void VisualChangeCheck(object sender, MouseButtonEventArgs e)
        {
            _isUserInitiated = true;
        }
        private void FullModelUpdate(object sender, SelectionChangedEventArgs e)
        {
            if (_isUserInitiated)
            {
                if (sender == ComboBoxCharType)
                {
                    var type = ((sender as ComboBox).SelectedItem as ComboBoxColour).TypeListing;
                    NameDescText.Value = type.Name;
                    DescText.Value = type.Description;
                }
                else if (sender == ComboJob) UpdateJobConsole(sender, e);
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    UpdateModelToNewValuesBuilder();
                else
                    UpdateModelToNewValues();
            }
        }
        private void FullModelUpdate(object sender, RoutedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    UpdateModelToNewValuesBuilder();
                else
                    UpdateModelToNewValues();
            }
        }
        private void BodyModelUpdate(object sender, RoutedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    UpdateModelToNewValuesBuilder();
                else
                    UpdateModelToNewValues();
            }
        }
        private void FaceModelUpdate(object sender, SelectionChangedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    UpdateModelToNewValuesBuilder();
                else
                    UpdateModelToNewValues();
            }
        }

        private void HairModelUpdate(object sender, SelectionChangedEventArgs e)
        {
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    UpdateModelToNewValuesBuilder();
                else
                    UpdateModelToNewValues();
            }
        }

        private void BodyModelUpdate(object sender, SelectionChangedEventArgs e) //Armour change
        {
            //Armour change
            if (_isUserInitiated)
            {
                _isUserInitiated = false;
                if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
                    UpdateModelToNewValuesBuilder();
                else
                    UpdateModelToNewValues();
            }
        }
        private void CharacterTabList()
        {
            var Mon = new List<CHARlock>();
            var An = new List<CHARlock>();
            var Hum = new List<CHARlock>();
            foreach (var a in ListText.TypeLockList)
            {
                switch (a.TypeListing.SpeciesCategory)
                {
                    case 0:
                        Hum.Add(a.TypeListing);
                        break;
                    case 1:
                        Mon.Add(a.TypeListing);
                        break;
                    case 2:
                        An.Add(a.TypeListing);
                        break;
                }
            }
            MenuListType menu = new MenuListType(Hum, "Human", An, "Animal", Mon, "Monster");
            menu.ButtonClicked += ChangeChar_OnClick;
            this.TabListToGo.Children.Add(menu);
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
            MainGrid.UpdateLayout();
            LoadingImage.UpdateLayout();
            await Task.Run(() => FullLoad(openFileDialog.FileName));
            await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SelectionList.createTabList();
                SelectionList.ScrollViewUpdate(this.ActualHeight - 110);
                SelectionList.StoryMenu.TextBoxFilter.Width = 350;
                SelectionList.GenericMenu.TextBoxFilter.Width = 350;
            });
            ZoomSliderBuilder.Value = 1;
            if (((TabItem)Tabs.SelectedItem).Name.ToString() == "TabBuilder")
            {
                SwapToBuilder();
            }

            swapGender(EditingBuilder.Value.sex, ListText.ArmourBuilderList);
            swapGender(EditingBuilder.Value.sex, ListText.ArmourBuilderListMirror);
            EditingBuilder.NotifyValue();

            ConsoleCommand("Loaded CMNDAT!", false, false);
            MainGrid.IsEnabled = true;
            LoadingImage.Visibility = Visibility.Collapsed;
        }

        private void FullLoad(string FileName)
        {
            byte[] CMNDAT = System.IO.File.ReadAllBytes(FileName);

            String prefix = Encoding.UTF8.GetString(CMNDAT, 0, 4);
            if (prefix != "aerC") return;

            code.CMNDAT.LoadFile(CMNDAT);
            SelectionList.StoryChar = code.CMNDAT.LoadCMNDATStory();
            SelectionList.GenericChar = code.CMNDAT.LoadCMNDATGeneric();
            EditingBuilder.Value = code.CMNDAT.LoadCMNDATBuilder();
            EditingBuilder.Value.UpdatedCoords += UpdateCoord;
        }
        private async void CMNDAT_Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CMNDAT.CMNDATfileBytes == null)
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
                await Task.Run(() => CMNDAT.SaveFile(saveFileDialog.FileName, EditingBuilder.Value));
                MainGrid.IsEnabled = true;
                Saving.Visibility = Visibility.Collapsed;
                ConsoleCommand("Saved CMNDAT!", false, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ConsoleCommand("Failed to save CMNDAT", true, false);
                MessageBox.Show(ex.Message, "Failed to save file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "*.bin|*.BIN"
            };

            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }

            if (System.IO.File.Exists(openFileDialog.FileName) == false) return;
            FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
            if (CMNDAT.SizeOfChar != (uint)(fileInfo.Length))
            {
                ConsoleCommand("NPC not valid", true, false);
                return;
            }
            byte Bk = 255;
            if (EditingNPC.Value != null)
                Bk = EditingNPC.Value.island;

            byte[] NPC = System.IO.File.ReadAllBytes(openFileDialog.FileName);
            EditingNPC.Value = new NPCData(NPC);

            EditingNPC.Value.UpdatedCoords += UpdateCoord;

            var Point = EditingNPC.Value.coordFocus(ImageMap);
            if (Point.X != double.NaN && Point.X != double.PositiveInfinity)
            {
                Map.RenderTransformOrigin = Point;
            }
            if (Bk != EditingNPC.Value.island)
            {
                ZoomSlider.Value = 1;
            }
            SwapToNPC();
            swapGender(EditingNPC.Value.sex, ListText.ArmourList);
            UpdateStyleList();

            MyHelixViewport.ZoomExtents();

            ConsoleCommand("Imported NPC", false, false);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (EditingNPC.Value == null) return;
            if (CMNDAT.CMNDATfileBytes == null) return;
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "*.BIN|*.bin",
                FileName = EditingNPC.Value.charType.ToString("D4") + EditingNPC.Value.name
            };
            if (saveFileDialog.ShowDialog() == false) return;
            System.IO.File.WriteAllBytes(saveFileDialog.FileName, EditingNPC.Value.byteData);
            ConsoleCommand("Exported NPC", false, false);
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
                if (ImageMap != null && Loaded)
                    Map.RenderTransformOrigin = EditingNPC.Value.coordFocus(ImageMap);
            }
            catch
            {
                ConsoleCommand("NOTE : Error on window size change. Please ignore.", false, true);
            }
        }
        private void SelectedNPC_OnClick(NPCDataMinimum NewSelectedNPC)
        {
            SelectedNPC.Value = new CharacterButton(NewSelectedNPC);
            ConsoleCommand("Selected NPC slot " + NewSelectedNPC.offset, false, false);
        }
        private void SwapToNPC()
        {
            if (Loaded)
            {
                UpdateModelToNewValues();
            }
        }

        private void SwapToBuilder() //Made spaguetti code back then, gotta work with this now...
        {
            if (LoadedBuilder)
            {
                UpdateModelToNewValuesBuilder();
            }
        }
        private void LoadSelectedNPC_Click(object sender, RoutedEventArgs e)
        {
            byte Bk = 255;
            if (EditingNPC.Value != null)
                Bk = EditingNPC.Value.island;
            EditingNPC.Value = CMNDAT.LoadCMNDATOffset(SelectedNPC.Value.NPC.Value.offset);
            EditingNPC.Value.UpdatedCoords += UpdateCoord;

            var Point = EditingNPC.Value.coordFocus(ImageMap);
            if (Point.X != double.NaN && Point.X != double.PositiveInfinity)
            {
                Map.RenderTransformOrigin = Point;
            }
            if (Bk != EditingNPC.Value.island)
            {
                ZoomSlider.Value = 1;
            }
            SwapToNPC();
            swapGender(EditingNPC.Value.sex, ListText.ArmourList);

            UpdateStyleList();
            MyHelixViewport.ZoomExtents();
            ConsoleCommand("NPC loaded!", false, false);
            //ConsoleCommand(EditingNPC.Value.question.ToString(), false, false);
        }
        private void SaveSelectedNPC_Click(object sender, RoutedEventArgs e)
        {
            EditingNPC.Value.offset = SelectedNPC.Value.NPC.Value.offset;
            CMNDAT.SaveCMNDATOffset(EditingNPC.Value);
            SelectedNPC.Value = new CharacterButton(SelectionList.UpdateCharButton(EditingNPC.Value.offset, EditingNPC.Value));
            ConsoleCommand("NPC saved!", false, false);
        }
        private void UpdateModelToNewValues()
        {
            if (Loaded)
            {
                //Get cloth colour before anything else.
                var ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingNPC.Value.armour);
                ushort IDColour = 0;
                if (ArmourClass != null)
                {
                    IDColour = ArmourClass.Armour.ArmourValues.ColourIDFemale;
                    if (EditingNPC.Value.sex == 1)
                        IDColour = ArmourClass.Armour.ArmourValues.ColourIDMale;
                }
                NPCModel.UpdateAll(EditingNPC.Value, IDColour);
                ModelGroupVisualName.Content = NPCModel.GetFullModel();
                ConsoleCommand("Updated NPC model.", false, false);
            }
        }
        private void UpdateModelToNewValuesBuilder()
        {
            if (LoadedBuilder)
            {
                //Get cloth colour before anything else.
                ComboBoxArmour ArmourClass;
                if (EditingBuilder.Value.mirrorClothes == 0)
                    ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingBuilder.Value.armour);
                else
                    ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == EditingBuilder.Value.mirrorClothes);

                ushort IDColour = ArmourClass.Armour.ArmourValues.ColourIDFemale;
                if (EditingBuilder.Value.sex == 1)
                    IDColour = ArmourClass.Armour.ArmourValues.ColourIDMale;
                BuilderModel.UpdateAll(EditingBuilder.Value, IDColour);
                ModelGroupVisualBuilder.Content = BuilderModel.GetFullModel();
                ConsoleCommand("Updated builder model.", false, false);
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
                ConsoleCommand("ERROR: Information not found.", true, false);
            }

        }
        private void CharTypeUpdate(object sender, RoutedEventArgs e)
        {
            FullModelUpdate(sender, e);
        }
        private void ChangeChar_OnClick(object sender, EventArgs e)
        {
            if (Loaded)
                EditingNPC.Value.charType = (sender as ComboBoxColour).ID;
            EditingNPC.NotifyValue();

            var type = (sender as ComboBoxColour).TypeListing;
            NameDescText.Value = type.Name;
            DescText.Value = type.Description;

            UpdateModelToNewValues();

            ConsoleCommand("Character type changed", false, false);
        }
        private void swapGender(byte gender, ObservableCollection<ComboBoxArmour> List)
        {
            for (int i = 0; i < List.Count; i++)
            {
                ComboBoxArmour BoxCheck = List[i];
                if (BoxCheck.Armour.ImageID != BoxCheck.Armour.ArmourValues.ImageIDFem)
                {
                    BoxCheck.ChangeGender(gender == 2);
                }
            }
            ConsoleCommand("(PLACEHOLDER) ComboBox gender changed. Delete this text.", false, true);
        }
        private void test(object sender, RoutedEventArgs e)
        {
            if (Loaded)
            {
                EditingNPC.NotifyValue();
                swapGender(EditingNPC.Value.sex, ListText.ArmourList);
                UpdateModelToNewValues();
            }

        }
        private void testBuilder(object sender, RoutedEventArgs e)
        {
            if (LoadedBuilder)
            {
                EditingBuilder.NotifyValue();
                swapGender(EditingBuilder.Value.sex, ListText.ArmourBuilderList);
                swapGender(EditingBuilder.Value.sex, ListText.ArmourBuilderListMirror);
                UpdateModelToNewValuesBuilder();
            }
        }

        private void ChangeColour(object sender, RoutedEventArgs e)
        {
            ushort Colour = 0;
            string Text = null;
            bool skin = false;
            if (!Loaded) return;
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
                    break;
                case "1": //hair
                    EditingNPC.Value.hairColour = ColorWindow.ColourPicked;
                    break;
                case "2": //skin
                    EditingNPC.Value.skinColour = ColorWindow.ColourPicked;
                    break;
            }
            UpdateModelToNewValues();
            EditingNPC.NotifyValue();
            ConsoleCommand("Colour changed on NPC.", false, false);
            //TextBoxConsole.Text = "Eye colour changed to " + ColorList + "!";
            //TextBoxConsole.Foreground = new SolidColorBrush(Colors.Green);
        }

        private void ChangeColourBuilder(object sender, RoutedEventArgs e)
        {
            ushort Colour = 0;
            string Text = null;
            bool skin = false;
            if (!LoadedBuilder) return;
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
                    break;
                case "1": //hair
                    EditingBuilder.Value.hairColour = ColorWindow.ColourPicked;
                    break;
                case "2": //skin
                    EditingBuilder.Value.skinColour = ColorWindow.ColourPicked;
                    break;
            }
            UpdateModelToNewValuesBuilder();
            EditingBuilder.NotifyValue();
            ConsoleCommand("Colour changed on Builder.", false, false);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (LoadedBuilder)
            {
                ConsoleCommand("Create only works without a loaded CMNDAT", true, false);
                return;
            }

            EditingNPC.Value = new NPCData(new byte[CMNDAT.SizeOfChar]);
            EditingNPC.Value.bodyModel = 1;
            EditingNPC.Value.faceModel = 1;
            EditingNPC.Value.hairModel = 1;

            EditingNPC.Value.skinColour = 7;
            EditingNPC.Value.eyeColour = 7;
            EditingNPC.Value.hairColour = 7;

            EditingNPC.NotifyValue();
            SwapToNPC();
            swapGender(EditingNPC.Value.sex, ListText.ArmourList);

            MyHelixViewport.ZoomExtents();
            ConsoleCommand("Empty NPC created!", false, false);
        }

        private void UpdateCoord(object sender, EventArgs e)
        {
            if (((TabItem)Tabs.SelectedItem).Name == "TabBuilder")
            {
                EditingBuilder.NotifyValue();
                if (ImageMapBuilder != null && LoadedBuilder)
                    MapBuilder.RenderTransformOrigin = EditingBuilder.Value.coordFocus(ImageMapBuilder);
            }
            else
            {
                EditingNPC.NotifyValue();
                if (ImageMap != null && Loaded)
                    Map.RenderTransformOrigin = EditingNPC.Value.coordFocus(ImageMap);
            }


        }
        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (Tabs.SelectedItem != null && ((TabItem)Tabs.SelectedItem).Name == "TabBuilder")
            {
                // Update the ScaleTransform with the Slider value
                ZoomTransformBuilder.ScaleX = e.NewValue;
                ZoomTransformBuilder.ScaleY = e.NewValue;

                if (ImageMap != null && LoadedBuilder)
                    MapBuilder.RenderTransformOrigin = EditingBuilder.Value.coordFocus(ImageMapBuilder);
            }
            else
            {
                // Update the ScaleTransform with the Slider value
                ZoomTransform.ScaleX = e.NewValue;
                ZoomTransform.ScaleY = e.NewValue;

                if (ImageMap != null && Loaded)
                    Map.RenderTransformOrigin = EditingNPC.Value.coordFocus(ImageMap);
            }

        }

        private void MoveToCoordinates(object sender, MouseButtonEventArgs e)
        {
            if (Tabs.SelectedItem != null && ((TabItem)Tabs.SelectedItem).Name == "TabBuilder")
            {
                Point clickPosition = e.GetPosition(ImageMapBuilder);
                EditingBuilder.Value.RelativeCoordinates((float)clickPosition.X, (float)clickPosition.Y);
                UpdateCoord(null, null);
            }
            else
            {
                Point clickPosition = e.GetPosition(ImageMap);
                EditingNPC.Value.RelativeCoordinates((float)clickPosition.X, (float)clickPosition.Y);
                UpdateCoord(null, null);
            }
        }

        private void UpdateJobConsole(object sender, SelectionChangedEventArgs e)
        {
            ConsoleCommand("Changed Job", true, false);
            var job = ((sender as ComboBox).SelectedItem as Job);
            NameDescText.Value = job.Name;
            DescText.Value = job.Description;
        }

        private void LoadBuilder(object sender, RoutedEventArgs e)
        {
            SwapToBuilder();
        }

        private void StyleChange(object sender, SelectionChangedEventArgs e)
        {
            if (EditingNPC.Value == null) return;
            if (!_isUserInitiated) return;
            EditingNPC.Value.style = (byte)Style.SelectedIndex;
            FullModelUpdate(sender, e);
        }
    }
}

/*
 *         private void test()
        {
            Dictionary<ushort, List<String>> asignment = new Dictionary<ushort, List<string>>();
            foreach (var minimum in SelectionList.StoryChar)
            {
                var npc = DQB2DataEditor.LoadCMNDATOffset(minimum.offset);
                if (npc.job != 0)
                {
                    String ee = npc.charType + " | " + ListText.getTypeCharVal(npc.charType).name;
                    if (!asignment.ContainsKey(npc.job))
                        asignment[npc.job] = new List<String>();

                    asignment[npc.job].Add(ee);
                }
            }
            foreach (var key in asignment.Keys)
            {
                var n = ListText.JobList.FirstOrDefault(x => x.Id == key).Name;
                foreach (var str in asignment[key])
                {
                    Console.WriteLine(key + " " + n + " " + str);
                }

            }
        }
 */