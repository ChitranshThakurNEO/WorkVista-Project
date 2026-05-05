using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.EmployeeShift;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.EmployeeShift
{
    public class EmployeeShiftRepository : IEmployeeShiftRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.EmployeeShift;

        public EmployeeShiftRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        public List<EmployeeShiftResponseModel> GetAll(out string errorMessage)
        {
            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllEmployeeShifts,
                new Dictionary<string, object>(),
                out errorMessage);

            return dt.ToList<EmployeeShiftResponseModel>();
        }

        public EmployeeShiftResponseModel GetById(int id, out string errorMessage)
        {
            var param = new Dictionary<string, object>
            {
                { "EmployeeShiftId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetEmployeeShiftById,
                param,
                out errorMessage);

            if (dt.Rows.Count > 0)
                return dt.ToList<EmployeeShiftResponseModel>().FirstOrDefault();

            return null;
        }

        public int AddOrUpdate(EmployeeShiftRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = request.EmployeeShiftId.HasValue && request.EmployeeShiftId > 0
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;
            var param = new Dictionary<string, object>
            {
                { "EmployeeShiftId", request.EmployeeShiftId ?? (object)DBNull.Value },
                { "EmployeeId", request.EmployeeId },
                { "ShiftId", request.ShiftId },
                { "EffectiveFrom", request.EffectiveFrom },
                { "EffectiveTo", request.EffectiveTo ?? (object)DBNull.Value },
                { "CreatedBy", request.CreatedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_UpsertEmployeeShift,
                param,
                out errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                var errorAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = request.CreatedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = request.EmployeeShiftId?.ToString(),
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

            var param = new Dictionary<string, object>
            {
                { "EmployeeShiftId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteEmployeeShift,
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
