
public class IslandJob
{
    public string IJName { get; set; }
    public string IJDescription { get; set; } = "N/A";
    public ushort IJId { get; set; }
    public bool IJValid { get; set; }
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
    public ushort jobID { get; set; }
    public string name { get; set; }
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

