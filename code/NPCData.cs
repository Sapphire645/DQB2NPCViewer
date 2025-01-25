using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace DQB2NPCViewer.code
{
    public class NPCData
    {
        public byte[] byteData { get; }
        public ushort offset {  get; set; }
        public event EventHandler UpdatedCoords;

        public NPCData(byte[] byteData)
        {
            this.byteData = byteData;
        }
        public String name { 
            get{
                var NameBytes = new byte[30];
                Array.Copy(byteData, 0, NameBytes, 0, 30);
                return System.Text.UTF8Encoding.Default.GetString(NameBytes);
            }
            set
            {
                var NameBytes = new byte[30];
                NameBytes = System.Text.UTF8Encoding.Default.GetBytes(value);
                Array.Copy(NameBytes, 0, byteData, 0, 30);
            }
        }

        public float coordX
        {
            get { return System.BitConverter.ToSingle(byteData, 0x5C); }
            set
            {  
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x5C, 4);
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }
        public float coordY
        {
            get { return System.BitConverter.ToSingle(byteData, 0x60); }
            set
            {
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x60, 4);
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }
        public float coordZ
        {
            get { return System.BitConverter.ToSingle(byteData, 0x64); }
            set
            {
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x64, 4);
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }
        public float coordAngle
        {
            get { return System.BitConverter.ToSingle(byteData, 0x8C); }
            set
            {
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x8C, 4);
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }
        public ushort charType
        {
            get
            { return BitConverter.ToUInt16(byteData, 0x90);}
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x90, 2);}
        }
        public ushort HP
        {
            get
            { return BitConverter.ToUInt16(byteData, 0x92); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x92, 2); }
        }
        public bool hasClothes
        {
            get { return (byteData[0x9C] & 0x40) == 0x40; }
            set { byteData[0x9C] = (byte)((byteData[0x9C] & 0xBF) + (((value == true) ? 1 : 0) * 0x40)); }
        }
        public bool hasRags
        {
            get { return (byteData[0x9C] & 0x02) == 0x02; }
            set { byteData[0x9C] = (byte)((byteData[0x9C] & 0xFD) + (((value == true) ? 1 : 0) * 0x02)); }
        }
        public ushort weapon
        {
            get
            { return BitConverter.ToUInt16(byteData, 0xC7); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0xC7, 2); }
        }
        public ushort armour
        {
            get {
                return BitConverter.ToUInt16(byteData, 0xCF);
            }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0xCF, 2); }
        }
        public byte island
        {
            get { return byteData[0xDF]; }
            set { byteData[0xDF] = value;
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }
        public ushort faceModel
        {
            get { return BitConverter.ToUInt16(byteData, 0xE5); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0xE5, 2); }
        }
        public ushort hairModel
        {
            get { return BitConverter.ToUInt16(byteData, 0xE7); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0xE7, 2); }
        }
        public ushort bodyModel
        {
            get { return BitConverter.ToUInt16(byteData, 0xE9); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0xE9, 2); }
        }
        public ushort eyeColour
        {
            get { return BitConverter.ToUInt16(byteData, 0xEB); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0xEB, 2); }
        }
        public ushort hairColour
        {
            get { return BitConverter.ToUInt16(byteData, 0xED); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0xED, 2); }
        }
        public ushort skinColour
        {
            get { return BitConverter.ToUInt16(byteData, 0xEF); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0xEF, 2); }
        }
        public byte sex
        {
            get { return byteData[0x102]; }
            set { byteData[0x102] = value; }
        }
        public bool isMale
        {
            get { return sex == 1; }
            set { if (value) sex = 1; else sex = 2; }
        }
        public bool isFemale
        {
            get { return sex == 2; }
            set { if (value) sex = 2; else sex = 1; }
        }
        public byte roomSize
        {
            get { return byteData[0x107]; }
            set { byteData[0x107] = value; }
        }
        public byte roomFancy
        {
            get { return byteData[0x108]; }
            set { byteData[0x108] = value; }
        }
        public byte roomAmbiance
        {
            get { return byteData[0x109]; }
            set { byteData[0x109] = value; }
        }
        public byte messages
        {
            get { return byteData[0x10A]; }
            set { byteData[0x10A] = value; }
        }
        public byte voice
        {
            get { return byteData[0x10B]; }
            set { byteData[0x10B] = value; }
        }
        public byte job
        {
            get { return byteData[0x10F]; }
            set { byteData[0x10F] = value; }
        }
        public byte nativeHome
        {
            get { return byteData[0x113]; }
            set { byteData[0x113] = value; }
        }
        public bool typeLock
        {
            get { return (byteData[0x12E] & 0x10) != 0x10; }
            set { byteData[0x12E] = (byte)((byteData[0x12E] & 0xEF) + (((value == true) ? 0 : 1) * 0x10)); }
        }
        public byte place
        {
            get { return byteData[0x144]; }
            set { byteData[0x144] = value; }
        }
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
        public ushort tempCoordOffsetX => ListText.CoordinateMap[island].Item1; //On N of tiles
        public ushort tempCoordOffsetZ => ListText.CoordinateMap[island].Item2; //On N of tiles
        public string imageFancy => $"/images/resource/fancy{roomFancy:0}.png";
        public string imageSize => $"/images/resource/size{roomSize:0}.png";
        public string imageCoordinates => $"/images/maps/STGDAT{island:00}.png";
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
                return ListText.getTypeCharVal(charType).name;
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
