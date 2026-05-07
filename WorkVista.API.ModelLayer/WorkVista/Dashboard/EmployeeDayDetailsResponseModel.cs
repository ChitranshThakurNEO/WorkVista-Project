namespace WorkVista.API.ModelLayer.WorkVista.Dashboard
{
    public class EmployeeDayDetailsResponseModel
    {
        public EmployeeDaySummaryResponseModel Summary { get; set; }

        public List<EmployeeApplicationUsageResponseModel> Applications { get; set; }
    }
}
