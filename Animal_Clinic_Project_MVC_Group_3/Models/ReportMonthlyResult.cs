using System;

namespace Animal_Clinic_Project_MVC_Group_3.Models
{
    public class ReportMonthlyResult
    {
        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
    }
}
