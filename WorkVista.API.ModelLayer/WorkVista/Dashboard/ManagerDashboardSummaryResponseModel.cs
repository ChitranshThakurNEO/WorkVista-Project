namespace WorkVista.API.ModelLayer.WorkVista.Dashboard
{
    public class ManagerDashboardSummaryResponseModel
    {
        public int TeamMembers { get; set; }
        public decimal AvgDailyActiveHours { get; set; }
        public decimal ProductivePercentage { get; set; }
        public int Below6HoursDays { get; set; }
        public int LeaveDays { get; set; }
    }
}
