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
    }
}
