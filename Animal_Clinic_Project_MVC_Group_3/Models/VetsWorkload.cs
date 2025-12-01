using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class VetsWorkload
{
    public int UserId { get; set; }

    public string VetName { get; set; } = null!;

    public int? TotalAppointments { get; set; }

    public int? Completed { get; set; }

    public int? Cancelled { get; set; }
}
