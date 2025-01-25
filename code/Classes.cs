using System.Windows.Media;

public class Island
{
    public string Name { get; set; }
    public string Description { get; set; } = "N/A";
    public byte Id { get; set; }
    public bool Valid { get; set; }
}
public class Job
{
    public string Name { get; set; }
    public string Description { get; set; } = "N/A";
    public byte Id { get; set; }
    public bool Valid { get; set; }
    public byte Size { get; set; }
    public string StringSize { get { if (Size == 0) return "Normal"; if (Size == 1) return "Small"; else return "Big"; } }
}
public class Place
{
    public string Name { get; set; }
    public byte Id { get; set; }
}

public class Colour
{
    public ushort ID { get; set; }
    public string color { get; set; }
}
public class TypeSet
{
    public ushort typeID { get; set; }
    public ushort hairID { get; set; }
    public ushort faceID { get; set; }
    public ushort bodyID { get; set; }
    public bool Monster { get; set; }
    public ushort Tier { get; set; }
    public string name { get; set; }
    public string description { get; set; }
}
public class Accesory
{
    public ushort ItemID { get; set; }
    public ushort ModelHairID { get; set; }
    public ushort ModelAccesoryID { get; set; }
    public ushort ImageID { get; set; }
    public string Name { get; set; }

    public string Image => $"/images/hair/{ImageID:000}.png";

    public Brush PositionNotCoded => (ItemID == 992 || ItemID == 993) ? Brushes.Red : Brushes.Black;

    public Accesory(ushort itemID, ushort modelHairID, ushort modelAccesoryID, ushort imageID, string name)
    {
        ItemID = itemID;
        ModelHairID = modelHairID;
        ModelAccesoryID = modelAccesoryID;
        ImageID = imageID;
        Name = name;
    }
    public Accesory(ushort itemID, ushort modelAccesoryID, ushort imageID, string name)
    {
        ItemID = itemID;
        ModelAccesoryID = modelAccesoryID;
        ImageID = imageID;
        Name = name;
    }
}
public class ModelClass
{
    public ushort ID { get; set; }
    public ushort ImageID { get; set; } = 0;
    public string ModelName { get; set; } = "--";
    public string StringImage { get; set; } = "hair";
    public string ModelImage => $"/images/{StringImage}/{ImageID:000}.png";
}

public class ArmourSub
{
    public ushort ImageIDFem { get; set; }
    public string ImageFem => $"/images/body/{ImageIDFem:000}.png";
    public ushort ModelIDFemale { get; set; }
    public ushort ColourIDMale { get; set; }
    public ushort ColourIDFemale { get; set; }
}
public class Equipment
{
    public ushort ID { get; set; }
    public ushort ModelIDMale { get; set; }
    public ushort ImageID { get; set; }
    public string Image => $"/images/body/{ImageID:000}.png";
    public string Name { get; set; }
    public ArmourSub ArmourValues { get; set; }

    public bool Change => ImageID == ArmourValues.ImageIDFem;
}

public class Weapon
{
    public ushort ItemID { get; set; }
    public ushort ImageID { get; set; }
    public string Image => $"/images/weapon/{ImageID:000}.png";
    public string ImageTool => $"/images/resource/icon/{PowerValue:00}.png";
    public string Name { get; set; }

    public ushort PowerValue { get; set; }

}
