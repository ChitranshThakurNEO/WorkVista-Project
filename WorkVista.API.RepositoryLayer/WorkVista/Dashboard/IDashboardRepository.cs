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
    }
}
