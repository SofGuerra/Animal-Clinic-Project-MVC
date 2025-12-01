using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class ClinicalHistory
{
    public int HistoryId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public string? Treatment { get; set; }

    public int? PetId { get; set; }

    public int? VeterinarianId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Pet? Pet { get; set; }

    public virtual Veterinarian? Veterinarian { get; set; }
}
