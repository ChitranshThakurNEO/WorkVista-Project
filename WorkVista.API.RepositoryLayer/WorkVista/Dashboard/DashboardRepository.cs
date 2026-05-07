using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.WorkVista.Dashboard;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;

        public DashboardRepository(IOptionsMonitor<AppSettingsModel> options)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
        }
        public ManagerDashboardSummaryResponseModel GetSummary(string managerEmployeeId, DateTime fromDate, DateTime toDate, out string errorMessage)
        {
            errorMessage = string.Empty;

            var inParams = new Dictionary<string, object>
            {
                { "ManagerEmployeeId", managerEmployeeId },
                { "FromDate", fromDate },
                { "ToDate", toDate }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.ManagerDashboard_GetSummary,
                inParams,
                out errorMessage
            );

            return dt.ToList<ManagerDashboardSummaryResponseModel>()
                     .FirstOrDefault();
        }

        public List<ManagerDashboardGridRawResponseModel> GetGridData(string managerEmployeeId, DateTime fromDate, DateTime toDate, string teamLeadEmployeeId, string geo, string searchText, out string errorMessage)
        {
            errorMessage = string.Empty;

            var inParams = new Dictionary<string, object>
            {
                { "ManagerEmployeeId", managerEmployeeId },
                { "FromDate", fromDate },
                { "ToDate", toDate },
                { "TeamLeadEmployeeId", string.IsNullOrWhiteSpace(teamLeadEmployeeId) ? DBNull.Value : teamLeadEmployeeId },
                { "Geo", string.IsNullOrWhiteSpace(geo) ? DBNull.Value : geo },
                { "SearchText", string.IsNullOrWhiteSpace(searchText) ? DBNull.Value : searchText }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.ManagerDashboard_GetGridData,
                inParams,
                out errorMessage
            );

            return dt.ToList<ManagerDashboardGridRawResponseModel>();
        }

        public EmployeeDayDetailsResponseModel GetEmployeeDayDetails(int employeeId, DateTime loggedDate, out string errorMessage)
        {
            errorMessage = string.Empty;

            var inParams = new Dictionary<string, object>
            {
                { "EmployeeId", employeeId },
                { "LoggedDate", loggedDate }
            };

            var ds = _sqlDbUtilities.ExectuteStoredProcedureForMultipleTables(
                WorkVistaStoreProcedures.WorkVista_ManagerDashboard_GetEmployeeDayDetails,
                inParams,
                out errorMessage);

            return new EmployeeDayDetailsResponseModel
            {
                Summary = ds.Tables[0]
                    .ToList<EmployeeDaySummaryResponseModel>()
                    .FirstOrDefault(),

                Applications = ds.Tables[1]
                    .ToList<EmployeeApplicationUsageResponseModel>()
            };
        }
    }
}
