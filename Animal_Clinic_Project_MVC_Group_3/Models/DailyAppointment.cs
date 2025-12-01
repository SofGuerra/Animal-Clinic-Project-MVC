using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class DailyAppointment
{
    public int VeterinarianId { get; set; }

    public string VetsName { get; set; } = null!;

    public TimeSpan? SlotTime { get; set; }

    public int? AppointmentId { get; set; }

    public string? PetName { get; set; }

    public string? OwnersName { get; set; }

    public string? Status { get; set; }
}
