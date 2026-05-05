using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterAlert
{
    public class MasterAlertResponseModel : MasterAuditDto
    {
        public long MasterAlertId { get; set; }
        public int EmployeeId { get; set; }

        public string AlertType { get; set; }
        public DateTime AlertDate { get; set; }
        public string Message { get; set; }

        public bool IsResolved { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }

    }
}
