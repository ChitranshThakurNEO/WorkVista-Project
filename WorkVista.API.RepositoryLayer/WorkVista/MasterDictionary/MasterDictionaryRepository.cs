using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.ModelLayer.WorkVista.MasterDictionary;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterDictionary
{
    public class MasterDictionaryRepository : IMasterDictionaryRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        private const string EntityName = AuditConstants.Entities.MasterDictionary;

        public MasterDictionaryRepository(IOptionsMonitor<AppSettingsModel> options, IMasterAuditLogRepository masterAuditLogRepository)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
            _masterAuditLogRepository = masterAuditLogRepository;
        }

        #region GET ALL DICTIONARIES

        public List<MasterDictionaryResponseModel> GetAll(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var parameters = new Dictionary<string, object>(); // empty

                var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                    WorkVistaStoreProcedures.WorkVista_GetAllMasterDictionaries,
                    parameters,
                    out errorMessage
                );

                return dt.ToList<MasterDictionaryResponseModel>();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return new List<MasterDictionaryResponseModel>();
            }
        }

        #endregion

        #region GET BY DICTIONARY ID 

        public MasterDictionaryResponseModel GetById(int id, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "MasterDictionaryId", id }
                };

                var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                    WorkVistaStoreProcedures.WorkVista_GetMasterDictionaryDetailsById,
                    parameters,
                    out errorMessage
                );

                return dt?.Rows.Count > 0
                   ? dt.ToList<MasterDictionaryResponseModel>().FirstOrDefault()
                   : null;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return null;
            }
        }

        #endregion

        #region UPSERT THE DICTIONARY

        public int AddOrUpdate(MasterDictionaryRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = request.MasterDictionaryId.HasValue && request.MasterDictionaryId > 0
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;

            var parameters = new Dictionary<string, object>
            {
                { "MasterDictionaryId", request.MasterDictionaryId ?? (object)DBNull.Value },
                { "Key", request.Key },
                { "Value", request.Value },
                { "Geo", request.Geo },
                { "Company", request.Company },
                { "Department", request.Department },
                { "SubProcess", request.SubProcess },
                { "CreatedBy", request.CreatedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_UpsertMasterDictionaryDetails,
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
                    EntityId = request.MasterDictionaryId?.ToString(),
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

        #endregion

        #region DELETE THE DICTIONARY

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            errorMessage = string.Empty;

            var action = AuditConstants.Actions.Delete;

            var parameters = new Dictionary<string, object>
            {
                { "MasterDictionaryId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason ?? string.Empty },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteMasterDictionaryDetails,
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

        #endregion
    }
}
