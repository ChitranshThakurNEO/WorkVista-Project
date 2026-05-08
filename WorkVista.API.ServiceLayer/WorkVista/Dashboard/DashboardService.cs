using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
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

        public ManagerDashboardGridResponseModel GetGridData(string managerEmployeeId, DateTime fromDate, DateTime toDate, string teamLeadEmployeeId, string geo, string searchText, out string errorMessage)
        {
            return _repository.GetGridData(
                managerEmployeeId,
                fromDate,
                toDate,
                teamLeadEmployeeId,
                geo,
                searchText,
                out errorMessage);
        }

        public EmployeeDayDetailsResponseModel GetEmployeeDayDetails(int employeeId, DateTime loggedDate, out string errorMessage)
        {
            return _repository.GetEmployeeDayDetails(
                employeeId,
                loggedDate,
                out errorMessage);
        }
    }
}
