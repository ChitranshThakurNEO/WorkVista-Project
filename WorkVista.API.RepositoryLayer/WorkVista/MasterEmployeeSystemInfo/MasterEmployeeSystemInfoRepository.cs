using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSystemInfo;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterEmployeeSystemInfo
{
    public class MasterEmployeeSystemInfoRepository : IMasterEmployeeSystemInfoRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.MasterEmployeeSystemInfo;

        public MasterEmployeeSystemInfoRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        public List<MasterEmployeeSystemInfoResponseModel> GetAll(out string errorMessage)
        {
            errorMessage = string.Empty;

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllMasterEmployeeSystemInfos,
                new Dictionary<string, object>(),
                out errorMessage
            );

            return dt.ToList<MasterEmployeeSystemInfoResponseModel>();
        }

        public MasterEmployeeSystemInfoResponseModel GetById(int id, out string errorMessage)
        {
            errorMessage = string.Empty;

            var param = new Dictionary<string, object>
            {
                { "MasterEmployeeSystemInfoId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetMasterEmployeeSystemInfoById,
                param,
                out errorMessage
            );

            return dt?.Rows.Count > 0
                ? dt.ToList<MasterEmployeeSystemInfoResponseModel>().FirstOrDefault()
                : null;
        }

        public int AddOrUpdate(MasterEmployeeSystemInfoRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = request.MasterEmployeeSystemInfoId.HasValue && request.MasterEmployeeSystemInfoId > 0
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;

            var param = new Dictionary<string, object>
            {
                { "MasterEmployeeSystemInfoId", request.MasterEmployeeSystemInfoId ?? (object)DBNull.Value },
                { "EmployeeId", request.EmployeeId },
                { "MachineName", request.MachineName },
                { "IPAddress", request.IPAddress },
                { "OSVersion", request.OSVersion },
                { "AgentVersion", request.AgentVersion },
                { "CreatedBy", request.CreatedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_UpsertMasterEmployeeSystemInfo,
                param,
                out errorMessage
            );

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                var errorAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = request.CreatedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = request.MasterEmployeeSystemInfoId?.ToString(),
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

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = AuditConstants.Actions.Delete;

            errorMessage = string.Empty;

            var param = new Dictionary<string, object>
            {
                { "MasterEmployeeSystemInfoId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason ?? string.Empty },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteMasterEmployeeSystemInfo,
                param,
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
    }
}
