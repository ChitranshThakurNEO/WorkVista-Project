using WorkVista.API.ModelLayer.WorkVista.Dashboard;

namespace WorkVista.API.RepositoryLayer.WorkVista.Dashboard
{
    public interface IDashboardRepository
    {
        ManagerDashboardSummaryResponseModel GetSummary(
            string managerEmployeeId,
            DateTime fromDate,
            DateTime toDate,
            out string errorMessage);

        List<ManagerDashboardGridRawResponseModel> GetGridData(
        string managerEmployeeId,
        DateTime fromDate,
        DateTime toDate,
        string teamLeadEmployeeId,
        string geo,
        string searchText,
        out string errorMessage);

        EmployeeDayDetailsResponseModel GetEmployeeDayDetails(
        int employeeId,
        DateTime loggedDate,
        out string errorMessage);
        }
}
