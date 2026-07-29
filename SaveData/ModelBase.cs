using DQB2NPCViewer.code;
using HelixToolkit.Wpf;
using SharpGLTF.Schema2;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;


public abstract class HelixViewportModel
{
    public System.Windows.Media.Color EyeImage { get; set; }
    protected ushort CurrentEyeColourID = 0;
    public System.Windows.Media.Color SkinImage { get; set; }
    protected ushort CurrentSkinColourID = 0;
    public System.Windows.Media.Color HairImage { get; set; }
    protected ushort CurrentHairColourID = 0;
    public System.Windows.Media.Color ClothImage { get; set; }
    protected ushort CurrentClothColourID = 0;

    public ObservableProperty<Brush> EyeColour { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
    public ObservableProperty<Brush> SkinColour { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
    public ObservableProperty<Brush> SkinColourFilter { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
    public ObservableProperty<Brush> HairColour { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };
    public ObservableProperty<Brush> ClothColour { get; set; } = new ObservableProperty<Brush>() { Value = Brushes.White };

    public ObservableProperty<String> faceModelDName { get; set; } = new ObservableProperty<String>() { Value = String.Empty};
    public ObservableProperty<String> hairModelDName { get; set; } = new ObservableProperty<String>() { Value = String.Empty };
    public ObservableProperty<String> bodyModelDName { get; set; } = new ObservableProperty<String>() { Value = String.Empty };

    protected Model3DGroup FaceModel;
    protected ushort CurrentFaceID = 0;
    protected Model3DGroup HairModel;
    protected ushort CurrentHairID = 0;
    protected Model3DGroup BodyModel;
    protected ushort CurrentBodyID = 0;

    protected Transform3DGroup transformGroup;

    protected Model3DGroup FullModel;

    protected bool CurrentError = true;

    public HelixViewportModel()
    {
        EyeImage = Colors.White;
        SkinImage = Colors.White;
        HairImage = Colors.White;
        ClothImage = Colors.White;
        Rotate();
    }
    //------------------------------------------------------------------------------------------------

    public abstract void UpdateAll(object NPC, ushort ClothColourID);


    public virtual Model3DGroup GetFullModel()
    {
        return FullModel;
    }

    protected void UpdateColourView(ushort skinColour)
    {
        EyeColour.Value = new SolidColorBrush(EyeImage);
        HairColour.Value = new SolidColorBrush(HairImage);
        SkinColour.Value = new SolidColorBrush(SkinImage);
        ClothColour.Value = new SolidColorBrush(ClothImage);
        SkinColourFilter.Value = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Multiply(ListText.getColorVal(skinColour).color)));
    }

    protected void LoadBaseModel(ushort HairID, ushort FaceID, ushort BodyID,
        ushort SkinID, ushort EyeID, ushort HairColourID, ushort ClothID)
    {
        //Load the parts
        bool Face = true;
        bool Hair = true;
        bool Body = true;
        //If no error only load the parts that are changed.
        if (!CurrentError)
        {
            Face = (FaceID != CurrentFaceID);
            Hair = (HairID != CurrentHairID);
            Body = (BodyID != CurrentBodyID);
            //Check colours too
            if (!Face)
                Face = SkinID != CurrentSkinColourID || EyeID != CurrentEyeColourID 
                    || HairColourID != CurrentHairColourID;
            if(!Hair)
                Hair = HairColourID != CurrentHairColourID || ClothID != CurrentClothColourID;
            if (!Body)
                Body = ClothID != CurrentClothColourID || SkinID != CurrentSkinColourID;
            if (!Face && !Hair && !Body)
                return;
        }
        FullModel = new Model3DGroup();
        CurrentError = false;
        //update new
        CurrentFaceID = FaceID;
        CurrentHairID = HairID;
        CurrentBodyID = BodyID;
        CurrentSkinColourID = SkinID;
        CurrentEyeColourID = EyeID;
        CurrentHairColourID = HairColourID;
        CurrentClothColourID = ClothID;
        try
        {
            if (BodyID < 200)
            {
                if (Body)
                    BodyModel = LoadAll(SkinImage, BodyID, 2);
                if (Hair)
                    HairModel = LoadAll(HairImage, HairID, 0);
                if (Face)
                    FaceModel = LoadAll(SkinImage, FaceID, 1);
            }
            else
            {
                if (Body)
                    BodyModel = LoadAll(SkinImage, BodyID, 4);
            }
            if (CurrentError)
            {
                var objReader = new HelixToolkit.Wpf.ObjReader();
                var model = objReader.Read("models/Question.obj");
                model.Transform = transformGroup;
                FullModel.Children.Add(model);
            }
            else
            {
                FullModel.Children.Add(BodyModel);
                if (BodyID < 200)
                {
                    FullModel.Children.Add(FaceModel);
                    FullModel.Children.Add(HairModel);
                }
            }
        }
        catch
        {
            var objReader = new HelixToolkit.Wpf.ObjReader();
            var model = objReader.Read("models/Question.obj");
            model.Transform = transformGroup;
            FullModel.Children.Add(model);
        }
    }
    protected Model3DGroup LoadAll(Color Base, ushort ID, ushort Case)
    {
        string location = Case == 0 ? "hair" : Case == 1 ? "face" : Case == 4 ? "monster" : Case == 2 ? "body" : "builder";
        System.Windows.Media.Media3D.DiffuseMaterial material = LoadTexture(Base, 
            "pack://application:,,,/textures/"+location+"/" + ID.ToString("D3") + ".dds", 
            "pack://application:,,,/textures/"+location+"/m" + ID.ToString("D3") + (Case == 1 ? ".dds" : ".png"),
            "pack://application:,,,/textures/"+location+"/" + (Case == 1 ? "e" : "c") + ID.ToString("D3") + ".png",
            Case, Case == 1);
        string location2 = Case == 0 ? "hair" : Case == 1 ? "face" : "body";
        return LoadModel(material, "models/"+ location+  "/" + ID.ToString("D3") + ".glb", "Mesh"+ location2 + ".obj");
    }


    //------------------------------------------------------------------------------------------------
    /// <summary>
    /// ALL COLOR TRANSFORMATIONS I HAVE NOT CODED MYSELF.
    /// THEY ARE TEMPORARY.
    /// COLOR CORRECTIONS WILL BE DONE ONCE THE EDITOR IS FUNCTIONAL
    /// 
    /// THIS IS PLACEHOLDER CODE
    /// </summary>
    private BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
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
    private BitmapSource CreateSolidColorBitmap(int width, int height, System.Windows.Media.Color color)
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
    private BitmapSource MultiplyImages(BitmapSource baseImage, BitmapSource overlayImage)
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
    private BitmapSource MergeImages(BitmapSource baseImage, BitmapSource overlayImage, ushort x = 0, ushort y = 0)
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
    private BitmapSource ApplyClippingMask(BitmapSource baseImage, BitmapSource maskImage)
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

    protected System.Windows.Media.Media3D.DiffuseMaterial LoadTextureBasic(String Texture_Path)
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
    private System.Windows.Media.Media3D.DiffuseMaterial LoadTexture(System.Windows.Media.Color ColorB, String Texture_Path, String TextureMask_Path, String TextureClotheMask_Path, ushort Case, bool cloth)
    {
        BitmapImage texture, textureMask, textureCloth;
        Uri temp = new Uri(Texture_Path, UriKind.Absolute);
        try
        {
            texture = new BitmapImage(temp);
        }
        catch (System.IO.IOException ex)
        {
            texture = BitmapSourceToBitmapImage(CreateSolidColorBitmap(50, 50, Colors.White));
        }
        int width = texture.PixelWidth;
        int height = texture.PixelHeight;

        temp = new Uri(TextureMask_Path, UriKind.Absolute);
        try
        {
            textureMask = new BitmapImage(temp);
        }
        catch (System.IO.IOException ex)
        {
            textureMask = BitmapSourceToBitmapImage(CreateSolidColorBitmap(1, 1, Colors.White));
        }
        temp = new Uri(TextureClotheMask_Path, UriKind.Absolute);
        try
        {
            textureCloth = new BitmapImage(temp);
        }
        catch (System.IO.IOException ex)
        {
            if (cloth)
                textureCloth = new BitmapImage(new Uri("pack://application:,,,/textures/face/eBase.png"));
            else
                if (Case == 0)
                textureCloth = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, Colors.Transparent));
            else
                textureCloth = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, Colors.White));
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
    protected void LoadGlbFromResources(string Model, string Type)
    {
        var model = ModelRoot.Load(Model);

        if (File.Exists("models/" + Type))
        { File.Delete("models/" + Type); }
        try
        { model.SaveAsWavefront("models/"+Type); }
        catch{}
    }
    private Model3DGroup LoadModel(System.Windows.Media.Media3D.DiffuseMaterial material, String Model_Path, String Type)
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
    
    private void Rotate()
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

    public static (int, int, int) HexToRGB(string hex)
        {
            // Remove the # if present
            hex = hex.TrimStart('#');

            // Convert hex to integer for R, G, B
            int r = Convert.ToInt32(hex.Substring(0, 2), 16);
            int g = Convert.ToInt32(hex.Substring(2, 2), 16);
            int b = Convert.ToInt32(hex.Substring(4, 2), 16);

            return (r, g, b);
        }

    public static int MultiplyColors(int component1, int component2)
        {
            // Multiply the components and divide by 255 to normalize the result
            return (component1 * component2) / 255;
        }

    public static string RGBToHex(int r, int g, int b)
        {
            // Convert the RGB values back to hex
            return $"#{r:X2}{g:X2}{b:X2}";
        }
}