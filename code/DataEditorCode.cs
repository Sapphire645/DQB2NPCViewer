using DQB2NPCViewer;
using System;
using System.IO;


public static class DQB2DataEditor
{

    private static byte[] fileBytes;
    public static string LoadedFile;
    public static bool LoadFile(string filename)
    {
        fileBytes = File.ReadAllBytes(filename);
        if (fileBytes.Length != 608)
        {
            return false;
        }
        LoadedFile = filename;
        var NameBytes = new byte[30];
        var TwoBytes = new byte[2];
        Array.Copy(fileBytes, 0, NameBytes, 0, 30);
        MainWindow.NameNPC = System.Text.Encoding.Default.GetString(NameBytes);

        TwoBytes[0] = fileBytes[0x90];
        TwoBytes[1] = fileBytes[0x91];
        MainWindow.Type = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));
        TwoBytes[0] = fileBytes[0x92];
        TwoBytes[1] = fileBytes[0x93];
        MainWindow.HP = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));

        if ((byte)(fileBytes[0x9C] & 0x40) == 0x40) MainWindow.ClothVisual = true;
        else MainWindow.ClothVisual = false;

        if ((byte)(fileBytes[0x9C] & 0x02) == 0x02) MainWindow.RagVisual = true;
        else MainWindow.RagVisual = false;

        TwoBytes[0] = fileBytes[0xC7];
        TwoBytes[1] = fileBytes[0xC8];
        MainWindow.Weapon = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));

        TwoBytes[0] = fileBytes[0xCF];
        TwoBytes[1] = fileBytes[0xD0];
        MainWindow.Armour = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));

        MainWindow.Island = fileBytes[0xDF];

        TwoBytes[0] = fileBytes[0xE5];
        TwoBytes[1] = fileBytes[0xE6];
        MainWindow.FaceModel = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));
        TwoBytes[0] = fileBytes[0xE7];
        TwoBytes[1] = fileBytes[0xE8];
        MainWindow.HairModel = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));
        TwoBytes[0] = fileBytes[0xE9];
        TwoBytes[1] = fileBytes[0xEA];
        MainWindow.BodyModel = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));
        TwoBytes[0] = fileBytes[0xEB];
        TwoBytes[1] = fileBytes[0xEC];
        MainWindow.EyeColor = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));
        TwoBytes[0] = fileBytes[0xED];
        TwoBytes[1] = fileBytes[0xEE];
        MainWindow.HairColor = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));
        TwoBytes[0] = fileBytes[0xEF];
        TwoBytes[1] = fileBytes[0xF0];
        MainWindow.SkinColor = (ushort)(BitConverter.ToUInt16(TwoBytes, 0));
        MainWindow.Sex = fileBytes[0x102];
        MainWindow.RoomSize = fileBytes[0x107];
        MainWindow.RoomFanciness = fileBytes[0x108];
        MainWindow.RoomAmbience = fileBytes[0x109];
        MainWindow.Dialogue = fileBytes[0x10A];
        MainWindow.Voice = fileBytes[0x10B];
        MainWindow.Job = fileBytes[0x10F];
        MainWindow.Home = fileBytes[0x113];

        if ((fileBytes[0x12E] & 0x10) == 0) MainWindow.TypeVisual = true;
        else MainWindow.TypeVisual = false;

        MainWindow.Place = fileBytes[0x144];
        return true;
    }
    public static void SaveFile(string filename)
    {
        var TwoBytes = new byte[2];
        var NameBytes = new byte[30];

        NameBytes = System.Text.Encoding.Default.GetBytes(MainWindow.NameNPC);
        Array.Copy(NameBytes, 0, fileBytes, 0, NameBytes.Length);

        TwoBytes = BitConverter.GetBytes(MainWindow.Type);
        fileBytes[0x90] = TwoBytes[0];
        fileBytes[0x91] = TwoBytes[1];

        TwoBytes = BitConverter.GetBytes(MainWindow.HP);
        fileBytes[0x92] = TwoBytes[0];
        fileBytes[0x93] = TwoBytes[1];

        if (MainWindow.ClothVisual) fileBytes[0x9C] = (byte)(fileBytes[0x9C] | 0x40);
        else fileBytes[0x9C] = (byte)(fileBytes[0x9C] & 0xBF);

        if (MainWindow.RagVisual) fileBytes[0x9C] = (byte)(fileBytes[0x9C] | 0x02);
        else fileBytes[0x9C] = (byte)(fileBytes[0x9C] & 0xFD);

        fileBytes[0xDF] = (byte)MainWindow.Island;

        TwoBytes = BitConverter.GetBytes(MainWindow.Weapon);
        fileBytes[0xC7] = TwoBytes[0];
        fileBytes[0xC8] = TwoBytes[1];

        TwoBytes = BitConverter.GetBytes(MainWindow.Armour);
        fileBytes[0xCF] = TwoBytes[0];
        fileBytes[0xD0] = TwoBytes[1];

        TwoBytes = BitConverter.GetBytes(MainWindow.FaceModel);
        fileBytes[0xE5] = TwoBytes[0];
        fileBytes[0xE6] = TwoBytes[1];

        TwoBytes = BitConverter.GetBytes(MainWindow.HairModel);
        fileBytes[0xE7] = TwoBytes[0];
        fileBytes[0xE8] = TwoBytes[1];

        TwoBytes = BitConverter.GetBytes(MainWindow.BodyModel);
        fileBytes[0xE9] = TwoBytes[0];
        fileBytes[0xEA] = TwoBytes[1];

        TwoBytes = BitConverter.GetBytes(MainWindow.EyeColor);
        fileBytes[0xEB] = TwoBytes[0];
        fileBytes[0xEC] = TwoBytes[1];

        TwoBytes = BitConverter.GetBytes(MainWindow.HairColor);
        fileBytes[0xED] = TwoBytes[0];
        fileBytes[0xEE] = TwoBytes[1];

        TwoBytes = BitConverter.GetBytes(MainWindow.SkinColor);
        fileBytes[0xEF] = TwoBytes[0];
        fileBytes[0xF0] = TwoBytes[1];

        fileBytes[0x102] = (byte)MainWindow.Sex;

        fileBytes[0x107] = (byte)MainWindow.RoomSize;
        fileBytes[0x108] = (byte)MainWindow.RoomFanciness;
        fileBytes[0x109] = (byte)MainWindow.RoomAmbience;

        fileBytes[0x10A] = (byte)MainWindow.Dialogue;
        fileBytes[0x10B] = (byte)MainWindow.Voice;
        fileBytes[0x10F] = (byte)MainWindow.Job;
        fileBytes[0x113] = (byte)MainWindow.Home;

        if (MainWindow.TypeVisual) fileBytes[0x12E] = (byte)(fileBytes[0x12E] & 0xEF);
        else fileBytes[0x12E] = (byte)(fileBytes[0x12E] | 0x10);

        fileBytes[0x144] = (byte)MainWindow.Place;

        File.WriteAllBytes(filename, fileBytes);
    }
}