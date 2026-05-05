using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSession
{
    public class MasterEmployeeSessionLoginRequestModel
    {
        public int EmployeeId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime SessionDate { get; set; }
        public int CreatedBy { get; set; }
    }

    public class LogoutRequest
    {
        public int Id { get; set; }
        public DateTime LogoutTime { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ToggleRequest
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
    }
}
