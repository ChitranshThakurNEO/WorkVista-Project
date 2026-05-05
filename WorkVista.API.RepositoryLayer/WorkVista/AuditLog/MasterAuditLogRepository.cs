using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.AuditLog
{
    public class MasterAuditLogRepository : IMasterAuditLogRepository
    {
        private readonly SqlDbUtilities _sqlDbUtilities;

        public MasterAuditLogRepository(IOptionsMonitor<AppSettingsModel> options)
        {
            _sqlDbUtilities = new SqlDbUtilities(options.CurrentValue.ConnectingString);
        }

        public long Add(MasterAuditLogRequestModel request, out string errorMessage)
        {
            var param = new Dictionary<string, object>
        {
            { "UserId", request.UserId ?? (object)DBNull.Value },
            { "Action", request.Action },
            { "EntityName", request.EntityName },
            { "EntityId", request.EntityId },
            { "EntityJsonData", request.EntityJsonData },
            { "Description", request.Description }
        };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_AddMasterAuditLog,
                param,
                out errorMessage);

            return (dt != null && dt.Rows.Count > 0)
                ? Convert.ToInt64(dt.Rows[0][0])
                : 0;
        }
    }
}
