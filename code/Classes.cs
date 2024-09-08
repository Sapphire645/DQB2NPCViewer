﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
    public ushort type { get; set; }
    public ushort hair { get; set; }
    public ushort face { get; set; }
    public ushort body { get; set; }
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
