using DQB2NPCViewer.control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Media;

namespace DQB2NPCViewer.code
{
    public class ListText
    {
        public List<Island> IslandList = new List<Island>();
        public List<Job> JobList = new List<Job>();
        public ObservableCollection<ComboBoxBody> BodyList = new ObservableCollection<ComboBoxBody>();
        public ObservableCollection<ComboBoxFace> FaceList = new ObservableCollection<ComboBoxFace>();
        public ObservableCollection<ComboBoxHair> HairList = new ObservableCollection<ComboBoxHair>();
        public List<Colour> ColorList = new List<Colour>();
        public List<AmbianceBox> AmbianceList = new List<AmbianceBox>();
        public List<TypeSet> TypeLockList = new List<TypeSet>();

        public List<Equipment> WeaponList = new List<Equipment>();
        public List<Equipment> ArmourList = new List<Equipment>();

        public Colour getColorVal(ushort ID) { return ColorList[ID]; }
        public void setList(string filename0, string filename1, string filename2, string filename3,
            string filename4, string filename5, string filename6, string filename7, string filename8, string filename9)
        {
            ConstructBodyNames("data/" + filename0 + ".txt");
            ConstructColorNames("data/" + filename1 + ".txt");
            ConstructFaceNames("data/" + filename2 + ".txt");
            ConstructHairNames("data/" + filename3 + ".txt");
            ConstructIslandNames("data/" + filename4 + ".txt");
            ConstructJobNames("data/" + filename5 + ".txt");
            ConstructAmbiance("data/" + filename6 + ".txt");
            ConstructTypeLock("data/" + filename7 + ".txt");
            ConstructEquipmentNames("data/" + filename8 + ".txt", WeaponList);
            ConstructEquipmentNames("data/" + filename9 + ".txt", ArmourList);
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
                var EquVal = new Equipment()
                {
                    ID = (ushort)Convert.ToInt16(values[0]),
                    ModelID = (ushort)Convert.ToInt16(values[1]),
                    Name = values[2]
                };
                EquipmentList.Add(EquVal);
            }
        }

        //Welcome to "Screw JSONs I want to do the think the save editor does.
        //Code from "Info.cs" in Turtle-Insect's save editor.
        private void ConstructIslandNames(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                var IslandValue = new Island()
                {
                    IslandName = values[1],
                    IslandId = (ushort)Convert.ToInt16(values[0]),
                    IslandValid = Convert.ToBoolean(values[2])
                };
                IslandList.Add(IslandValue);
            }
        }
        private void ConstructColorNames(string filename)
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
                ColorList.Add(ColorValue);
            }
        }
        private void ConstructJobNames(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            String Description = "N/A";
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                if (values.Length == 3) { Description = values[2]; } else { Description = "N/A"; };
                var JobValue = new Job()
                {
                    JobId = (ushort)Convert.ToInt16(values[0]),
                    JobName = values[1],
                    JobDescription = Description
                };
                JobList.Add(JobValue);
            }
        }
        private void ConstructBodyNames(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                BodyList.Add(new ComboBoxBody
                {
                    ID = (ushort)Convert.ToInt16(values[0]),
                    BodyModelClassV = new BodyModelClass()
                    {
                        ID = (ushort)Convert.ToInt16(values[0]),
                        ImageID = (ushort)Convert.ToInt16(values[1]),
                        BodyName = values[2]
                    }
                });
            }
        }
            private void ConstructFaceNames(string filename)
            {
                if (!System.IO.File.Exists(filename)) return;
                String[] lines = System.IO.File.ReadAllLines(filename);
                foreach (String line in lines)
                {
                    if (line.Length < 3) continue;
                    if (line[0] == '#') continue;
                    String[] values = line.Split('\t');
                    FaceList.Add(new ComboBoxFace
                    {
                        ID = (ushort)Convert.ToInt16(values[0]),
                        FaceModelClassV = new FaceModelClass()
                        {
                            ID = (ushort)Convert.ToInt16(values[0]),
                            ImageID = (ushort)Convert.ToInt16(values[1]),
                            FaceName = values[2]
                        }
                    });
                }
            }
        private void ConstructHairNames(string filename)
        {
            if (!System.IO.File.Exists(filename)) return;
            String[] lines = System.IO.File.ReadAllLines(filename);
            foreach (String line in lines)
            {
                if (line.Length < 3) continue;
                if (line[0] == '#') continue;
                String[] values = line.Split('\t');
                HairList.Add(new ComboBoxHair
                {
                    ID = (ushort)Convert.ToInt16(values[0]),
                    HairModelClassV = new HairModelClass()
                    {
                        ID = (ushort)Convert.ToInt16(values[0]),
                        ImageID = (ushort)Convert.ToInt16(values[1]),
                        HairName = values[2]
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
                        typeID =    (0 < values.Length) ? (ushort)Convert.ToInt16(values[0]) : (ushort)0,
                        name =      (1 < values.Length) ? values[1] : "???",
                        hairID =    (2 < values.Length) ? (ushort)Convert.ToInt16(values[2]) : (ushort)0,
                        faceID = (3 < values.Length) ? (ushort)Convert.ToInt16(values[3]) : (ushort)0,
                        bodyID =    (4 < values.Length) ? (ushort)Convert.ToInt16(values[4]) : (ushort)0,
                        jobID =  (5 < values.Length) ? (ushort)Convert.ToInt16(values[5]) : (ushort)0,
                        Tier =    (6 < values.Length) ? (ushort)Convert.ToInt16(values[6]) : (ushort)0,
                        Monster = (7 < values.Length) ? Convert.ToBoolean(values[7]) : false
                    };
                    TypeLockList.Add(typeLockVal);
                }
                catch (Exception ex) {
                    var typeLockVal = new TypeSet();
                    TypeLockList.Add(typeLockVal);
                }

            }
        }
    }
}
