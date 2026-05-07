using WorkVista.API.ModelLayer.WorkVista.Dashboard;

namespace WorkVista.API.ServiceLayer.WorkVista.Dashboard
{
    public interface IDashboardService
    {
        ManagerDashboardSummaryResponseModel GetSummary(
        string managerEmployeeId,
        DateTime fromDate,
        DateTime toDate,
        out string errorMessage);

        List<ManagerDashboardEmployeeGridResponseModel> GetGridData(
        string managerEmployeeId,
        DateTime fromDate,
        DateTime toDate,
        string teamLeadEmployeeId,
        string geo,
        string searchText,
        out string errorMessage);
    }
}
