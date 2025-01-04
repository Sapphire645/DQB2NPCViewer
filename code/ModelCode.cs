using HelixToolkit.Wpf;
using SharpGLTF.Schema2;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;


public static class DQB2ModelRendering
{
    public static System.Windows.Media.Color EyeImage { get; set; }
    public static System.Windows.Media.Color SkinImage { get; set; }
    public static System.Windows.Media.Color HairImage { get; set; }
    public static System.Windows.Media.Color ClothImage { get; set; }
    private static Model3DGroup FaceModel { get; set; }
    private static Model3DGroup HairModel { get; set; }
    private static Model3DGroup BodyModel { get; set; }
    private static Model3DGroup Accesory1Model { get; set; }
    private static Model3DGroup Accesory2Model { get; set; }
    private static Model3DGroup Accesory3Model { get; set; }
    private static Model3DGroup AccesoryExtraModel { get; set; }
    private static Transform3DGroup transformGroup { get; set; }
    private static Transform3DGroup transformGroupAccesory { get; set; }

    private static bool CurrentError = true;

    public static void ModelCodeC()
    {
        EyeImage = Colors.White;
        SkinImage = Colors.White;
        HairImage = Colors.White;
        ClothImage = Colors.White;
    }
    /// <summary>
    /// ALL COLOR TRANSFORMATIONS I HAVE NOT CODED MYSELF.
    /// THEY ARE TEMPORARY.
    /// COLOR CORRECTIONS WILL BE DONE ONCE THE EDITOR IS FUNCTIONAL
    /// 
    /// THIS IS PLACEHOLDER CODE
    /// </summary>
    public static BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
    {
        // Create a MemoryStream to hold the image data
        using (MemoryStream memoryStream = new MemoryStream())
        {
            // Encode the BitmapSource to PNG format
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            // Create a BitmapImage from the MemoryStream
            memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position to the beginning
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = memoryStream;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Load image into memory
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
    public static BitmapSource CreateSolidColorBitmap(int width, int height, System.Windows.Media.Color color)
    {
        // Create a DrawingVisual to hold the drawing
        DrawingVisual drawingVisual = new DrawingVisual();

        // Create a DrawingContext to draw on the DrawingVisual
        using (DrawingContext drawingContext = drawingVisual.RenderOpen())
        {
            // Draw a solid color rectangle
            drawingContext.DrawRectangle(new SolidColorBrush(color), null, new Rect(0, 0, width, height));
        }

        // Create a RenderTargetBitmap to render the DrawingVisual to a BitmapSource
        RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
        renderTargetBitmap.Render(drawingVisual);

        return renderTargetBitmap;
    }
    public static BitmapSource MultiplyImages(BitmapSource baseImage, BitmapSource overlayImage)
    {
        int width = baseImage.PixelWidth;
        int height = baseImage.PixelHeight;

        // Convert images to pixel data
        var basePixels = new byte[width * height * 4];
        var overlayPixels = new byte[width * height * 4];
        baseImage.CopyPixels(basePixels, width * 4, 0);
        overlayImage.CopyPixels(overlayPixels, width * 4, 0);

        // Apply blending mode (Overlay)
        for (int i = 3; i < basePixels.Length; i += 4)
        {
            basePixels[i] = 255; // Set alpha to fully opaque
        }

        // Apply Multiply blending mode
        for (int i = 0; i < basePixels.Length; i += 4)
        {
            byte baseA = basePixels[i + 3];
            byte baseR = basePixels[i];
            byte baseG = basePixels[i + 1];
            byte baseB = basePixels[i + 2];

            byte overlayR = overlayPixels[i];
            byte overlayG = overlayPixels[i + 1];
            byte overlayB = overlayPixels[i + 2];

            // Normalize color components
            float baseRNorm = baseR / 255f;
            float baseGNorm = baseG / 255f;
            float baseBNorm = baseB / 255f;

            float overlayRNorm = overlayR / 255f;
            float overlayGNorm = overlayG / 255f;
            float overlayBNorm = overlayB / 255f;

            // Apply Multiply blending mode
            float resultR = baseRNorm * overlayRNorm;
            float resultG = baseGNorm * overlayGNorm;
            float resultB = baseBNorm * overlayBNorm;

            // Convert back to byte
            basePixels[i] = (byte)(resultR * 255);
            basePixels[i + 1] = (byte)(resultG * 255);
            basePixels[i + 2] = (byte)(resultB * 255);
        }

        // Create a new BitmapSource with the blended pixels
        return BitmapSource.Create(width, height, baseImage.DpiX, baseImage.DpiY, PixelFormats.Bgra32, null, basePixels, width * 4);
    }
    public static BitmapSource MergeImages(BitmapSource baseImage, BitmapSource overlayImage, ushort x = 0, ushort y = 0)
    {
        var visual = new DrawingVisual();
        using (var context = visual.RenderOpen())
        {
            context.DrawImage(baseImage, new Rect(0, 0, baseImage.PixelWidth, baseImage.PixelHeight));
            context.DrawImage(overlayImage, new Rect(x, y, overlayImage.PixelWidth, overlayImage.PixelHeight));
        }

        var bitmap = new RenderTargetBitmap(
            baseImage.PixelWidth,
            baseImage.PixelHeight,
            baseImage.DpiX,
            baseImage.DpiY,
            PixelFormats.Pbgra32);

        bitmap.Render(visual);

        return bitmap;
    }
    public static BitmapSource ApplyClippingMask(BitmapSource baseImage, BitmapSource maskImage)
    {
        int width = baseImage.PixelWidth;
        int height = baseImage.PixelHeight;

        // Convert images to pixel data
        var basePixels = new byte[width * height * 4];
        var maskPixels = new byte[width * height * 4];
        baseImage.CopyPixels(basePixels, width * 4, 0);
        maskImage.CopyPixels(maskPixels, width * 4, 0);

        // Apply the alpha from the mask image to the base image
        for (int i = 0; i < basePixels.Length; i += 4)
        {
            // Base image RGB components remain unchanged
            byte baseR = basePixels[i];
            byte baseG = basePixels[i + 1];
            byte baseB = basePixels[i + 2];

            // Get the alpha value from the mask image (using its R, G, or B channel)
            // Assuming the mask image has grayscale values for the mask, any of the R, G, or B channels will work
            byte maskAlpha = maskPixels[i];  // Or maskPixels[i + 1] or maskPixels[i + 2], since it's grayscale

            // Set the base image's alpha to the mask alpha
            basePixels[i + 3] = maskAlpha;  // The alpha channel of the base image is set to the mask's grayscale value
        }

        // Create a new BitmapSource with the modified pixels
        return BitmapSource.Create(width, height, baseImage.DpiX, baseImage.DpiY, PixelFormats.Bgra32, null, basePixels, width * 4);
    }

    private static System.Windows.Media.Media3D.DiffuseMaterial LoadTextureBasic(String Texture_Path)
    {
        BitmapImage texture = new BitmapImage(new Uri(Texture_Path));

        int width = texture.PixelWidth;
        int height = texture.PixelHeight;

        var baseImage = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, Colors.White));
        BitmapSource mergedImage = MultiplyImages(baseImage, texture); //Not transparent

        ImageBrush imageBrush = new ImageBrush(mergedImage)
        {
            Opacity = 1.0,
            ViewportUnits = BrushMappingMode.Absolute,
            TileMode = TileMode.Tile
        };

        var material = new System.Windows.Media.Media3D.DiffuseMaterial(imageBrush);

        material.Brush.Opacity = 1.0;
        return material;
    }
    private static System.Windows.Media.Media3D.DiffuseMaterial LoadTexture(System.Windows.Media.Color ColorB, String Texture_Path, String TextureMask_Path, String TextureClotheMask_Path, ushort Case, bool cloth)
    {
        BitmapImage texture, textureMask, textureCloth;
        try
        {
            texture = new BitmapImage(new Uri(Texture_Path));
        }
        catch
        {
            texture = BitmapSourceToBitmapImage(CreateSolidColorBitmap(50, 50, Colors.White));
        }
        int width = texture.PixelWidth;
        int height = texture.PixelHeight;

        try //FOR AROUND EYE AND CLOTHES COLOUR
        {
            textureMask = new BitmapImage(new Uri(TextureMask_Path));
        }
        catch
        {
            textureMask = BitmapSourceToBitmapImage(CreateSolidColorBitmap(1, 1, Colors.White));
        }
        try //FOR EYEBROWS AND CLOTHES NOT SKIN COLOUR 
        {
            textureCloth = new BitmapImage(new Uri(TextureClotheMask_Path));
        }
        catch
        {
            if (cloth)
            {
                textureCloth = new BitmapImage(new Uri("pack://application:,,,/textures/face/eBase.png"));
            }
            else
            {
                if (Case == 0)
                {
                    textureCloth = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, Colors.Transparent));
                }
                else{
                    textureCloth = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, Colors.White));
                }
                
            };
        }
        var baseImage = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, ColorB));
        BitmapSource mergedImage;
        switch (Case)
        {
            case 0: //Hair
                var baseImage2 = MergeImages(baseImage, textureCloth,0,0); //Color + Normal hair
                var ClothHairColourImage = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, ClothImage)); //Cloth Colour
                var ClothColourClip2 = ApplyClippingMask(ClothHairColourImage, textureMask); //Clip to dye cloth mask

                baseImage2 = MergeImages(baseImage2, ClothColourClip2,0,0);
                mergedImage = MultiplyImages(baseImage2, texture);
                break;
            case 1: //Face
                var EyeImageIm = BitmapSourceToBitmapImage(CreateSolidColorBitmap(128, 128, EyeImage)); //eye colour
                var mergedImageColor = MergeImages(baseImage, EyeImageIm, 256, 256); //add Eye colour to skin colour
                EyeImageIm = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, HairImage)); //create hair colour
                var EyebrowImage = ApplyClippingMask(EyeImageIm, textureCloth); //Clip to eyebrow mask
                mergedImageColor = MergeImages(mergedImageColor, EyebrowImage, 0, 0); //All colours
                mergedImage = MultiplyImages(texture, mergedImageColor); //Multiply
                mergedImage = MergeImages(mergedImage, textureMask, 256, 256); //Set eye overlay
                break;
            default: //Body
                var ClothColourImage = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, ClothImage)); //Cloth Colour
                mergedImage = MergeImages(baseImage, textureCloth, 0, 0);

                var ClothColourClip = ApplyClippingMask(ClothColourImage, textureMask); //Clip to dye cloth mask
                mergedImage = MergeImages(mergedImage, ClothColourClip, 0, 0); //SET COLOUR CLOTH
                mergedImage = MultiplyImages(mergedImage, texture);

                break;
        }

        ImageBrush imageBrush = new ImageBrush(mergedImage)
        {
            Opacity = 1.0,
            ViewportUnits = BrushMappingMode.Absolute,
            TileMode = TileMode.Tile
        };
        var material = new System.Windows.Media.Media3D.DiffuseMaterial(imageBrush);

        material.Brush.Opacity = 1.0;
        return material;
    }
    private static void LoadGlbFromResources(string Model, string Type)
    {
        var model = ModelRoot.Load(Model);

        if (File.Exists("models/" + Type))
        { File.Delete("models/" + Type); }
        try
        { model.SaveAsWavefront("models/"+Type); }
        catch{}
    }
    private static Model3DGroup LoadModel(System.Windows.Media.Media3D.DiffuseMaterial material, String Model_Path, String Type)
    {
        try
        {
            var objReader = new HelixToolkit.Wpf.ObjReader();
            LoadGlbFromResources(Model_Path, Type);
            var model = objReader.Read("models/"+Type);

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
            model.Transform = transformGroup;
            return model;
        }
        catch
        {
            CurrentError = true;
            return null;
        }
    }
    private static Model3DGroup LoadModelAccesory(System.Windows.Media.Media3D.DiffuseMaterial material, String Model_Path, String Type)
    {
        try
        {
            var objReader = new HelixToolkit.Wpf.ObjReader();
            LoadGlbFromResources(Model_Path, Type);
            var model = objReader.Read(Type);

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
    public static void Rotate()
    {
        double angleX = 90;
        double angleY = 0;
        double angleZ = 90;

        var rotationX = new AxisAngleRotation3D(new Vector3D(1, 0, 0), angleX);
        var rotationY = new AxisAngleRotation3D(new Vector3D(0, 1, 0), angleY);
        var rotationZ = new AxisAngleRotation3D(new Vector3D(0, 0, 1), angleZ);

        transformGroup = new Transform3DGroup();
        transformGroup.Children.Add(new RotateTransform3D(rotationX));
        transformGroup.Children.Add(new RotateTransform3D(rotationY));
        transformGroup.Children.Add(new RotateTransform3D(rotationZ));
    }
    public static void RotateAccesory()
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

        transformGroupAccesory.Children.Add(new TranslateTransform3D(0,0,1.125));
    }
    public static Model3DGroup GroupModels(ushort face, ushort hair, ushort body, bool Face, bool Hair, bool Body)
    {
        var importer = new ModelImporter();
        var modelGroup = new Model3DGroup();
        System.Windows.Media.Media3D.DiffuseMaterial material;
        if (CurrentError)
        {
            CurrentError = false;
            Face = true;
            Hair = true;
            Body = true;
        }
        try
        {
            if (body < 200)
            {
                // Load and add the first model
                if (Body == true)
                {
                    material = LoadTexture(SkinImage, "pack://application:,,,/textures/body/" + body.ToString("D3") + ".dds", "pack://application:,,,/textures/body/m" + body.ToString("D3") + ".png", "pack://application:,,,/textures/body/c" + body.ToString("D3") + ".png", 2, false);
                    BodyModel = LoadModel(material, "models/body/" + body.ToString("D3") + ".glb", "MeshBody.obj");
                }
                if (BodyModel != null)
                {
                    modelGroup.Children.Add(BodyModel);
                }
                // Load and add the second model
                if (Hair == true)
                {
                    material = LoadTexture(HairImage, "pack://application:,,,/textures/hair/" + hair.ToString("D3") + ".dds", "pack://application:,,,/textures/hair/m" + hair.ToString("D3") + ".png", "pack://application:,,,/textures/hair/c" + hair.ToString("D3") + ".png", 0, false);
                    HairModel = LoadModel(material, "models/hair/" + hair.ToString("D3") + ".glb", "MeshHair.obj");
                }
                if (HairModel != null)
                {
                    modelGroup.Children.Add(HairModel);
                }

                // Load and add the third model
                if (Face == true)
                {
                    material = LoadTexture(SkinImage, "pack://application:,,,/textures/face/" + face.ToString("D3") + ".dds", "pack://application:,,,/textures/face/m" + face.ToString("D3") + ".dds", "pack://application:,,,/textures/face/e" + face.ToString("D3") + ".png", 1, true);
                    FaceModel = LoadModel(material, "models/face/" + face.ToString("D3") + ".glb", "MeshFace.obj");
                }
                if (FaceModel != null)
                {
                    modelGroup.Children.Add(FaceModel);
                }
                //if (BodyModel == null || FaceModel == null || HairModel == null)
                //    return LoadModel(new System.Windows.Media.Media3D.DiffuseMaterial(), "/models/Unknown.glb", "MeshUnk.obj");
            }
            else
            {
                // Load and add the first model
                if (Body == true)
                {
                    material = LoadTexture(SkinImage, "pack://application:,,,/textures/monster/" + body.ToString("D3") + ".dds", "pack://application:,,,/textures/monster/m" + body.ToString("D3") + ".png", "pack://application:,,,/textures/monster/c" + body.ToString("D3") + ".png", 2, false);
                    BodyModel = LoadModel(material, "models/monster/" + body.ToString("D3") + ".glb", "MeshBody.obj");
                }
                if (BodyModel != null)
                {
                    modelGroup.Children.Add(BodyModel);
                }
            }
            if (CurrentError)
            {
                modelGroup = new Model3DGroup();
                var objReader = new HelixToolkit.Wpf.ObjReader();
                var model = objReader.Read("models/Question.obj");
                model.Transform = transformGroup;
                modelGroup.Children.Add(model);
            }
            return modelGroup;
        }
        catch
        {
            modelGroup = new Model3DGroup();
            var objReader = new HelixToolkit.Wpf.ObjReader();
            var model = objReader.Read("models/Question.obj");
            model.Transform = transformGroup;
            modelGroup.Children.Add(model);
            return modelGroup;
        }
    }
    public static Model3DGroup GroupModelsBuilder(ushort face, ushort hair, ushort body,
                                            ushort accesory1, ushort accesory2, ushort accesory3, ushort accesoryE,
                                                bool Face, bool Hair, bool Body,
                                                bool Baccesory1, bool Baccesory2, bool Baccesory3, bool BaccesoryE)
    {
        var importer = new ModelImporter();
        var modelGroup = new Model3DGroup();
        System.Windows.Media.Media3D.DiffuseMaterial material;
        if (CurrentError)
        {
            CurrentError = false;
            Face = true;
            Hair = true;
            Body = true;
        }
        try
        {
            // Load and add the first model
            if (Body == true)
            {
                material = LoadTexture(SkinImage, "pack://application:,,,/textures/body/" + body.ToString("D3") + ".dds", "pack://application:,,,/textures/body/m" + body.ToString("D3") + ".png", "pack://application:,,,/textures/body/c" + body.ToString("D3") + ".png", 2, false);
                BodyModel = LoadModel(material, "models/body/" + body.ToString("D3") + ".glb", "MeshBody.obj");
            }
            if (BodyModel != null)
            {
                modelGroup.Children.Add(BodyModel);
            }
            // Load and add the second model
            if (Hair == true)
            {
                material = LoadTexture(HairImage, "pack://application:,,,/textures/hair/" + hair.ToString("D3") + ".dds", "pack://application:,,,/textures/hair/m" + hair.ToString("D3") + ".png", "pack://application:,,,/textures/hair/c" + hair.ToString("D3") + ".png", 0, false);
                HairModel = LoadModel(material, "models/hair/" + hair.ToString("D3") + ".glb", "MeshHair.obj");
            }
            if (HairModel != null)
            {
                modelGroup.Children.Add(HairModel);
            }

            // Load and add the third model
            if (Face == true)
            {
                material = LoadTexture(SkinImage, "pack://application:,,,/textures/face/" + face.ToString("D3") + ".dds", "pack://application:,,,/textures/face/m" + face.ToString("D3") + ".dds", "pack://application:,,,/textures/face/e" + face.ToString("D3") + ".png", 1, true);
                FaceModel = LoadModel(material, "models/face/" + face.ToString("D3") + ".glb", "MeshFace.obj");
            }
            if (FaceModel != null)
            {
                modelGroup.Children.Add(FaceModel);
            }

            // Load and add the accesories
            if (Baccesory1 == true)
            {
                if (accesory1 > 0)
                {
                    material = LoadTextureBasic("pack://application:,,,/textures/builder/" + (accesory1 - 1).ToString("D2") + ".dds");
                    Accesory1Model = LoadModelAccesory(material, "models/builder/" + (accesory1 - 1).ToString("D2") + ".glb", "MeshA1.obj");
                }
                else
                    Accesory1Model = null;

            }
            if (Accesory1Model != null)
            {
                modelGroup.Children.Add(Accesory1Model);
            }
            if (Baccesory2 == true)
            {
                if (accesory2 > 0)
                {
                    material = LoadTextureBasic("pack://application:,,,/textures/builder/" + (accesory2-1).ToString("D2") + ".dds");
                    Accesory2Model = LoadModelAccesory(material, "models/builder/" + (accesory2 - 1).ToString("D2") + ".glb", "MeshA2.obj");
                }
                else
                    Accesory2Model = null;

            }
            if (Accesory2Model != null)
            {
                modelGroup.Children.Add(Accesory2Model);
            }
            if (Baccesory3 == true)
            {
                if (accesory3 > 0)
                {
                    material = LoadTextureBasic("pack://application:,,,/textures/builder/" + (accesory3 - 1).ToString("D2") + ".dds");
                    Accesory3Model = LoadModelAccesory(material, "models/builder/" + (accesory3 - 1).ToString("D2") + ".glb", "MeshA3.obj");
                }
                else
                    Accesory3Model = null;

            }
            if (Accesory3Model != null)
            {
                modelGroup.Children.Add(Accesory3Model);
            }
            if (BaccesoryE == true)
            {
                if (accesoryE > 0)
                {
                    material = LoadTextureBasic("pack://application:,,,/textures/builder/" + (accesoryE - 1).ToString("D2") + ".dds");
                    AccesoryExtraModel = LoadModelAccesory(material, "models/builder/" + (accesoryE - 1).ToString("D2") + ".glb", "MeshAE.obj");
                }
                else
                    AccesoryExtraModel = null;

            }
            if (AccesoryExtraModel != null)
            {
                modelGroup.Children.Add(AccesoryExtraModel);
            }

            if (CurrentError)
            {
                modelGroup = new Model3DGroup();
                var objReader = new HelixToolkit.Wpf.ObjReader();
                var model = objReader.Read("models/Question.obj");
                model.Transform = transformGroup;
                modelGroup.Children.Add(model);
            }

            return modelGroup;
        }
        catch
        {
            modelGroup = new Model3DGroup();
            var objReader = new HelixToolkit.Wpf.ObjReader();
            var model = objReader.Read("models/Question.obj");
            model.Transform = transformGroup;
            modelGroup.Children.Add(model);
            return modelGroup;
        }
    }

    //Colour blend code for the skin
    public static string Multiply(string hexColor2)
        {
            string hexColor1 = "#EFC294";

            // Convert the hex strings to RGB components
            (int r1, int g1, int b1) = HexToRGB(hexColor1);
            (int r2, int g2, int b2) = HexToRGB(hexColor2);

            // Apply the multiply filter
            int rResult = MultiplyColors(r1, r2);
            int gResult = MultiplyColors(g1, g2);
            int bResult = MultiplyColors(b1, b2);

            // Convert the result back to a hex color
            return RGBToHex(rResult, gResult, bResult);
        }

        private static  (int, int, int) HexToRGB(string hex)
        {
            // Remove the # if present
            hex = hex.TrimStart('#');

            // Convert hex to integer for R, G, B
            int r = Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = Convert.ToInt32(hex.Substring(4, 2), 16);

            return (r, g, b);
        }

    private static int MultiplyColors(int component1, int component2)
        {
            // Multiply the components and divide by 255 to normalize the result
            return (component1 * component2) / 255;
        }

    private static string RGBToHex(int r, int g, int b)
        {
            // Convert the RGB values back to hex
            return $"#{r:X2}{g:X2}{b:X2}";
        }
}