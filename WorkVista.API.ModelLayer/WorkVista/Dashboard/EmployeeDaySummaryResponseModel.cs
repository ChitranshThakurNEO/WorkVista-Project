namespace WorkVista.API.ModelLayer.WorkVista.Dashboard
{
    public class EmployeeDaySummaryResponseModel
    {
        public int EmployeeId { get; set; }

        public string FullName { get; set; }

        public DateTime LoggedDate { get; set; }

        public decimal TOT_LOGGEDHOURS { get; set; }

        public decimal TOT_ACTIVEHOURS { get; set; }

        public decimal TOT_TIMEONSYSTEM { get; set; }

        public decimal TOT_TIMEAWAYFROMSYSTEM { get; set; }

        public decimal TOT_IDLETIME { get; set; }

        public DateTime FIRSTLOGIN { get; set; }

        public DateTime LASTLOGOUT { get; set; }

        public string AttendanceStatus { get; set; }

        public decimal ProductivityPercentage { get; set; }
    }
}
