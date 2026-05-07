namespace WorkVista.API.ModelLayer.WorkVista.Dashboard
{
    public class ManagerDashboardEmployeeGridResponseModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Geo { get; set; }
        public Dictionary<string, string> Dates { get; set; }
        public string Total { get; set; }
        public string AveragePerDay { get; set; }
    }
}
