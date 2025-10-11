using System;
using System.Windows;
using System.Windows.Controls;

namespace DQB2NPCViewer.code
{
    public class BuilderData
    {
        public byte[] byteData { get; }
        public byte[] byteDataInventory { get; }
        public byte[] byteDataName { get; }
        public byte[] byteDataFlagBools { get; }

        public event EventHandler UpdatedCoords;

        public BuilderData(byte[] byteData, byte[] byteDataName, byte[] byteDataInventory, byte[] byteDataFlagBools, byte island)
        {
            this.byteData = byteData;
            this.byteDataName = byteDataName;
            this.byteDataInventory = byteDataInventory;
            this.byteDataFlagBools = byteDataFlagBools;
            this.island = island;
        }

        public byte island { get; private set; }

        public ushort tempCoordOffsetX => ListText.CoordinateMap[island].Item1; //On N of tiles
        public ushort tempCoordOffsetZ => ListText.CoordinateMap[island].Item2; //On N of tiles
        public string imageCoordinates => $"/images/maps/STGDAT{island:00}.png";
        public Thickness coordMargin => new Thickness(((coordX + 1024) * 2) - (tempCoordOffsetX * 16) - 13, ((coordZ + 1024) * 2) - (tempCoordOffsetZ * 16) - 13, 0, 0);
        public void RelativeCoordinates(float x, float z)
        {
            coordX = (x + 16 * tempCoordOffsetX) / 2 - 1024;
            coordZ = (z + 16 * tempCoordOffsetZ) / 2 - 1024;
        }
        public Point coordFocus(Image image)
        {
            var x = (((coordX + 1024) * 2) - (tempCoordOffsetX * 16)) / image.ActualWidth;
            var y = (((coordZ + 1024) * 2) - (tempCoordOffsetZ * 16)) / image.ActualHeight;

            x = x * 1.1 - 0.05;
            y = y * 1.1 - 0.05;
            return new Point(x, y);
        }
        public float coordX
        {
            get { return System.BitConverter.ToSingle(byteData, 0x16); }
            set
            {
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x16, 4);
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }
        public float coordY
        {
            get { return System.BitConverter.ToSingle(byteData, 0x1A); }
            set
            {
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x1A, 4);
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }
        public float coordZ
        {
            get { return System.BitConverter.ToSingle(byteData, 0x1E); }
            set
            {
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x1E, 4);
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }
        public float coordAngle
        {
            get { return System.BitConverter.ToSingle(byteData, 0x24); }
            set
            {
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x24, 4);
                UpdatedCoords?.Invoke(null, EventArgs.Empty);
            }
        }

        public String name
        {
            get
            {
                var NameBytes = new byte[12];
                Array.Copy(byteDataName, 0, NameBytes, 0, 12);
                return System.Text.Encoding.UTF8.GetString(NameBytes);
            }
            set
            {
                var NameBytes = new byte[12];
                NameBytes = System.Text.Encoding.UTF8.GetBytes(value);
                Array.Copy(NameBytes, 0, byteDataName, 0, 12);
            }
        }
        public ushort hairColour
        {
            get { return BitConverter.ToUInt16(byteData, 0x10); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x10, 2); }
        }
        public ushort eyeColour
        {
            get { return BitConverter.ToUInt16(byteData, 0x12); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x12, 2); }
        }
        public ushort skinColour
        {
            get { return BitConverter.ToUInt16(byteData, 0x14); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x14, 2); }
        }
        public ushort gratitude
        {
            get { return BitConverter.ToUInt16(byteData, 0x28); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x28, 2); }
        }
        public ushort HP
        {
            get { return BitConverter.ToUInt16(byteData, 0x2A); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x2A, 2); }
        }
        public ushort maxHP
        {
            get { return BitConverter.ToUInt16(byteData, 0x2C); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x2C, 2); }
        }
        public ushort hungerDecay
        {
            get { return BitConverter.ToUInt16(byteData, 0x2E); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x2E, 2); }
        }
        public ushort hunger
        {
            get { return BitConverter.ToUInt16(byteData, 0x30); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x30, 2); }
        }
        public ushort attack
        {
            get { return BitConverter.ToUInt16(byteData, 0x32); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x32, 2); }
        }
        public ushort defense
        {
            get { return BitConverter.ToUInt16(byteData, 0x34); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x34, 2); }
        }
        public float currentStamina
        {
            get { return System.BitConverter.ToSingle(byteData, 0x36); }
            set
            {
                var floatBytes = System.BitConverter.GetBytes(value);
                Array.Copy(floatBytes, 0, byteData, 0x36, 4);
            }
        }
        public ushort maxStamina
        {
            get { return BitConverter.ToUInt16(byteData, 0x3A); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x3A, 2); }
        }

        public ushort mirrorWeapon
        {
            get { return BitConverter.ToUInt16(byteData, 0x5c); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x5c, 2); }
        }
        public ushort mirrorHammer
        {
            get { return BitConverter.ToUInt16(byteData, 0x60); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x60, 2); }
        }
        public ushort mirrorClothes
        {
            get { return BitConverter.ToUInt16(byteData, 0x64); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x64, 2); }
        }
        public ushort mirrorShield
        {
            get { return BitConverter.ToUInt16(byteData, 0x68); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x68, 2); }
        }
        public ushort mirrorHair
        {
            get { return BitConverter.ToUInt16(byteData, 0x6C); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x6C, 2); }
        }
        public ushort mirrorAccesory1
        {
            get { return BitConverter.ToUInt16(byteData, 0x70); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x70, 2); }
        }
        public ushort mirrorAccesory2
        {
            get { return BitConverter.ToUInt16(byteData, 0x74); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x74, 2); }
        }
        public ushort mirrorAccesory3
        {
            get { return BitConverter.ToUInt16(byteData, 0x78); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x78, 2); }
        }
        public byte sex
        {
            get { return byteData[0x86]; }
            set { byteData[0x86] = value; }
        }
        public ushort LV
        {
            get { return BitConverter.ToUInt16(byteData, 0x169); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x169, 2); }
        }
        public uint exp
        {
            get { return BitConverter.ToUInt16(byteData, 0x16B); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteData, 0x16B, 4); }
        }
        public byte maxLV
        {
            get { return byteData[0x16F]; }
            set { byteData[0x16F] = value; }
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
        public ushort bodyModelBase => (sex == 1) ? (ushort)29 : (sex == 2) ? (ushort)30 : (ushort)0;
        public ushort hairModelBase => (sex == 1) ? (ushort)41 : (sex == 2) ? (ushort)42 : (ushort)0;
        public ushort faceModelBase => (sex == 1) ? (ushort)1 : (sex == 2) ? (ushort)2 : (ushort)0;



        public ushort weapon
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x0); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x0, 2); }
        }

        public ushort hammer
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x4); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x4, 2); }
        }
        public bool hasHammer
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x6) > 0; }
            set { byteDataInventory[0x6] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort gloves
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x8); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x8, 2); }
        }
        public bool hasGloves
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0xA) > 0; }
            set { byteDataInventory[0xA] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort pencil
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0xC); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0xC, 2); }
        }
        public bool hasPencil
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0xE) > 0; }
            set { byteDataInventory[0xE] = (byte)((value == true) ? 1 : 0); }
        }

        public ushort trowel
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x10); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x10, 2); }
        }
        public bool hasTrowel
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x12) > 0; }
            set { byteDataInventory[0x12] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort flute
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x14); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x14, 2); }
        }
        public bool hasFlute
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x16) > 0; }
            set { byteDataInventory[0x16] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort chisel
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x18); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x18, 2); }
        }
        public bool hasChisel
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x1A) > 0; }
            set { byteDataInventory[0x1A] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort pot
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x1C); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x1C, 2); }
        }
        public bool hasPot
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x1E) > 0; }
            set { byteDataInventory[0x1E] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort rod
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x20); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x20, 2); }
        }
        public bool hasRod
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x22) > 0; }
            set { byteDataInventory[0x22] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort unused1
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x24); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x24, 2); }
        }
        public bool hasUnused1
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x26) > 0; }
            set { byteDataInventory[0x26] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort unused2
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x28); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x28, 2); }
        }
        public bool hasUnused2
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x2A) > 0; }
            set { byteDataInventory[0x2A] = (byte)((value == true) ? 1 : 0); }
        }
        public ushort shield
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x2C); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x2C, 2); }
        }
        public ushort armour
        {
            get { return BitConverter.ToUInt16(byteDataInventory, 0x30); }
            set { var bytes = BitConverter.GetBytes(value); Array.Copy(bytes, 0, byteDataInventory, 0x30, 2); }
        }

    }
}
