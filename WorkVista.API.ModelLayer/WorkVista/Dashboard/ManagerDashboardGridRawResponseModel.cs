namespace WorkVista.API.ModelLayer.WorkVista.Dashboard
{
    public class ManagerDashboardGridRawResponseModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DESIGNATION { get; set; }
        public string GEO { get; set; }
        public DateTime LoggedDate { get; set; }
        public string DisplayValue { get; set; }
        public int TotalMinutes { get; set; }
        public int AvgMinutesPerDay { get; set; }
    }
}
