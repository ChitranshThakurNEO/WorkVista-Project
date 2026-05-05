namespace WorkVista.API.ModelLayer.WorkVista.LicenseAssignment
{
    public class LicenseAssignmentRequestModel
    {
        public int? LicenseAssignmentId { get; set; }
        public int ApplicationLicenseId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AssignedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
