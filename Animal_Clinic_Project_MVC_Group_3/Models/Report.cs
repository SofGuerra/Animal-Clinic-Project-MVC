using System;
using System.Collections.Generic;

namespace Animal_Clinic_Project_MVC_Group_3.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int? Month { get; set; }

    public int? Year { get; set; }

    public int? AppointmentsMade { get; set; }

    public int? AppointmentsCancelled { get; set; }
}
