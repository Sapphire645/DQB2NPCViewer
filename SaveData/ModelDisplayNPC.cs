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

            //Get the ID of the parts
            var HairID = NPCdata.hairModel;
            var FaceID = NPCdata.faceModel;
            var BodyID = NPCdata.bodyModel;
            if (NPCdata.typeLock == true)
            {
                TypeSet TypeLockCurrent = null;
                var a = ListText.TypeLockList.FirstOrDefault(x => x.ID == NPCdata.charType);
                if (a != null)
                    TypeLockCurrent = a.TypeListing;
                if (TypeLockCurrent != null)
                {
                    if (TypeLockCurrent.faceID != 0)
                        FaceID = TypeLockCurrent.faceID;
                    if (TypeLockCurrent.bodyID != 0)
                        BodyID = TypeLockCurrent.bodyID;
                    if (TypeLockCurrent.hairID != 0)
                        HairID = TypeLockCurrent.hairID;
                }
            }
            if (NPCdata.hasRags == true)
            {
                if (NPCdata.sex == 1)
                    BodyID = 31;
                else
                    BodyID = 32;
            }
            else
            {
                if (NPCdata.armour != 0 && NPCdata.hasClothes == true)
                {
                    var ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == NPCdata.armour);
                    if (NPCdata.sex == 1)
                        BodyID = ArmourClass.Armour.ModelIDMale;
                    else
                        BodyID = ArmourClass.Armour.ArmourValues.ModelIDFemale;
                }

            }
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
