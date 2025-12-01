using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class Veterinarian
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Speciality { get; set; }

    public string? Availability { get; set; }

    public string? Role { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<ClinicalHistory> ClinicalHistories { get; set; } = new List<ClinicalHistory>();
}
