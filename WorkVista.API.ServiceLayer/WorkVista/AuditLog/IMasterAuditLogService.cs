using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkVista.API.ModelLayer.WorkVista.AuditLog;

namespace WorkVista.API.ServiceLayer.WorkVista.AuditLog
{
    public interface IMasterAuditLogService
    {
        void Log(MasterAuditLogRequestModel request);
    }
}
