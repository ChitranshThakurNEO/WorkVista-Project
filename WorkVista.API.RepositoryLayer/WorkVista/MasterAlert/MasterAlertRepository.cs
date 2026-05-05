using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterAlert;
using WorkVista.API.RepositoryLayer.Common.AuditLogs;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterAlert
{
    public class MasterAlertRepository : IMasterAlertRepository
    {
        private readonly SqlDbUtilities _sqlDbUtilities;
        private readonly IAuditLogCommonRepository _auditLogCommonRepository;
        private const string EntityName = AuditConstants.Entities.MasterAlert;

        public MasterAlertRepository(IOptionsMonitor<AppSettingsModel> options)
        {
            _sqlDbUtilities = new SqlDbUtilities(options.CurrentValue.ConnectingString);
        }

        public List<MasterAlertResponseModel> GetAll(out string errorMessage)
        {
            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllMasterAlerts,
                new Dictionary<string, object>(),
                out errorMessage);

            return dt.ToList<MasterAlertResponseModel>();
        }

        public MasterAlertResponseModel GetById(long id, out string errorMessage)
        {
            var param = new Dictionary<string, object>
            {
                { "MasterAlertId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetMasterAlertById,
                param,
                out errorMessage);

            return dt.Rows.Count > 0
                ? dt.ToList<MasterAlertResponseModel>().FirstOrDefault()
                : null;
        }

        public long AddOrUpdate(MasterAlertRequestModel request, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool isUpdate = request.MasterAlertId.HasValue && request.MasterAlertId > 0;
            var action = isUpdate
                ? AuditConstants.Actions.Update
                : AuditConstants.Actions.Insert;

            var param = new Dictionary<string, object>
            {
                { "MasterAlertId", request.MasterAlertId },
                { "EmployeeId", request.EmployeeId },
                { "AlertType", request.AlertType },
                { "AlertDate", request.AlertDate },
                { "Message", request.Message },
                { "CreatedBy", request.CreatedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_UpsertMasterAlert,
                param,
                out errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                _auditLogCommonRepository.AddAuditLog(request.CreatedBy, 
                                            AuditConstants.Actions.Error,
                                            EntityName,
                                            request.MasterAlertId?.ToString(),
                                            request,
                                            errorMessage);
                return 0;
            }

            var identity = dt != null && dt.Rows.Count > 0
                ? Convert.ToInt32(dt.Rows[0]["AlertId"])
                : 0;

            if (identity > 0 && isUpdate)
            {
                _auditLogCommonRepository.AddAuditLog(request.CreatedBy,
                            AuditConstants.Actions.Update,
                            EntityName,
                            identity.ToString(),
                            request,
                            $"{EntityName} updated successfully");
            }

            return identity;
        }

        public bool MarkAsRead(long id, int createdBy, out string errorMessage)
        {
            var param = new Dictionary<string, object>
        {
            { "MasterAlertId", id },
            { "CreatedBy", createdBy }
        };

            _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_MarkMasterAlertAsRead,
                param,
                out errorMessage);

            return string.IsNullOrEmpty(errorMessage);
        }

        public bool Resolve(long id, int createdBy, out string errorMessage)
        {
            var param = new Dictionary<string, object>
        {
            { "MasterAlertId", id },
            { "CreatedBy", createdBy }
        };

            _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_ResolveMasterAlert,
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
                { "MasterAlertId", deleteRequest.Id },
                { "DeletedReason", deleteRequest.Reason },
                { "DeletedBy", deleteRequest.DeletedBy }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_DeleteMasterAlert,
                param,
                out errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                _auditLogCommonRepository.AddAuditLog(deleteRequest.DeletedBy,
                                                    AuditConstants.Actions.Error,
                                                    EntityName,
                                                    deleteRequest.Id.ToString(),
                                                    deleteRequest,
                                                    errorMessage);
                return 0;
            }

            var identity = dt != null && dt.Rows.Count > 0
                ? Convert.ToInt32(dt.Rows[0]["Id"])
                : 0;

            if (identity > 0)
            {
                _auditLogCommonRepository.AddAuditLog(deleteRequest.DeletedBy,
                                            AuditConstants.Actions.Delete,
                                            EntityName,
                                            identity.ToString(),
                                            deleteRequest,
                                            $"{EntityName} deleted successfully");
            }
            return identity;
        }
    }
}
