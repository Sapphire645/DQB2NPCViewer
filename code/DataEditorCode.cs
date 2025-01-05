using System;
using Ionic.Zlib;
using System.Collections.Generic;

namespace DQB2NPCViewer.code
{
    public static class DQB2DataEditor
    {
        public static string LoadedFile;

        private static byte[] Header = new byte[0x2A444];
        public static byte[] CMNDATfileBytes;
        public static readonly int StartOfData = 0x6ACC8;
        public static readonly int StartOfBuilder = 0x6A866;
        public static readonly int StartOfBuilderName = 0xCD;
        public static readonly int StartOfBuilderInventory = 0x55B959;
        public static readonly int StartOfBuilderFlag = 0x500;
        public static readonly int SizeOfChar = 0x260;
        public static readonly int CountStory = 1023;
        public static readonly int CountMisc = 238;

        public static NPCData LoadCMNDATOffset(ushort offset)
        {
            var fileBytes = new byte[SizeOfChar];
            Array.Copy(CMNDATfileBytes, ((offset - 1) * SizeOfChar) + StartOfData, fileBytes, 0, SizeOfChar);
            return new NPCData(fileBytes);
        }
        public static NPCDataMinimum SaveCMNDATOffset(NPCData NPCData)
        {
            Array.Copy(NPCData.byteData, 0, CMNDATfileBytes, ((NPCData.offset - 1) * SizeOfChar) + StartOfData, SizeOfChar);
            return new NPCDataMinimum(NPCData.offset, NPCData);
        }

        public static bool LoadFile(byte[] CmnDat) {
            Byte[] comp = new Byte[CmnDat.Length - Header.Length];
            Array.Copy(CmnDat, Header.Length, comp, 0, comp.Length);
            Array.Copy(CmnDat, Header, Header.Length);
            try
            {
                CMNDATfileBytes = ZlibStream.UncompressBuffer(comp);
            }
            catch
            {
                return false;
            }
            //Backup();
            return true;
        }
        public static bool SaveFile(string path, BuilderData Builder) {
            if (CMNDATfileBytes == null) return false;

            SaveCMNDATBuilder(Builder);

            var comp = ZlibStream.CompressBuffer(CMNDATfileBytes);
            Byte[] tmp = new Byte[Header.Length + comp.Length];
            Array.Copy(Header, tmp, Header.Length);
            Byte[] size = BitConverter.GetBytes(tmp.Length);
            Array.Copy(size, 0, tmp, 0x10, size.Length);
            Array.Copy(comp, 0, tmp, Header.Length, comp.Length);
            System.IO.File.WriteAllBytes(path, tmp);
            return true;
        }

        public static List<List<NPCDataMinimum>> LoadCMNDATStory()
        {
            return LoadCMNDAT(StartOfData, StartOfData + (CountStory * SizeOfChar),1);

        }
        public static List<List<NPCDataMinimum>> LoadCMNDAT(int start, int end, ushort offset)
        {
            var Human = new List<NPCDataMinimum>();
            var Animal = new List<NPCDataMinimum>();
            var Monster = new List<NPCDataMinimum>();
            var Null = new List<NPCDataMinimum>();
            for (int i = start; i < end; i += SizeOfChar)
            {
                var temp = new NPCDataMinimum(offset,LoadCMNDATOffset(offset));
                if(temp.charType == 0)
                {
                    Null.Add(temp);
                }
                else
                if (ListText.getTypeCharVal(temp.charType).Monster)
                {
                    Monster.Add(temp);
                }
                else
                {
                    Human.Add(temp);
                }
                offset++;
            }
            var list = new List<List<NPCDataMinimum>>();
            list.Add(Human);
            list.Add(Animal);
            list.Add(Monster);
            list.Add(Null);
            return list;
        }
        public static List<List<NPCDataMinimum>> LoadCMNDATGeneric()
        {
            return LoadCMNDAT(StartOfData + (CountStory * SizeOfChar), StartOfData + ((CountStory + CountMisc) * SizeOfChar),1024);
        }
        public static BuilderData LoadCMNDATBuilder()
        {
            var fileBytes = new byte[SizeOfChar];
            var fileBytesName = new byte[12];
            var fileBytesInventory = new byte[0x40];
            var fileBytesFlag = new byte[0x200];
            Array.Copy(CMNDATfileBytes, StartOfBuilder, fileBytes, 0, SizeOfChar);
            Array.Copy(Header, StartOfBuilderName, fileBytesName, 0, 12);
            Array.Copy(CMNDATfileBytes, StartOfBuilderInventory, fileBytesInventory, 0, 0x40);
            Array.Copy(CMNDATfileBytes, StartOfBuilderFlag, fileBytesFlag, 0, 0x200);
            return new BuilderData(fileBytes, fileBytesName, fileBytesInventory, fileBytesFlag);
        }
        public static void SaveCMNDATBuilder(BuilderData Builder)
        {
            Array.Copy( Builder.byteData, 0, CMNDATfileBytes, StartOfBuilder, SizeOfChar);
            Array.Copy(Builder.byteDataName, 0, Header, StartOfBuilderName, 12);
            Array.Copy(Builder.byteDataInventory, 0, CMNDATfileBytes, StartOfBuilderInventory, 0x40);
            Array.Copy(Builder.byteDataFlagBools, 0,CMNDATfileBytes, StartOfBuilderFlag, 0x200);
        }
    }
}