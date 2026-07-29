using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQB2NPCViewer.SaveData
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class CHARlock
    {
        public ushort ID { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public ushort HP { get; set; }
        public byte Job { get; set; }
        public ushort HairColor { get; set; }
        public ushort EyeColor { get; set; }
        public ushort SkinColor { get; set; }
        public Dictionary<byte, CHARState> Models { get; set; } = new();
        public byte MessageType { get; set; }
        public ushort Weapon { get; set; }
        public byte HomeIsland { get; set; }
        public byte RoomSize { get; set; }
        public byte RoomFanciness { get; set; }
        public byte RoomAmbience { get; set; }

        public byte SpeciesCategory { get {
                if (Models.Count == 0) return 0;
                if (Models[0].BodyModel >= 400 && Models[0].BodyModel < 500) return 2;
                if (Models[0].BodyModel >= 200 && Models[0].BodyModel < 500) return 1;
                return 0;
            } }
    }

    public class CHARState
    {
        public string Name { get; set; } = "Base";
        public ushort BodyModel { get; set; }
        public ushort HairModel { get; set; }
        public ushort FaceModel { get; set; }
        public ushort SwimBodyModel { get; set; }
        public ushort BathBodyModel { get; set; }
        public float Size { get; set; }
        public float WalkSpeed { get; set; }
        public float RunSpeed { get; set; }
        public byte Voice { get; set; }
    }
}
