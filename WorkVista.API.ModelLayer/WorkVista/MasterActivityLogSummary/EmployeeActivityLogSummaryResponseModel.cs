using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterActivityLogSummary
{
    public class EmployeeActivityLogSummaryResponseModel : MasterAuditDto
    {
        public long EmployeeActivityLogSummaryId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LoggedDate { get; set; }

        public decimal TOT_LOGGEDHOURS { get; set; }
        public decimal TOT_ACTIVEHOURS { get; set; }
        public decimal TOT_TIMEONSYSTEM { get; set; }
        public decimal TOT_TIMEAWAYFROMSYSTEM { get; set; }
        public decimal TOT_IDLETIME { get; set; }

        public DateTime FIRSTLOGIN { get; set; }
        public DateTime LASTLOGOUT { get; set; }

        public string SHIFTNAME { get; set; }
        public string NOOFDAYS { get; set; }
        public string DAYTYPE { get; set; }
        public string AttendanceStatus { get; set; }

        public bool IsValidated { get; set; }
    }
}
