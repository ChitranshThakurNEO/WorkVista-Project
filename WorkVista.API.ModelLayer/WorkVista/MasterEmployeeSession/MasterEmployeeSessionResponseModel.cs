using System;
using System.Collections.Generic;
using System.Linq;
using WorkVista.API.ModelLayer.Common;
namespace WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSession
{
        public class MasterEmployeeSessionResponseModel : MasterAuditDto
    {
            public long MasterEmployeeSessionId { get; set; }
            public int EmployeeId { get; set; }
            public int? DeviceId { get; set; }
            public DateTime LoginTime { get; set; }
            public DateTime? LogoutTime { get; set; }
            public DateTime SessionDate { get; set; }
        }
}
