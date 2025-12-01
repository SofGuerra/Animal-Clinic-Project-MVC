using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class VPetHistory
{
    public int PetId { get; set; }

    public string PetName { get; set; } = null!;

    public string? Species { get; set; }

    public int? Age { get; set; }

    public string? OwnersName { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public string? Treatment { get; set; }

    public string VetName { get; set; } = null!;
}
