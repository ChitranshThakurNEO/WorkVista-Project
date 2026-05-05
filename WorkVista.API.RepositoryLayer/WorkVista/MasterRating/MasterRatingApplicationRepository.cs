using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.MasterRating;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using WorkVista.API.RepositoryLayer.WorkVista.MasterRatingApplication;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterRating
{
    public class MasterRatingApplicationRepository : IMasterRatingApplicationRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.MasterRatingApplication;

        public MasterRatingApplicationRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        public List<MasterRatingApplicationResponseModel> GetAll(out string errorMessage)
        {
            errorMessage = string.Empty;

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllMasterRatingApplications,
                null,
                out errorMessage
            );

            return dt.ToList<MasterRatingApplicationResponseModel>();
        }

        public MasterRatingApplicationResponseModel GetById(int id, out string errorMessage)
        {
            errorMessage = string.Empty;

            var param = new Dictionary<string, object>
            {
                { "MasterRatingApplicationId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetMasterRatingApplicationById,
                param,
                out errorMessage
            );

            return dt != null && dt.Rows.Count > 0
                ? dt.ToList<MasterRatingApplicationResponseModel>().FirstOrDefault()
                : null;
        }

        public int AddOrUpdate(MasterRatingApplicationRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = request.MasterRatingApplicationId.HasValue && request.MasterRatingApplicationId > 0
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;

            var parameters = new Dictionary<string, object>
            {
                { "MasterRatingApplicationId", request.MasterRatingApplicationId ?? (object)DBNull.Value },
                { "ActivityCategoryId", request.ActivityCategoryId },
                { "Name", request.Name },
                { "Score", request.Score },
                { "CreatedBy", request.CreatedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_UpsertMasterRatingApplication,
                parameters,
                out errorMessage
            );

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                var errorAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = request.CreatedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = request.MasterRatingApplicationId?.ToString(),
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
                { "MasterRatingApplicationId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteMasterRatingApplication,
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

        public bool ToggleActive(int id, int createdBy, out string errorMessage)
        {
            errorMessage = string.Empty;

            var param = new Dictionary<string, object>
            {
                { "MasterRatingApplicationId", id },
                { "CreatedBy", createdBy }
            };

            var result = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_ToggleMasterRatingApplicationActive,
                param,
                out errorMessage
            );

            return result != null;
        }
    }
}
