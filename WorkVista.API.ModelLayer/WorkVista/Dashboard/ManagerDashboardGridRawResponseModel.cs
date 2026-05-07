namespace WorkVista.API.ModelLayer.WorkVista.Dashboard
{
    public class ManagerDashboardGridRawResponseModel
    {
        public int MasterEmployeeId { get; set; }
        public string FullName { get; set; }
        public string DESIGNATION { get; set; }
        public string GEO { get; set; }
        public DateTime LoggedDate { get; set; }
        public decimal TOT_ACTIVEHOURS { get; set; }
        public string AttendanceStatus { get; set; }
        public string DayType { get; set; }
    }
}
