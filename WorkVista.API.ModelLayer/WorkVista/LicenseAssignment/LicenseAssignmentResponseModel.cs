using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.LicenseAssignment
{
    public class LicenseAssignmentResponseModel : MasterAuditDto
    {
        public int LicenseAssignmentId { get; set; }
        public int ApplicationLicenseId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? ReleasedDate { get; set; }
    }
}
