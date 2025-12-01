using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public int? Duration { get; set; }

    public string? Status { get; set; }

    public int? PetId { get; set; }

    public int? VeterinarianId { get; set; }

    public virtual Pet? Pet { get; set; }

    public virtual Veterinarian? Veterinarian { get; set; }
}
