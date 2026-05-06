using WorkVista.API.ModelLayer.WorkVista.Dashboard;
using WorkVista.API.RepositoryLayer.WorkVista.Dashboard;

namespace WorkVista.API.ServiceLayer.WorkVista.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }
        public ManagerDashboardSummaryResponseModel GetSummary( string managerEmployeeId, DateTime fromDate, DateTime toDate, out string errorMessage)
        {
            return _repository.GetSummary(
                managerEmployeeId,
                fromDate,
                toDate,
                out errorMessage);
        }
    }
}
