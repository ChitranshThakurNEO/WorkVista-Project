using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.MasterShift;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterShift
{
    public class MasterShiftRepository : IMasterShiftRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.MasterShift;

        public MasterShiftRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(options.CurrentValue.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        public List<MasterShiftResponseModel> GetAll(out string errorMessage)
        {
            errorMessage = string.Empty;

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllMasterShifts,
                new Dictionary<string, object>(),
                out errorMessage
            );

            return dt.ToList<MasterShiftResponseModel>();
        }

        public MasterShiftResponseModel GetById(int id, out string errorMessage)
        {
            errorMessage = string.Empty;

            var param = new Dictionary<string, object>
            {
                { "MasterShiftId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetMasterShiftDetailsById,
                param,
                out errorMessage
            );

            return dt?.Rows.Count > 0
            ? dt.ToList<MasterShiftResponseModel>().FirstOrDefault()
            : null;
        }

        public int AddOrUpdate(MasterShiftRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = request.MasterShiftId.HasValue && request.MasterShiftId > 0
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;

            try
            {
                var param = new Dictionary<string, object>
                {
                    { "MasterShiftId", request.MasterShiftId ?? (object)DBNull.Value },
                    { "Name", request.Name },
                    { "StartTime", request.StartTime },
                    { "EndTime", request.EndTime },
                    { "IsFlexi", request.IsFlexi },
                    { "CreatedBy", request.CreatedBy }
                };

                var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                    WorkVistaStoreProcedures.WorkVista_UpsertMasterShiftDetails,
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
                        EntityId = request.MasterShiftId?.ToString(),
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
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                var exceptionAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = request.CreatedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = request.MasterShiftId?.ToString(),
                    EntityJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(request),
                    Description = ex.Message
                };

                _masterAuditLogRepository.Add(exceptionAuditRequest, out _);

                return 0;
            }

        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = AuditConstants.Actions.Delete;

            try
            {
                var param = new Dictionary<string, object>
                {
                    { "MasterShiftId", deleteRequest.Id },
                    { "DeletedReason", deleteRequest.Reason ?? string.Empty },
                    { "DeletedBy", deleteRequest.DeletedBy }
                };

                var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                    WorkVistaStoreProcedures.WorkVista_DeleteMasterShiftDetails,
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
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                var exceptionAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = deleteRequest.DeletedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = deleteRequest.Id.ToString(),
                    EntityJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(deleteRequest),
                    Description = ex.Message
                };

                _masterAuditLogRepository.Add(exceptionAuditRequest, out _);
                return 0;
            }
}
    }
}
