using DQB2NPCViewer.code;
using DQB2NPCViewer.control;
using HelixToolkit.Wpf;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace DQB2NPCViewer.SaveData
{
    public class ModelDisplayNPC : HelixViewportModel
    {

        public Model3DGroup GetFullModel()
        {
            return FullModel;
        }
        public override void UpdateAll(object NPC, ushort ClothColourID)
        {
            var importer = new ModelImporter();

            NPCData NPCdata = (NPCData)NPC;

            var face = ListText.FaceList.FirstOrDefault(x => x.ID == NPCdata.faceModelDisplay);
            faceModelDName.Value = NPCdata.faceModelDisplay + " - " + (face != null ? face.ModelClassV.ModelName : "?");
            var hair = ListText.HairList.FirstOrDefault(x => x.ID == NPCdata.hairModelDisplay);
            hairModelDName.Value = NPCdata.hairModelDisplay + " - " + (hair != null ? hair.ModelClassV.ModelName : "?");
            var body = ListText.BodyList.FirstOrDefault(x => x.ID == NPCdata.bodyModelDisplay);
            bodyModelDName.Value = NPCdata.bodyModelDisplay + " - " + (body != null ? body.ModelClassV.ModelName : "?");

            //Get the ID of the parts
            var HairID = NPCdata.hairModelDisplay;
            var FaceID = NPCdata.faceModelDisplay;
            var BodyID = NPCdata.bodyModelDisplay;
            //Get the colours 
            ClothImage = ListText.getColorDyeVal(ClothColourID);
            EyeImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(NPCdata.eyeColour).color);
            HairImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(NPCdata.hairColour).color);
            SkinImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(NPCdata.skinColour).color);
            UpdateColourView(NPCdata.skinColour);
            LoadBaseModel(HairID, FaceID, BodyID, NPCdata.skinColour, NPCdata.eyeColour, NPCdata.hairColour, ClothColourID);

        }
    }
}
