using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
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

        public ManagerDashboardGridResponseModel GetGridData(string managerEmployeeId, DateTime fromDate, DateTime toDate, string teamLeadEmployeeId, string geo, string searchText,out string errorMessage)
        {
            errorMessage = string.Empty;

            var inParams = new Dictionary<string, object>
            {
                { "ManagerEmployeeId", managerEmployeeId },
                { "FromDate", fromDate },
                { "ToDate", toDate },
                { "TeamLeadEmployeeId", string.IsNullOrWhiteSpace(teamLeadEmployeeId) ? (object)DBNull.Value : teamLeadEmployeeId },
                { "Geo", string.IsNullOrWhiteSpace(geo) ? (object)DBNull.Value : geo },
                { "SearchText", string.IsNullOrWhiteSpace(searchText) ? (object)DBNull.Value : searchText }
            };

            var ds = _sqlDbUtilities.ExectuteStoredProcedureForMultipleTables(
                WorkVistaStoreProcedures.ManagerDashboard_GetGridData,
                inParams,
                out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage) || ds == null || ds.Tables.Count < 2)
            {
                return new ManagerDashboardGridResponseModel();
            }

            var employeeRawData = ds.Tables[0].ToList<ManagerDashboardGridRawResponseModel>();
            var teamAverageRawData = ds.Tables[1].ToList<TeamAverageRawResponseModel>();

            var employees = employeeRawData
                .GroupBy(x => new
                {
                    x.EmployeeId,
                    x.EmployeeName,
                    x.DESIGNATION,
                    x.GEO,
                    x.TotalMinutes,
                    x.AvgMinutesPerDay
                })
                .Select(group => new ManagerDashboardEmployeeGridResponseModel
                {
                    EmployeeId = group.Key.EmployeeId,
                    EmployeeName = group.Key.EmployeeName,
                    Designation = group.Key.DESIGNATION,
                    Geo = group.Key.GEO,
                    Total = FormatDateExtensions.MinutesToHHMM(group.Key.TotalMinutes),
                    AveragePerDay = FormatDateExtensions.MinutesToHHMM(group.Key.AvgMinutesPerDay),

                    Dates = group.ToDictionary(
                        x => x.LoggedDate.ToString(DashboardConstants.DateFormats.GridDateFormat),
                        x => x.DisplayValue
                    )
                })
                .ToList();

            var teamAverage = teamAverageRawData.ToDictionary(
                x => x.LoggedDate.ToString(DashboardConstants.DateFormats.GridDateFormat),
                x => x.TeamAverageDisplay
            );

            return new ManagerDashboardGridResponseModel
            {
                Employees = employees,
                TeamAverage = teamAverage
            };
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
