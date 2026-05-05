using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityLogSummary;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterActivityLogSummary
{
    public class EmployeeActivityLogSummaryRepository : IEmployeeActivityLogSummaryRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.EmployeeActivityLogSummary;

        public EmployeeActivityLogSummaryRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        public List<EmployeeActivityLogSummaryResponseModel> GetAll(out string errorMessage)
        {
            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllEmployeeActivityLogSummary,
                new Dictionary<string, object>(),
                out errorMessage);

            return dt.ToList<EmployeeActivityLogSummaryResponseModel>();
        }

        public EmployeeActivityLogSummaryResponseModel GetById(long id, out string errorMessage)
        {
            var param = new Dictionary<string, object>
            {
                { "EmployeeActivityLogSummaryId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetEmployeeActivityLogSummaryById,
                param,
                out errorMessage);

            return dt.Rows.Count > 0
                ? dt.ToList<EmployeeActivityLogSummaryResponseModel>().FirstOrDefault()
                : null;
        }

        public List<EmployeeActivityLogSummaryResponseModel> GetByEmployeeDate(
            int employeeId, DateTime fromDate, DateTime toDate, out string errorMessage)
        {
            var param = new Dictionary<string, object>
            {
                { "EmployeeId", employeeId },
                { "FromDate", fromDate },
                { "ToDate", toDate }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetEmployeeActivityLogSummaryByEmployeeDate,
                param,
                out errorMessage);

            return dt.ToList<EmployeeActivityLogSummaryResponseModel>();
        }

        public long AddOrUpdate(EmployeeActivityLogSummaryRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = request.EmployeeId.HasValue && request.EmployeeId > 0
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;

            var param = new Dictionary<string, object>
            {
                { "EmployeeId", request.EmployeeId },
                { "LoggedDate", request.LoggedDate },

                { "TOT_LOGGEDHOURS", request.TOT_LOGGEDHOURS },
                { "TOT_ACTIVEHOURS", request.TOT_ACTIVEHOURS },
                { "TOT_TIMEONSYSTEM", request.TOT_TIMEONSYSTEM },
                { "TOT_TIMEAWAYFROMSYSTEM", request.TOT_TIMEAWAYFROMSYSTEM },
                { "TOT_IDLETIME", request.TOT_IDLETIME },

                { "FIRSTLOGIN", request.FIRSTLOGIN },
                { "LASTLOGOUT", request.LASTLOGOUT },

                { "SHIFTNAME", request.SHIFTNAME },
                { "NOOFDAYS", request.NOOFDAYS },
                { "DAYTYPE", request.DAYTYPE },
                { "AttendanceStatus", request.AttendanceStatus },

                { "IsValidated", request.IsValidated },
                { "CreatedBy", request.CreatedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_UpsertEmployeeActivityLogSummary,
                param,
                out errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                var errorAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = request.CreatedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = request.EmployeeId?.ToString(),
                    EntityJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(request),
                    Description = errorMessage
                };

                _masterAuditLogRepository.Add(errorAuditRequest, out _);
                return 0;
            }

            var identity = dt != null && dt.Rows.Count > 0
                ? Convert.ToInt32(dt.Rows[0]["Id"])
                : 0;

            if (identity > 0)
            {
                var successAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = request.CreatedBy,
                    Action = action,
                    EntityName = EntityName,
                    EntityId = identity.ToString(),
                    EntityJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(request),
                    Description = $"{EntityName} {action} successful"
                };

                _masterAuditLogRepository.Add(successAuditRequest, out _);
            }

            return identity;
        }

        public int Delete(DeleteRequestLong deleteRequest, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = AuditConstants.Actions.Delete;

            var param = new Dictionary<string, object>
            {
                { "EmployeeActivityLogSummaryId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteEmployeeActivityLogSummary,
                param,
                out errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                var errorAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = deleteRequest.DeletedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = deleteRequest.Id.ToString(),
                    EntityJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(deleteRequest),
                    Description = errorMessage
                };

                _masterAuditLogRepository.Add(errorAuditRequest, out _);
                return 0;
            }

            var identity = dt != null && dt.Rows.Count > 0
                ? Convert.ToInt32(dt.Rows[0]["Id"])
                : 0;

            if (identity > 0)
            {
                var successAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = deleteRequest.DeletedBy,
                    Action = action,
                    EntityName = EntityName,
                    EntityId = identity.ToString(),
                    EntityJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(deleteRequest),
                    Description = $"{EntityName} {action} successful"
                };

                _masterAuditLogRepository.Add(successAuditRequest, out _);
            }

            return identity;
        }
    }
}
