using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace DQB2NPCViewer.code
{
    public class NPCData
    {

        public ushort faceModelDisplay { 
            get
            {
                if (!typeLock & faceModel != 0)
                    return faceModel;
                if (ListText.getTypeCharVal(charType).Models.ContainsKey(style))
                    return ListText.getTypeCharVal(charType).Models[style].FaceModel;
                return 0;
            }
        }
        public ushort hairModelDisplay {
            get
            {
                if (!typeLock & hairModel != 0)
                    return hairModel;
                if (ListText.getTypeCharVal(charType).Models.ContainsKey(style))
                    return ListText.getTypeCharVal(charType).Models[style].HairModel;
                return 0;
            }
        }
        public ushort bodyModelDisplay
        {
            get
            {
                if (hasRags) return (ushort)(sex == 1 ? 31 : 32);
                if (hasClothes)
                {
                    var ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == armour);
                    if(ArmourClass.Armour.ID != 0)
                    {
                        return (ushort)(sex == 1 ? ArmourClass.Armour.ModelIDMale : ArmourClass.Armour.ArmourValues.ModelIDFemale);
                    }
                }
                if (!typeLock & bodyModel != 0)
                    return bodyModel;
                if (ListText.getTypeCharVal(charType).Models.ContainsKey(style))
                    return ListText.getTypeCharVal(charType).Models[style].BodyModel;
                return 0;
            }
        }

        public byte[] byteData { get; }
        public ushort offset {  get; set; }
        public event EventHandler UpdatedCoords;

        public NPCData(byte[] byteData)
        {
            this.byteData = byteData;
        }
        public void SetFloat(float value, ushort offset)
        {
            var floatBytes = System.BitConverter.GetBytes(value);
            Array.Copy(floatBytes, 0, byteData, offset, 4);
        }
        public void SetUshort(ushort value, ushort offset)
        {
            var bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, byteData, offset, 2);
        }

        public void SetBool(bool value, ushort offset, byte place)
        {
            byteData[offset] = (byte)((byteData[offset] & (0xFF-place)) + (((value == true) ? 1 : 0) * place));
        }

        public String name { 
            get{
                var NameBytes = new byte[30];
                Array.Copy(byteData, 0, NameBytes, 0, 30);
                return System.Text.Encoding.UTF8.GetString(NameBytes);
            }
            set
            {
                var NameBytes = new byte[30];
                NameBytes = System.Text.Encoding.UTF8.GetBytes(value);
                Array.Copy(NameBytes, 0, byteData, 0, 30);
            }
        }
        public float coordX{ get => System.BitConverter.ToSingle(byteData, 0x5C); 
            set{ SetFloat(value, 0x5C); UpdatedCoords?.Invoke(null, EventArgs.Empty);} }
        public float coordY { get => System.BitConverter.ToSingle(byteData, 0x60);
            set { SetFloat(value, 0x60); UpdatedCoords?.Invoke(null, EventArgs.Empty); } }
        public float coordZ { get => System.BitConverter.ToSingle(byteData, 0x64); 
            set { SetFloat(value, 0x64); UpdatedCoords?.Invoke(null, EventArgs.Empty); } }
        public float coordAngle { get => System.BitConverter.ToSingle(byteData, 0x8C); 
            set { SetFloat(value, 0x8C); UpdatedCoords?.Invoke(null, EventArgs.Empty); } }
        public ushort charType { get => BitConverter.ToUInt16(byteData, 0x90); set => SetUshort(value, 0x90); }
        public ushort HP { get => BitConverter.ToUInt16(byteData, 0x92); set => SetUshort(value, 0x92); }
        public bool hasClothes { get => (byteData[0x9C] & 0x40) == 0x40; set => SetBool(value, 0x9C, 0x40); }
        public bool hasRags { get => (byteData[0x9C] & 0x02) == 0x02; set => SetBool(value, 0x9C, 0x02); }
        public ushort weapon { get => BitConverter.ToUInt16(byteData, 0xC7); set => SetUshort(value, 0xC7); }
        public ushort armour { get => BitConverter.ToUInt16(byteData, 0xCF); set => SetUshort(value, 0xCF); }
        public byte island { get =>  byteData[0xDF]; set{ byteData[0xDF] = value;UpdatedCoords?.Invoke(null, EventArgs.Empty);}}
        public ushort faceModel { get => BitConverter.ToUInt16(byteData, 0xE5); set => SetUshort(value, 0xE5); }
        public ushort hairModel { get => BitConverter.ToUInt16(byteData, 0xE7); set => SetUshort(value, 0xE7); }
        public ushort bodyModel { get => BitConverter.ToUInt16(byteData, 0xE9); set => SetUshort(value, 0xE9); }
        public ushort eyeColour { get => BitConverter.ToUInt16(byteData, 0xEB); set => SetUshort(value, 0xEB); }
        public ushort hairColour { get => BitConverter.ToUInt16(byteData, 0xED); set => SetUshort(value, 0xED); }
        public ushort skinColour { get => BitConverter.ToUInt16(byteData, 0xEF); set => SetUshort(value, 0xEF); }
        public byte sex { get => byteData[0x102]; set => byteData[0x102] = value; }
        public bool isMale{ get => sex == 1; set { if (value) sex = 1; else sex = 2; } } //Problematic
        public bool isFemale{ get => sex == 2;  set { if (value) sex = 2; else sex = 1; }}
        public bool canBattle { get => (byteData[0x103] & 0x02) == 0x02; set => SetBool(value, 0x103, 0x02); }
        public byte roomSize{get => byteData[0x107]; set => byteData[0x107] = value; }
        public byte roomFancy{ get => byteData[0x108]; set => byteData[0x108] = value; }
        public byte roomAmbiance { get => byteData[0x109]; set => byteData[0x109] = value; }
        public byte messages { get => byteData[0x10A]; set => byteData[0x10A] = value; }
        public byte voice { get => byteData[0x10B]; set => byteData[0x10B] = value; }
        public byte style { get => byteData[0x10D]; set => byteData[0x10D] = value; }
        public byte job { get => byteData[0x10F]; set => byteData[0x10F] = value; }

        public byte commonName { get => byteData[0x112]; set => byteData[0x112] = value; }
        public byte nativeHome { get => byteData[0x113]; set => byteData[0x113] = value; }
        //public bool question { get => (byteData[0x133] & 0x01) == 0x01; set => SetBool(value, 0x133, 0x01); }
        public bool useCommonName { get => (byteData[0x12D] & 0x80) == 0x80; set => SetBool(value, 0x12D, 0x80); }
        public bool typeLock { get => (byteData[0x12E] & 0x10) != 0x10; set => SetBool(!value, 0x12E, 0x10); }
        public bool isHidden { get => (byteData[0x133] & 0x08) == 0x08; set => SetBool(value, 0x133, 0x08); }
        public byte place { get => byteData[0x144]; set => byteData[0x144] = value; }
        public Thickness coordMargin => new Thickness(((coordX+1024)*2) - (tempCoordOffsetX*16)-13, ((coordZ+1024)*2) - (tempCoordOffsetZ*16)-13, 0,0);
        public Point coordFocus(Image image)
        {
            var x = (((coordX + 1024) * 2) - (tempCoordOffsetX * 16)) / image.ActualWidth;
            var y = (((coordZ + 1024) * 2) - (tempCoordOffsetZ * 16)) / image.ActualHeight;

            x = x * 1.1 -0.05;
            y = y * 1.1 - 0.05;
            return new Point(x,y);
        }
        public void RelativeCoordinates(float x, float z)
        {
            coordX = (x + 16 * tempCoordOffsetX) / 2 - 1024;
            coordZ = (z + 16 * tempCoordOffsetZ) / 2 - 1024;
        }
        public ushort tempCoordOffsetX => island < ListText.CoordinateMap.Count ? ListText.CoordinateMap[island].Item1 : (ushort)0; //On N of tiles
        public ushort tempCoordOffsetZ => island < ListText.CoordinateMap.Count ? ListText.CoordinateMap[island].Item2 : (ushort)0; //On N of tiles
        public string imageFancy => $"/images/resource/fancy{roomFancy:0}.png";
        public string imageSize => $"/images/resource/size{roomSize:0}.png";
        public string imageCoordinates => ListText.IslandList.Any(x => x.Id == island) ? $"/images/maps/STGDAT{island:00}.png" : $"/images/resource/icon/10.png";
    }
    public class NPCDataMinimum
    {
        public ushort offset { get; private set; }
        public ushort charType { get; private set; }
        public byte island { get; private set; }

        public byte place { get; private set; }

        public string IslandImage => $"/images/island/{island:0}.png";
        public string PlaceImage => $"/images/island/p{place:0}.png";
        public Visibility PlaceVisibility => island == 1 ? Visibility.Visible : Visibility.Collapsed;
        public string name { get; private set; }

        public NPCDataMinimum(ushort offset, NPCData byteData)
        {
            this.offset = offset;
            charType = byteData.charType;
            name = byteData.name;
            island = byteData.island;
            place = byteData.place;
        }
        public string CharNameGet
        {
            get { 
                return ListText.getTypeCharVal(charType).Name;
            }
        }
        public CroppedBitmap Image
        {
            get
            {
                var TypeVar = charType;
                if (TypeVar < 1 || TypeVar > 900) // Validate number range for a 5x5 grid
                {
                    Int32Rect iconRect2 = new Int32Rect(0, 0, 124, 124);
                    return new CroppedBitmap(ListText.NullImage, iconRect2);
                }
                try
                {
                    BitmapImage gridImage = ListText.IconImage;
                    // Calculate row and column in the 5x5 grid
                    int row = (TypeVar - 1) / 30;
                    int col = (TypeVar - 1) % 30;

                    // Calculate the x and y coordinates of the top-left corner of the icon
                    int x = col * 124;
                    int y = row * 124;

                    // Ensure the icon is within the grid's bounds
                    if (x + 124 > 3720 || y + 124 > 3720)
                    {
                        Int32Rect iconRect2 = new Int32Rect(0, 0, 124, 124);
                        return new CroppedBitmap(ListText.AnonImage, iconRect2);
                    }
                    // Crop the icon from the grid
                    Int32Rect iconRect = new Int32Rect(x, y, 124, 124);
                    var croppedIcon = new CroppedBitmap(gridImage, iconRect);
                    return croppedIcon;
                }
                catch
                {
                    Int32Rect iconRect2 = new Int32Rect(0, 0, 124, 124);
                    return new CroppedBitmap(ListText.AnonImage, iconRect2);
                }
            }
        }
    }
}
