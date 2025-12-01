using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class Pet
{
    public int PetId { get; set; }

    public string Name { get; set; } = null!;

    public string? Species { get; set; }

    public int? Age { get; set; }

    public int? OwnerId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<ClinicalHistory> ClinicalHistories { get; set; } = new List<ClinicalHistory>();

    public virtual Owner? Owner { get; set; }
}
