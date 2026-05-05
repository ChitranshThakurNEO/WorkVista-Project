namespace WorkVista.API.ModelLayer.WorkVista.EmployeeShift
{
    public class EmployeeShiftRequestModel
    {
        public int? EmployeeShiftId { get; set; }
        public int EmployeeId { get; set; }
        public int ShiftId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public int CreatedBy { get; set; }
    }
}
