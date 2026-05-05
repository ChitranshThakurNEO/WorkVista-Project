using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityLog;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterActivityLog
{
    public class MasterActivityLogRepository : IMasterActivityLogRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.MasterActivityLog;

        public MasterActivityLogRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        public List<MasterActivityLogResponseModel> GetAll(out string errorMessage)
        {
            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllMasterActivityLogs,
                new Dictionary<string, object>(),
                out errorMessage);

            return dt.ToList<MasterActivityLogResponseModel>();
        }

        public MasterActivityLogResponseModel GetById(long id, out string errorMessage)
        {
            var param = new Dictionary<string, object>
        {
            { "MasterActivityLogId", id }
        };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetMasterActivityLogById,
                param,
                out errorMessage);

            if (dt.Rows.Count > 0)
                return dt.ToList<MasterActivityLogResponseModel>().FirstOrDefault();

            return null;
        }

        public List<MasterActivityLogResponseModel> GetByEmployee(int employeeId, out string errorMessage)
        {
            var param = new Dictionary<string, object>
        {
            { "EmployeeId", employeeId }
        };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetLogsByEmployee,
                param,
                out errorMessage);

            return dt.ToList<MasterActivityLogResponseModel>();
        }

        public List<MasterActivityLogResponseModel> GetByEmployeeAndDate(int employeeId, DateTime date, out string errorMessage)
        {
            var param = new Dictionary<string, object>
            {
                { "EmployeeId", employeeId },
                { "LogDate", date }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetLogsByEmployeeAndDate,
                param,
                out errorMessage);

            return dt.ToList<MasterActivityLogResponseModel>();
        }

        public int AddOrUpdate(MasterActivityLogRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = request.MasterActivityLogId.HasValue && request.MasterActivityLogId > 0
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;

            var param = new Dictionary<string, object>
            {
                { "MasterActivityLogId", request.MasterActivityLogId ?? (object)DBNull.Value },
                { "EmployeeId", request.EmployeeId },
                { "EmployeeSystemInfoId", request.EmployeeSystemInfoId ?? (object)DBNull.Value },
                { "CategoryId", request.CategoryId },
                { "SessionId", request.SessionId },
                { "StartTime", request.StartTime },
                { "EndTime", request.EndTime ?? (object)DBNull.Value },
                { "Status", request.Status },
                { "LogDate", request.LogDate },
                { "CreatedBy", request.CreatedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_UpsertMasterActivityLog,
                param,
                out errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                var errorAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = request.CreatedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = request.MasterActivityLogId?.ToString(),
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

        public long StartActivity(StartActivityRequestModel request, out string errorMessage)
        {
            var param = new Dictionary<string, object>
        {
            { "EmployeeId", request.EmployeeId },
            { "EmployeeSystemInfoId", request.EmployeeSystemInfoId ?? (object)DBNull.Value },
            { "CategoryId", request.CategoryId },
            { "SessionId", request.SessionId },
            { "StartTime", request.StartTime },
            { "LogDate", request.LogDate },
            { "CreatedBy", request.CreatedBy }
        };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_StartMasterActivityLog,
                param,
                out errorMessage);

            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToInt64(dt.Rows[0][0]);

            return 0;
        }

        public bool StopActivity(StopActivityRequestModel request, out string errorMessage)
        {
            var param = new Dictionary<string, object>
        {
            { "MasterActivityLogId", request.MasterActivityLogId },
            { "EndTime", request.EndTime },
            { "CreatedBy", request.CreatedBy }
        };

            _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_StopMasterActivityLog,
                param,
                out errorMessage);

            return string.IsNullOrEmpty(errorMessage);
        }

        public int Delete(DeleteRequestLong deleteRequest, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = AuditConstants.Actions.Delete;

            var param = new Dictionary<string, object>
            {
                { "MasterActivityLogId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteMasterActivityLog,
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
