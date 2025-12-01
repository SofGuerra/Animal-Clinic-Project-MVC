using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class VetsAvailability
{
    public int UserId { get; set; }

    public string VetName { get; set; } = null!;

    public TimeSpan Slot { get; set; }
}
