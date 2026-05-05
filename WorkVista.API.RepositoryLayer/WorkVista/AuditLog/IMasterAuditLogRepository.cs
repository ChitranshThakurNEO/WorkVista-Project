using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;

namespace WorkVista.API.RepositoryLayer.WorkVista.AuditLog
{
    public interface IMasterAuditLogRepository
    {
        long Add(MasterAuditLogRequestModel request, out string errorMessage);
    }
}
