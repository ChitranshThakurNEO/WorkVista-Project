namespace WorkVista.API.ModelLayer.WorkVista.LicenseAssignment
{
    public class LicenseAssignmentReleaseRequest
    {
        public int LicenseAssignmentId { get; set; }
        public DateTime ReleasedDate { get; set; }
        public int CreatedBy { get; set; }
    }
}
