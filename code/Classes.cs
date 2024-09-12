
public class Job
{
    public string JobName { get; set; }
    public string JobDescription { get; set; }  
    public ushort JobId { get; set; }
}

public class Island
{
    public string IslandName { get; set; }
    public string IslandDescription { get; set; } = "N/A";
    public ushort IslandId { get; set; }
    public bool IslandValid { get; set; } 
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
public class BodyModelClass
{
    public ushort ID { get; set; }
    public ushort ImageID { get; set; } = 0;
    public string BodyName { get; set; }
    public string BodyImage => $"/images/body/{ImageID:000}.png";
}
public class FaceModelClass
{
    public ushort ID { get; set; }
    public ushort ImageID { get; set; } = 0;
    public string FaceName { get; set; }
    public string FaceImage => $"/images/face/{ImageID:000}.png";
}
public class HairModelClass
{
    public ushort ID { get; set; }
    public ushort ImageID { get; set; } = 0;
    public string HairName { get; set; }
    public string HairImage => $"/images/hair/{ImageID:000}.png";
}

public class Equipment
{
    public ushort ID { get; set; }
    public ushort ModelID { get; set; }
    public ushort ImageID { get; set; }
    public string Image => $"/images/hair/{ImageID:000}.png";
    public string Name { get; set; }
}
