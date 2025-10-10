using DQB2NPCViewer.code;
using DQB2NPCViewer.control;
using HelixToolkit.Wpf;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace DQB2NPCViewer.SaveData
{
    public class ModelDisplayBuilder : HelixViewportModel
    {
        protected Model3DGroup Accesory1Model;
        protected ushort Accesory1ID = 0;
        protected Model3DGroup Accesory2Model;
        protected ushort Accesory2ID = 0;
        protected Model3DGroup Accesory3Model;
        protected ushort Accesory3ID = 0;
        protected Model3DGroup AccesoryExtraModel;
        protected ushort AccesoryEID = 0;

        protected Transform3DGroup transformGroupAccesory;

        public ModelDisplayBuilder() : base()
        {
            RotateAccesory();
        }
        public override void UpdateAll(object NPC, ushort ClothColourID)
        {
            var importer = new ModelImporter();

            BuilderData Builder = (BuilderData)NPC;

            RotateAccesory();
            var HairVisual = Builder.hairModelBase;
            var FaceVisual = Builder.faceModelBase;
            var BodyVisual = Builder.bodyModelBase;

            var Accesory1 = Builder.mirrorAccesory1;
            var Accesory2 = Builder.mirrorAccesory2;
            var Accesory3 = Builder.mirrorAccesory3;

            ushort AccesoryExtra = 0;
            if (Builder.mirrorClothes != 0 || Builder.armour != 0)
            {
                ComboBoxArmour ArmourClass;
                if (Builder.mirrorClothes == 0)
                    ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == Builder.armour);
                else
                    ArmourClass = ListText.ArmourList.FirstOrDefault(x => x.ID == Builder.mirrorClothes);
                if (Builder.sex == 1)
                    BodyVisual = ArmourClass.Armour.ModelIDMale;
                else
                    BodyVisual = ArmourClass.Armour.ArmourValues.ModelIDFemale;
            }
            if (Accesory1 != 0)
            {
                var AccClass = ListText.AccesoryList.FirstOrDefault(x => x.ItemID == Builder.mirrorAccesory1);
                Accesory1 = AccClass.ModelAccesoryID;
            }
            if (Accesory2 != 0)
            {
                var AccClass = ListText.AccesoryList.FirstOrDefault(x => x.ItemID == Builder.mirrorAccesory2);
                Accesory2 = AccClass.ModelAccesoryID;
            }
            if (Accesory3 != 0)
            {
                var AccClass = ListText.AccesoryList.FirstOrDefault(x => x.ItemID == Builder.mirrorAccesory3);
                Accesory3 = AccClass.ModelAccesoryID;
            }
            if (Builder.mirrorHair != 0)
            {
                var AccClass = ListText.HairBuilderList.FirstOrDefault(x => x.ItemID == Builder.mirrorHair);
                HairVisual = AccClass.ModelHairID;
                if (HairVisual == 0)
                {
                    AccesoryExtra = AccClass.ModelAccesoryID;
                    HairVisual = Builder.isMale ? (ushort)53 : (ushort)52;
                }
            }

            //Get the colours 
            ClothImage = ListText.getColorDyeVal(ClothColourID);
            EyeImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(Builder.eyeColour).color);
            HairImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(Builder.hairColour).color);
            SkinImage = (Color)ColorConverter.ConvertFromString(ListText.getColorVal(Builder.skinColour).color);
            UpdateColourView(Builder.skinColour);
            LoadBaseModel(HairVisual, FaceVisual, BodyVisual, 
                Builder.skinColour, Builder.eyeColour, Builder.hairColour, 
                ClothColourID);
            LoadBuilderAccesories(Accesory1, Accesory2, Accesory3, AccesoryExtra);

        }

        protected void LoadBuilderAccesories(ushort Accesory1, ushort Accesory2, ushort Accesory3, ushort AccesoryExtra)
        {
            //Load the parts
            bool a1 = true;
            bool a2 = true;
            bool a3 = true;
            bool ae = true;
            //If no error only load the parts that are changed.
            if (!CurrentError)
            {
                a1 = (Accesory1 != Accesory1ID);
                a2 = (Accesory2 != Accesory2ID);
                a3 = (Accesory3 != Accesory3ID);
                ae = (AccesoryExtra != AccesoryEID);
            }
            CurrentError = false;
            //update new
            Accesory1ID = Accesory1;
            Accesory2ID = Accesory2;
            Accesory3ID = Accesory3;
            AccesoryEID = AccesoryExtra;

            // Load and add the accesories
            if (a1)
            {
                if (Accesory1 > 0)
                {
                    Accesory1Model = LoadModelAccesory(
                        LoadTextureBasic("pack://application:,,,/textures/builder/" + (Accesory1 - 1).ToString("D2") + ".dds")
                        , "models/builder/" + (Accesory1 - 1).ToString("D2") + ".glb", "MeshA1.obj");
                }
                else
                    Accesory1Model = null;

            }
            if (Accesory1Model != null)
            {
                FullModel.Children.Add(Accesory1Model);
            }
            if (a2 == true)
            {
                if (Accesory2 > 0)
                {
                    Accesory2Model = LoadModelAccesory(
                        LoadTextureBasic("pack://application:,,,/textures/builder/" + (Accesory2 - 1).ToString("D2") + ".dds")
                        , "models/builder/" + (Accesory2 - 1).ToString("D2") + ".glb", "MeshA2.obj");
                }
                else
                    Accesory2Model = null;

            }
            if (Accesory2Model != null)
            {
                FullModel.Children.Add(Accesory2Model);
            }
            if (a3 == true)
            {
                if (Accesory3 > 0)
                {
                    Accesory3Model = LoadModelAccesory(
                        LoadTextureBasic("pack://application:,,,/textures/builder/" + (Accesory3 - 1).ToString("D2") + ".dds")
                        , "models/builder/" + (Accesory3 - 1).ToString("D2") + ".glb", "MeshA3.obj");
                }
                else
                    Accesory3Model = null;

            }
            if (Accesory3Model != null)
            {
                FullModel.Children.Add(Accesory3Model);
            }
            if (ae == true)
            {
                if (AccesoryExtra > 0)
                {
                    AccesoryExtraModel = LoadModelAccesory(
                        LoadTextureBasic("pack://application:,,,/textures/builder/" + (AccesoryExtra - 1).ToString("D2") + ".dds")
                        , "models/builder/" + (AccesoryExtra - 1).ToString("D2") + ".glb", "MeshAE.obj");
                }
                else
                    AccesoryExtraModel = null;

            }
            if (AccesoryExtraModel != null)
            {
                FullModel.Children.Add(AccesoryExtraModel);
            }

            if (CurrentError)
            {
                FullModel = new Model3DGroup();
                var objReader = new HelixToolkit.Wpf.ObjReader();
                var model = objReader.Read("models/Question.obj");
                model.Transform = transformGroup;
                FullModel.Children.Add(model);
            }
        }


        private void RotateAccesory()
        {
            double angleX = -90;
            double angleY = 0;
            double angleZ = 0;

            var rotationX = new AxisAngleRotation3D(new Vector3D(1, 0, 0), angleX);
            var rotationY = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angleY);
            var rotationZ = new AxisAngleRotation3D(new Vector3D(0, 0, 1), angleZ);

            transformGroupAccesory = new Transform3DGroup();
            transformGroupAccesory.Children.Add(new RotateTransform3D(rotationX));
            transformGroupAccesory.Children.Add(new RotateTransform3D(rotationY));
            transformGroupAccesory.Children.Add(new RotateTransform3D(rotationZ));

            transformGroupAccesory.Children.Add(new TranslateTransform3D(0, 0, 1.125));
        }

        private Model3DGroup LoadModelAccesory(System.Windows.Media.Media3D.DiffuseMaterial material, String Model_Path, String Type)
        {
            try
            {
                var objReader = new HelixToolkit.Wpf.ObjReader();
                LoadGlbFromResources(Model_Path, Type);
                var model = objReader.Read("models/" + Type);

                if (model is Model3DGroup modelGroup)
                {
                    foreach (var geometry in modelGroup.Children)
                    {
                        if (geometry is System.Windows.Media.Media3D.GeometryModel3D geomModel)
                        {
                            geomModel.Material = material;
                            geomModel.BackMaterial = material;
                        }
                    }
                }
                model.Transform = transformGroupAccesory;
                return model;
            }
            catch
            {
                CurrentError = true;
                return null;
            }
        }
    }
}
