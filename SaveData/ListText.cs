using DQB2NPCViewer.control;
using DQB2NPCViewer.SaveData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DQB2NPCViewer.code
{
    public static class ListText
    {

        public static readonly BitmapImage IconImage = new BitmapImage(new Uri("pack://application:,,,/data/icon.png"));
        public static readonly BitmapImage AnonImage = new BitmapImage(new Uri("pack://application:,,,/data/anon.png"));
        public static readonly BitmapImage NullImage = new BitmapImage(new Uri("pack://application:,,,/data/null.png"));
        public static List<Island> IslandList = new List<Island>();
        public static List<Job> JobList = new List<Job>();
        public static List<Place> PlaceList { get; } = new List<Place>();
        public static ObservableCollection<ComboBoxModel> BodyList = new ObservableCollection<ComboBoxModel>();
        public static ObservableCollection<ComboBoxModel> FaceList = new ObservableCollection<ComboBoxModel>();
        public static ObservableCollection<ComboBoxModel> HairList = new ObservableCollection<ComboBoxModel>();

        public static List<Accesory> HairBuilderList = new List<Accesory>();
        public static List<Accesory> AccesoryList = new List<Accesory>();

        public static List<Colour> ColorList = new List<Colour>();
        public static List<Colour> DyesList = new List<Colour>();
        public static List<AmbianceBox> AmbianceList = new List<AmbianceBox>();

        public static ObservableCollection<Hearts> RoomFancyList = new ObservableCollection<Hearts>();
        public static ObservableCollection<Hearts> RoomSizeList = new ObservableCollection<Hearts>();
        public static ObservableCollection<ComboBoxColour> TypeLockList { get; } = new ObservableCollection<ComboBoxColour>();
        public static List<String> InfoText = new List<String>();
        private static Dictionary<ushort, CHARlock> CharLockList;

        public static ObservableCollection<ComboBoxArmour> ArmourList = new ObservableCollection<ComboBoxArmour>();
        public static ObservableCollection<ComboBoxArmour> ArmourBuilderList = new ObservableCollection<ComboBoxArmour>();
        public static ObservableCollection<ComboBoxArmour> ArmourBuilderListMirror = new ObservableCollection<ComboBoxArmour>();
        public static List<Weapon> WeaponList = new List<Weapon>();
        public static List<Weapon> ToolList = new List<Weapon>();
        public static List<Weapon> ShieldList = new List<Weapon>();

        public static List<(ushort,ushort)> CoordinateMap = new List<(ushort, ushort)>();

        public static List<Place> DefaultState;
        public static Colour getColorVal(ushort ID) { return ColorList[ID]; }

        public static Color getColorDyeVal(ushort ID) { return (Color)ColorConverter.ConvertFromString(DyesList.FirstOrDefault(x => x.ID == ID).color); }

        public static CHARlock getTypeCharVal(ushort ID) { if(CharLockList.ContainsKey(ID)) return CharLockList[ID]; return CharLockList[0]; }

        //Welcome to "Screw JSONs I want to do the think the save editor does.
        //Code from "Info.cs" in Turtle-Insect's save editor.
        public static void setList(string filename0, string filename1, string filename2, string filename3,
            string filename4, string filename5, string filename6, string filename7, string filename8, string filename9, string filename10,
            string filename11, string filename12, string filename13, string filename14, string filename15)
        {
            DefaultState = new List<Place>();
            DefaultState.Add(new Place() { Id = 0, Name = "Null" });

            ConstructColorNames("data/" + filename1 + ".txt", ColorList);
            ConstructColorNames("data/dyecolourResource.txt", DyesList);

            ConstructModelNames("data/" + filename0 + ".txt", "body", BodyList);
            ConstructModelNames("data/" + filename2 + ".txt", "face", FaceList);
            ConstructModelNames("data/" + filename3 + ".txt", "hair", HairList);

            ConstructIslandNames("data/" + filename4 + ".txt", IslandList);
            ConstructIJNames("data/" + filename5 + ".txt", JobList);
            
            ConstructAmbiance("data/" + filename6 + ".txt");
            ConstructTypeLock("data/" + filename7 + ".json", "data/" + filename7+"Text.txt");
            ConstructEquipmentNames("data/" + filename8 + ".txt", WeaponList);
            ConstructArmourNames("data/" + filename9 + ".txt", ArmourList, ArmourBuilderList, ArmourBuilderListMirror);

            ConstructPlaceNames("data/" + filename10 + ".txt", PlaceList);

            ConstructInfo("data/info.txt");
            ConstructHairItems("data/" + filename11 + ".txt", HairBuilderList);
            ConstructHairItems("data/" + filename12 + ".txt", AccesoryList);

            ConstructEquipmentNames("data/" + filename13 + ".txt", ToolList);
            ConstructEquipmentNames("data/" + filename14 + ".txt", ShieldList);

            ConstructCoords("data/" + filename15 + ".txt");

            CreateComboBoxHearts();
        }

        private static void CreateComboBoxHearts()
        {
            for (byte i = 0; i < 6; i++)
            {
                var HeartsVar = new Hearts(i, "size");
                RoomSizeList.Add(HeartsVar);
                HeartsVar = new Hearts(i, "fancy");
                RoomFancyList.Add(HeartsVar);
            }
        }
        private static void ConstructInfo(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                if (values.Length < 3) continue;
                InfoText.Add(values[1]);
                InfoText.Add(values[2]);
            }
        }
        private static void ConstructCoords(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                if (values.Length < 3) continue;
                CoordinateMap.Add((ushort.Parse(values[1]), ushort.Parse(values[2])));
            }
        }
        private static void ConstructHairItems(string filename, List<Accesory> list)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                if (values.Length == 5)
                {
                    list.Add(new Accesory(ushort.Parse(values[0]), ushort.Parse(values[1]), ushort.Parse(values[2]), ushort.Parse(values[3]), values[4]));
                }else
                    if (values.Length == 4)
                {
                    list.Add(new Accesory(ushort.Parse(values[0]), ushort.Parse(values[1]), ushort.Parse(values[2]), values[3]));
                }
            }
        }

        private static void ConstructEquipmentNames(string filename, List<Weapon> EquipmentList)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                EquipmentList.Add(new Weapon()
                {
                    ItemID = ushort.Parse(values[0]),
                    ImageID = ushort.Parse(values[1]),
                    PowerValue = ushort.Parse(values[2]),
                    Name = values[3]
                });
            }
        }
        private static void ConstructArmourNames(string filename, ObservableCollection<ComboBoxArmour> EquipmentList, ObservableCollection<ComboBoxArmour> ArmourBuilderList,
             ObservableCollection<ComboBoxArmour> ArmourBuilderListMirror)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var Female = (ushort)Convert.ToInt16(values[6]);
                if (values.Length == 8)
                {
                    Female = (ushort)Convert.ToInt16(values[7]);
                }
                var Eq = new Equipment()
                {
                    ID = (ushort)Convert.ToInt16(values[0]),
                    ModelIDMale = (ushort)Convert.ToInt16(values[1]),
                    ImageID = (ushort)Convert.ToInt16(values[3]),
                    Name = values[5],
                    ArmourValues = new ArmourSub()
                    {
                        ImageIDFem = (ushort)Convert.ToInt16(values[4]),
                        ColourIDMale = (ushort)Convert.ToInt16(values[6]),
                        ColourIDFemale = Female,
                        ModelIDFemale = (ushort)Convert.ToInt16(values[2])
                    }

                };
                var Arm = new ComboBoxArmour((ushort)Convert.ToInt16(values[0]), Eq, true);
                EquipmentList.Add(Arm);
                Arm = new ComboBoxArmour((ushort)Convert.ToInt16(values[0]), Eq, true);
                ArmourBuilderList.Add(Arm);
                Arm = new ComboBoxArmour((ushort)Convert.ToInt16(values[0]), Eq, true);
                ArmourBuilderListMirror.Add(Arm);
            }
        }

        private static void ConstructIJNames(string filename, List<Job> List)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var IJValue = new Job()
                {
                    Name = values[1],
                    Id = (byte)Convert.ToInt16(values[0]),
                    Description = values[2],
                    Valid = Convert.ToBoolean(values[3]),
                    Size = 0
                };
                List.Add(IJValue);
            }
        }
        private static void ConstructIslandNames(string filename, List<Island> List)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var IJValue = new Island()
                {
                    Name = values[1],
                    Id = (byte)Convert.ToInt16(values[0]),
                    Description = values[2],
                    Valid = Convert.ToBoolean(values[3])
                };
                List.Add(IJValue);
            }
        }
        private static void ConstructPlaceNames(string filename, List<Place> List)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 1) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var IJValue = new Place()
                {
                    Name = values[1],
                    Id = (byte)Convert.ToInt16(values[0]),
                };
                List.Add(IJValue);
            }
        }
        private static void ConstructColorNames(string filename, List<Colour> List)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var ColorValue = new Colour()
                {
                    ID = (ushort)Convert.ToInt16(values[0]),
                    color = values[1].Substring(0, 7)
                };
                List.Add(ColorValue);
            }
        }
        private static void ConstructModelNames(string filename, string image, ObservableCollection<ComboBoxModel> List)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                List.Add(new ComboBoxModel
                ((ushort)Convert.ToInt16(values[0]),new ModelClass()
                    {
                        ID = (ushort)Convert.ToInt16(values[0]),
                        ImageID = (ushort)Convert.ToInt16(values[1]),
                        ModelName = values[2],
                        StringImage = image
                    }
                ));
            }
        }
        private static void ConstructAmbiance(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var Ambiance = new AmbianceBox((byte)Convert.ToInt16(values[0]))
                {
                    AName = values[1]
                };
                AmbianceList.Add(Ambiance);
            }
        }
        private static void ConstructTypeLock(string filename, string filenameText)
        {
            if (!System.IO.File.Exists(filename)) return;

            string json = File.ReadAllText(filename);

            CharLockList = JsonSerializer.Deserialize<Dictionary<ushort, CHARlock>>(json)!;

            String[] linesText = System.IO.File.ReadAllLines(filenameText);
            for(int Index = 0; Index < linesText.Length-1; Index++)
            {
                String lineText = linesText[Index];
                if (lineText.Length < 1) continue;
                if (lineText[0] == '#') continue;
                String[] valuesText = lineText.Split('\t');
                try
                {
                    CharLockList[(ushort)Convert.ToInt16(valuesText[0])].Name = valuesText[1];
                }
                catch
                {
                }
            }
            foreach (var chara in CharLockList){
                chara.Value.ID = chara.Key;

                var typeLockVal = new ComboBoxColour(chara.Value, chara.Value.ID);
                if (typeLockVal.TypeListing.Models.Count == 0)
                {
                    typeLockVal.Background = new SolidColorBrush(Colors.LightCoral);
                }
                TypeLockList.Add(typeLockVal);
            }
        }
    }
}
