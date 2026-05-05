using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityCategory;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterActivityCategory
{
    public class MasterActivityCategoryRepository : IMasterActivityCategoryRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.MasterActivityCategory;

        public MasterActivityCategoryRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        public List<MasterActivityCategoryResponseModel> GetAll(out string errorMessage)
        {
            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllMasterActivityCategories,
                new Dictionary<string, object>(),
                out errorMessage);

            return dt.ToList<MasterActivityCategoryResponseModel>();
        }

        public MasterActivityCategoryResponseModel GetById(int id, out string errorMessage)
        {
            var param = new Dictionary<string, object>
            {
                { "MasterActivityCategoryId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetMasterActivityCategoryById,
                param,
                out errorMessage);

            if (dt.Rows.Count > 0)
                return dt.ToList<MasterActivityCategoryResponseModel>().FirstOrDefault();

            return null;
        }

        public int AddOrUpdate(MasterActivityCategoryRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = request.MasterActivityCategoryId.HasValue && request.MasterActivityCategoryId > 0
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;

            var param = new Dictionary<string, object>
            {
                { "MasterActivityCategoryId", request.MasterActivityCategoryId ?? (object)DBNull.Value },
                { "ActivityTypeId", request.ActivityTypeId },
                { "Geo", request.Geo },
                { "Company", request.Company },
                { "Department", request.Department },
                { "ProcessName", request.ProcessName },
                { "SubCategoryName", request.SubCategoryName },
                { "SubCategoryContent", request.SubCategoryContent },
                { "ColorHex", request.ColorHex },
                { "SortOrder", request.SortOrder },

                { "IsProductive", request.IsProductive },
                { "IsNonProductive", request.IsNonProductive },
                { "IsNoImpact", request.IsNoImpact },

                { "CreatedBy", request.CreatedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_UpsertMasterActivityCategory,
                param,
                out errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                var errorAuditRequest = new MasterAuditLogRequestModel
                {
                    UserId = request.CreatedBy,
                    Action = AuditConstants.Actions.Error,
                    EntityName = EntityName,
                    EntityId = request.MasterActivityCategoryId?.ToString(),
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
                { "MasterActivityCategoryId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteMasterActivityCategory,
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
