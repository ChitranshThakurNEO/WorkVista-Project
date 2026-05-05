using Microsoft.Extensions.Options;
using System.Data;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSession;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterEmployeeSession
{
    public class MasterEmployeeSessionRepository : IMasterEmployeeSessionRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.MasterEmployeeSession;

        public MasterEmployeeSessionRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        public List<MasterEmployeeSessionResponseModel> GetAll(out string errorMessage)
        {
            errorMessage = string.Empty;

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllMasterEmployeeSessions,
                null,
                out errorMessage
            );

            return dt.ToList<MasterEmployeeSessionResponseModel>();
        }

        public MasterEmployeeSessionResponseModel GetById(long id, out string errorMessage)
        {
            errorMessage = string.Empty;

            var parameters = new Dictionary<string, object>
            {
                { "MasterEmployeeSessionId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetMasterEmployeeSessionById,
                parameters,
                out errorMessage
            );

            if (dt != null && dt.Rows.Count > 0)
                return dt.ToList<MasterEmployeeSessionResponseModel>().FirstOrDefault();

            return null;
        }

        public long Login(MasterEmployeeSessionLoginRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var inParams = new Dictionary<string, object>
            {
                { "EmployeeId", request.EmployeeId },
                { "DeviceId", request.DeviceId ?? (object)DBNull.Value },
                { "LoginTime", request.LoginTime },
                { "SessionDate", request.SessionDate },
                { "CreatedBy", request.CreatedBy }
            };

            var outParams = new string[,]
            {
                { "SessionId", DBConstants.SQLDBTYPE_BIGINT }
            };

            var result = _sqlDbUtilities.ExectuteStoredProcedureToArray(
                WorkVistaStoreProcedures.WorkVista_LoginEmployeeSession,
                inParams,
                outParams,
                out errorMessage
            );

            if (result != null)
            {
                var data = result.Cast<string>()
                    .Select((s, i) => new { s, i })
                    .GroupBy(x => x.i / result.GetLength(1))
                    .ToDictionary(g => g.First().s, g => g.Skip(1).Select(y => y.s).FirstOrDefault());

                return Convert.ToInt64(data["SessionId"]);
            }

            return 0;
        }

        public bool Logout(long sessionId, DateTime logoutTime, int createdBy, out string errorMessage)
        {
            errorMessage = string.Empty;

            var parameters = new Dictionary<string, object>
            {
                { "MasterEmployeeSessionId", sessionId },
                { "LogoutTime", logoutTime },
                { "CreatedBy", createdBy }
            };

            var result = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_LogoutEmployeeSession,
                parameters,
                out errorMessage
            );

            return string.IsNullOrEmpty(errorMessage);
        }

        public int Delete(DeleteRequestLong deleteRequest, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = AuditConstants.Actions.Delete;

            var parameters = new Dictionary<string, object>
            {
                { "MasterEmployeeSessionId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteMasterEmployeeSession,
                parameters,
                out errorMessage
            );

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

        public bool ToggleActive(long id, int createdBy, out string errorMessage)
        {
            errorMessage = string.Empty;

            var parameters = new Dictionary<string, object>
            {
                { "MasterEmployeeSessionId", id },
                { "CreatedBy", createdBy }
            };

            var result = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_ToggleMasterEmployeeSessionActive,
                parameters,
                out errorMessage
            );

            return result != null;
        }
    }
}