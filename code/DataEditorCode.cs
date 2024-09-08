using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using System.IO;
using DQB2NPCViewer;


public static class DQB2DataEditor
{

    private static byte[] fileBytes;
    public static string LoadedFile;
    public static void LoadFile(string filename)
    {
        fileBytes = File.ReadAllBytes(filename);
        if (fileBytes.Length != 608)
        {
            return;
        }
        LoadedFile = filename;
        var NameBytes = new byte[30];
        var TwoBytes = new byte[2];
        Array.Copy(fileBytes, 0, NameBytes, 0, 30);
        MainWindow.NameNPC = System.Text.Encoding.Default.GetString(NameBytes);

        TwoBytes[0] = fileBytes[0x92];
        TwoBytes[1] = fileBytes[0x93];
        MainWindow.HP = (ushort)(BitConverter.ToUInt16(TwoBytes,0));

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
        MainWindow.Dialogue = fileBytes[0x10A];
        MainWindow.Voice = fileBytes[0x10B];
        MainWindow.Job = fileBytes[0x10F];
        MainWindow.Home = fileBytes[0x113];
        MainWindow.Place = fileBytes[0x144];
    }
    public static void SaveFile(string filename)
    {
        var TwoBytes = new byte[2];
        var NameBytes = new byte[30];

        NameBytes = System.Text.Encoding.Default.GetBytes(MainWindow.NameNPC);
        Array.Copy(NameBytes, 0, fileBytes, 0, 30);

        TwoBytes = BitConverter.GetBytes(MainWindow.HP);
        fileBytes[0x92] = TwoBytes[0];
        fileBytes[0x93] = TwoBytes[1];

        fileBytes[0xDF] = (byte)MainWindow.Island;

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
        fileBytes[0x10A] = (byte)MainWindow.Dialogue;
        fileBytes[0x10B] = (byte)MainWindow.Voice;
        fileBytes[0x10F] = (byte)MainWindow.Job;
        fileBytes[0x113] = (byte)MainWindow.Home;
        fileBytes[0x144] = (byte)MainWindow.Place;

        File.WriteAllBytes(filename, fileBytes);
    }
}