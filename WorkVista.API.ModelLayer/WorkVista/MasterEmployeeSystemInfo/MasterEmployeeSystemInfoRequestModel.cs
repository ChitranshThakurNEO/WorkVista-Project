namespace WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSystemInfo
{
    public class MasterEmployeeSystemInfoRequestModel
    {
        public int? MasterEmployeeSystemInfoId { get; set; }

        public int EmployeeId { get; set; }
        public string MachineName { get; set; }
        public string IPAddress { get; set; }
        public string OSVersion { get; set; }
        public string AgentVersion { get; set; }

        public int CreatedBy { get; set; }
    }
}
