using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DQB2NPCViewer;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

public static class DQB2ModelRendering
{
    public static Color EyeImage {  get; set; }
    public static Color SkinImage { get; set; }
    public static Color HairImage { get; set; }
    private static Model3DGroup FaceModel { get; set; }
    private static Model3DGroup HairModel { get; set; }
    private static Model3DGroup BodyModel { get; set; }
    private static Transform3DGroup transformGroup { get; set; }


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
    public static BitmapSource CreateSolidColorBitmap(int width, int height, Color color)
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
    public static BitmapSource OverlayImages(BitmapSource baseImage, BitmapSource overlayImage)
    {
        int width = baseImage.PixelWidth;
        int height = baseImage.PixelHeight;

        // Convert images to pixel data
        var basePixels = new byte[width * height * 4];
        var overlayPixels = new byte[width * height * 4];
        baseImage.CopyPixels(basePixels, width * 4, 0);
        overlayImage.CopyPixels(overlayPixels, width * 4, 0);

        // Ensure the base image is fully opaque
        for (int i = 3; i < basePixels.Length; i += 4)
        {
            basePixels[i] = 255; // Set alpha to fully opaque
        }

        // Apply blending mode (Overlay)
        for (int i = 0; i < basePixels.Length; i += 4)
        {
            byte baseA = basePixels[i + 3];
            byte baseR = basePixels[i];
            byte baseG = basePixels[i + 1];
            byte baseB = basePixels[i + 2];

            byte overlayR = overlayPixels[i];
            byte overlayG = overlayPixels[i + 1];
            byte overlayB = overlayPixels[i + 2];

            // Normalize base color components (since alpha is now 255)
            float baseRNorm = baseR / 255f;
            float baseGNorm = baseG / 255f;
            float baseBNorm = baseB / 255f;

            // Normalize overlay color components
            float overlayRNorm = overlayR / 255f;
            float overlayGNorm = overlayG / 255f;
            float overlayBNorm = overlayB / 255f;

            // Apply overlay blending mode
            float resultR = (baseRNorm < 0.5f) ? (2 * baseRNorm * overlayRNorm) : (1 - 2 * (1 - baseRNorm) * (1 - overlayRNorm));
            float resultG = (baseGNorm < 0.5f) ? (2 * baseGNorm * overlayGNorm) : (1 - 2 * (1 - baseGNorm) * (1 - overlayGNorm));
            float resultB = (baseBNorm < 0.5f) ? (2 * baseBNorm * overlayBNorm) : (1 - 2 * (1 - baseBNorm) * (1 - overlayBNorm));

            // Convert back to byte
            basePixels[i] = (byte)(resultR * 255);
            basePixels[i + 1] = (byte)(resultG * 255);
            basePixels[i + 2] = (byte)(resultB * 255);
        }

        // Create a new BitmapSource with the blended pixels
        return BitmapSource.Create(width, height, baseImage.DpiX, baseImage.DpiY, PixelFormats.Bgra32, null, basePixels, width * 4);
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
    public static BitmapSource MergeImages(BitmapSource baseImage, BitmapSource overlayImage,ushort x = 0, ushort y = 0)
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

    public static float Clamp(float value, float min, float max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
    public static BitmapSource LinearLightImages(BitmapSource baseImage, BitmapSource overlayImage)
    {
        int width = baseImage.PixelWidth;
        int height = baseImage.PixelHeight;

        // Convert images to pixel data
        var basePixels = new byte[width * height * 4];
        var overlayPixels = new byte[width * height * 4];
        baseImage.CopyPixels(basePixels, width * 4, 0);
        overlayImage.CopyPixels(overlayPixels, width * 4, 0);

        // Ensure the base image is fully opaque
        for (int i = 3; i < basePixels.Length; i += 4)
        {
            basePixels[i] = 255; // Set alpha to fully opaque
        }

        // Apply Linear Light blending mode
        for (int i = 0; i < basePixels.Length; i += 4)
        {
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

            // Calculate Linear Light blending
            float resultR, resultG, resultB;

            if (overlayRNorm > 0.5f)
            {
                resultR = baseRNorm + 2 * (overlayRNorm - 0.5f);
            }
            else
            {
                resultR = baseRNorm - 2 * (0.5f - overlayRNorm);
            }

            if (overlayGNorm > 0.5f)
            {
                resultG = baseGNorm + 2 * (overlayGNorm - 0.5f);
            }
            else
            {
                resultG = baseGNorm - 2 * (0.5f - overlayGNorm);
            }

            if (overlayBNorm > 0.5f)
            {
                resultB = baseBNorm + 2 * (overlayBNorm - 0.5f);
            }
            else
            {
                resultB = baseBNorm - 2 * (0.5f - overlayBNorm);
            }

            // Clamp the results to [0, 1]
            resultR = Clamp(resultR, 0f, 1f);
            resultG = Clamp(resultG, 0f, 1f);
            resultB = Clamp(resultB, 0f, 1f);

            // Convert back to byte
            basePixels[i] = (byte)(resultR * 255);
            basePixels[i + 1] = (byte)(resultG * 255);
            basePixels[i + 2] = (byte)(resultB * 255);
        }

        // Create a new BitmapSource with the blended pixels
        return BitmapSource.Create(width, height, baseImage.DpiX, baseImage.DpiY, PixelFormats.Bgra32, null, basePixels, width * 4);
    }
    private static DiffuseMaterial LoadTexture(Color ColorB, String Texture_Path, String TextureMask_Path, String TextureClotheMask_Path, ushort Case, Color ColorEye)
    {
        BitmapImage texture, textureMask, textureCloth;
        try
        {
            texture = new BitmapImage(new Uri(Texture_Path));
        }
        catch
        {
            texture = BitmapSourceToBitmapImage(CreateSolidColorBitmap(50, 50, ColorB));
        }
        try
        {
            textureMask = new BitmapImage(new Uri(TextureMask_Path));
        }
        catch
        {
            textureMask = BitmapSourceToBitmapImage(CreateSolidColorBitmap(50, 50, ColorB));
        }
        int width = texture.PixelWidth;
        int height = texture.PixelHeight;
        var baseImage = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height, ColorB));
        BitmapSource mergedImage;
        switch(Case){
            case 0: //Hair
                var white = BitmapSourceToBitmapImage(CreateSolidColorBitmap(width, height,Color.FromRgb(255,255,255)));
                var textureWhite = MergeImages(white, texture, 0, 0);
                mergedImage = MultiplyImages(baseImage, textureWhite);
                break;
            case 1: //Face
                var EyeImage = BitmapSourceToBitmapImage(CreateSolidColorBitmap(128, 128,ColorEye));
                var mergedImagePrev = MergeImages(baseImage, EyeImage,256,256);
                mergedImage = OverlayImages(texture, mergedImagePrev);
                //try
                //{
                //    var textureMask = new BitmapImage(new Uri(TextureMask_Path));
                //    mergedImage = MergeImages(mergedImage, textureMask, 256, 256);
                //}
                //catch
                //{
                //    width = texture.PixelWidth; //fix or smth
                //}
                break;
            default: //Body
                try
                {
                    textureCloth = new BitmapImage(new Uri(TextureClotheMask_Path));
                }
                catch
                {
                    textureCloth = BitmapSourceToBitmapImage(CreateSolidColorBitmap(50, 50, ColorB));
                }
                mergedImage = MergeImages(baseImage, textureCloth, 0, 0);
                mergedImage = MergeImages(baseImage, textureMask,0,0); //SET COLOUR CLOTH (LEAVE FOR NOW)
                mergedImage = OverlayImages(texture, mergedImage);
                break;
        }

        ImageBrush imageBrush = new ImageBrush(mergedImage)
        {
            Opacity = 1.0,
            ViewportUnits = BrushMappingMode.Absolute,
            TileMode = TileMode.Tile
        };
        var material = new DiffuseMaterial(imageBrush);

        material.Brush.Opacity = 1.0;
        return material;
    }

private static Model3DGroup LoadModel(DiffuseMaterial material, String Model_Path)
    {
        var importer = new ModelImporter();
        try
        {
            var objReader = new HelixToolkit.Wpf.ObjReader();
            var model = objReader.Read(Model_Path);
            
            if (model is Model3DGroup modelGroup)
            {
                foreach (var geometry in modelGroup.Children)
                {
                    if (geometry is GeometryModel3D geomModel)
                    {
                            geomModel.Material = material;
                    }
                }
            }
            model.Transform = transformGroup;
            return model;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Model does not exist! NPC will not appear.");
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
    public static Model3DGroup GroupModels(ushort face, ushort hair, ushort body,bool Face, bool Hair,bool Body)
    {
        var importer = new ModelImporter();
        var modelGroup = new Model3DGroup();
        DiffuseMaterial material;
        try
        {
            // Load and add the first model
            if (Body == true)
            {
                material = LoadTexture(SkinImage, "pack://application:,,,/textures/body/" + body.ToString("D3") + ".dds", "pack://application:,,,/textures/body/m" + body.ToString("D3") + ".dds", "pack://application:,,,/textures/body/c" + body.ToString("D3") + ".png", 2, SkinImage);
                BodyModel = LoadModel(material, "models/body/" + body.ToString("D3") + ".obj");
            }
            if (BodyModel != null)
            {
                modelGroup.Children.Add(BodyModel);
            }

            // Load and add the second model

            if (Hair == true)
            {
                material = LoadTexture(HairImage, "pack://application:,,,/textures/hair/" + hair.ToString("D3") + ".dds", "pack://application:,,,/textures/hair/m" + hair.ToString("D3") + ".dds", "pack://application:,,,/textures/body/c" + body.ToString("D3") + ".png", 0, HairImage);
                HairModel = LoadModel(material, "models/hair/" + hair.ToString("D3") + ".obj");
            }
            if (HairModel != null)
            {
                modelGroup.Children.Add(HairModel);
            }

            // Load and add the third model
            if (Face == true)
            {
                material = LoadTexture(SkinImage, "pack://application:,,,/textures/face/" + face.ToString("D3") + ".dds", "pack://application:,,,/textures/face/m" + face.ToString("D3") + ".dds", "NULL", 1, EyeImage);
                FaceModel = LoadModel(material, "models/face/" + face.ToString("D3") + ".obj");
            }
            if (FaceModel != null)
                {
                    modelGroup.Children.Add(FaceModel);
                }

            if (BodyModel == null || FaceModel == null || HairModel == null) 
                            return LoadModel(new DiffuseMaterial(), "models/Unknown.obj");
            return modelGroup;

        }
        catch (Exception ex)
        {
            return modelGroup;
        }
    }
}