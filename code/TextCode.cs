using DQB2NPCViewer.control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DQB2NPCViewer.code
{
    public class ListText
    {
        public List<IslandJob> IslandList = new List<IslandJob>();
        public List<IslandJob> JobList = new List<IslandJob>();
        public ObservableCollection<ComboBoxModel> BodyList = new ObservableCollection<ComboBoxModel>();
        public ObservableCollection<ComboBoxModel> FaceList = new ObservableCollection<ComboBoxModel>();
        public ObservableCollection<ComboBoxModel> HairList = new ObservableCollection<ComboBoxModel>();
        public List<Colour> ColorList = new List<Colour>();
        public List<Colour> DyesList = new List<Colour>();
        public List<AmbianceBox> AmbianceList = new List<AmbianceBox>();
        public List<TypeSet> TypeLockList = new List<TypeSet>();
        public List<String> InfoText = new List<String>();

        public List<Equipment> WeaponList = new List<Equipment>();
        public ObservableCollection<ComboBoxArmour> ArmourList = new ObservableCollection<ComboBoxArmour>();

        public Colour getColorVal(ushort ID) { return ColorList[ID]; }

        public Color getColorDyeVal(ushort ID) { return (Color)ColorConverter.ConvertFromString(DyesList.FirstOrDefault(x => x.ID == ID).color); }

        //Welcome to "Screw JSONs I want to do the think the save editor does.
        //Code from "Info.cs" in Turtle-Insect's save editor.
        public void setList(string filename0, string filename1, string filename2, string filename3,
            string filename4, string filename5, string filename6, string filename7, string filename8, string filename9)
        {

            ConstructColorNames("data/" + filename1 + ".txt", ColorList);
            ConstructColorNames("data/dyecolourResource.txt", DyesList);

            ConstructModelNames("data/" + filename0 + ".txt", "body", BodyList);
            ConstructModelNames("data/" + filename2 + ".txt", "face", FaceList);
            ConstructModelNames("data/" + filename3 + ".txt", "hair", HairList);

            ConstructIJNames("data/" + filename4 + ".txt", IslandList);
            ConstructIJNames("data/" + filename5 + ".txt", JobList);
            ConstructAmbiance("data/" + filename6 + ".txt");
            ConstructTypeLock("data/" + filename7 + ".txt");
            ConstructEquipmentNames("data/" + filename8 + ".txt", WeaponList);
            ConstructArmourNames("data/" + filename9 + ".txt", ArmourList);

            ConstructInfo("data/info.txt");
        }
        private void ConstructInfo(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                InfoText.Add(values[0]);
                InfoText.Add(values[1]);
            }
        }

        private void ConstructEquipmentNames(string filename, List<Equipment> EquipmentList)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                EquipmentList.Add(new Equipment()
                {
                        ID = (ushort)Convert.ToInt16(values[0]),
                        ModelIDMale = (ushort)Convert.ToInt16(values[1]),
                        Name = values[2]
                    });
            }
        }
        private void ConstructArmourNames(string filename, ObservableCollection<ComboBoxArmour> EquipmentList)
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
                var Arm = new ComboBoxArmour()
                {
                    ID = (ushort)Convert.ToInt16(values[0]),
                    Armour = new Equipment()
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

                    }
                };
                Arm.Image = Arm.Armour.Image;
                Arm.Colour = getColorDyeVal(Arm.Armour.ArmourValues.ColourIDMale);
                Arm.SetImage();
                EquipmentList.Add(Arm);
            }
        }

        private void ConstructIJNames(string filename, List<IslandJob> List)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var IJValue = new IslandJob()
                {
                    IJName = values[1],
                    IJId = (ushort)Convert.ToInt16(values[0]),
                    IJDescription = values[2],
                    IJValid = Convert.ToBoolean(values[3])
                };
                List.Add(IJValue);
            }
        }
        private void ConstructColorNames(string filename, List<Colour> List)
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
                    color = values[1]
                };
                List.Add(ColorValue);
            }
        }
        private void ConstructModelNames(string filename, string image, ObservableCollection<ComboBoxModel> List)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                List.Add(new ComboBoxModel
                {
                    ID = (ushort)Convert.ToInt16(values[0]),
                    ModelClassV = new ModelClass()
                    {
                        ID = (ushort)Convert.ToInt16(values[0]),
                        ImageID = (ushort)Convert.ToInt16(values[1]),
                        ModelName = values[2],
                        StringImage = image
                    }
                });
            }
        }
        private void ConstructAmbiance(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var Ambiance = new AmbianceBox()
                {
                    ID = (ushort)Convert.ToInt16(values[0]),
                    AName = values[1]
                };
                AmbianceList.Add(Ambiance);
            }
        }
        private void ConstructTypeLock(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 1) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                try
                {
                    var typeLockVal = new TypeSet()
                    {
                        typeID = (0 < values.Length) ? (ushort)Convert.ToInt16(values[0]) : (ushort)0,
                        name = (1 < values.Length) ? values[1] : "???",
                        hairID = (2 < values.Length) ? (ushort)Convert.ToInt16(values[2]) : (ushort)0,
                        faceID = (3 < values.Length) ? (ushort)Convert.ToInt16(values[3]) : (ushort)0,
                        bodyID = (4 < values.Length) ? (ushort)Convert.ToInt16(values[4]) : (ushort)0,
                        jobID = (5 < values.Length) ? (ushort)Convert.ToInt16(values[5]) : (ushort)0,
                        Tier = (6 < values.Length) ? (ushort)Convert.ToInt16(values[6]) : (ushort)0,
                        Monster = (7 < values.Length) ? Convert.ToBoolean(values[7]) : false
                    };
                    TypeLockList.Add(typeLockVal);
                }
                catch
                {
                    var typeLockVal = new TypeSet();
                    TypeLockList.Add(typeLockVal);
                }

            }
        }
    }
}
