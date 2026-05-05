namespace WorkVista.API.ModelLayer.WorkVista.MasterShift
{
    public class MasterShiftRequestModel
    {
        public int? MasterShiftId { get; set; }
        public string Name { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public bool IsFlexi { get; set; }

        public int CreatedBy { get; set; }
    }
}
