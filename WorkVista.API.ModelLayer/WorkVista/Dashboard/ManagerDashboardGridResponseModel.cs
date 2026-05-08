namespace WorkVista.API.ModelLayer.WorkVista.Dashboard
{
    public class ManagerDashboardGridResponseModel
    {
        public List<ManagerDashboardEmployeeGridResponseModel> Employees { get; set; }
        public Dictionary<string, string> TeamAverage { get; set; }
    }
}
