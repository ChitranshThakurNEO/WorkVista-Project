using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSystemInfo
{
    public class MasterEmployeeSystemInfoResponseModel : MasterAuditDto
    {
        public int MasterEmployeeSystemInfoId { get; set; }
        public int EmployeeId { get; set; }

        public string MachineName { get; set; }
        public string IPAddress { get; set; }
        public string OSVersion { get; set; }
        public string AgentVersion { get; set; }

    }
}
