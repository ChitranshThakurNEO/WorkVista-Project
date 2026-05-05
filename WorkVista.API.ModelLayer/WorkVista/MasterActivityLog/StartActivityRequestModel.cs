namespace WorkVista.API.ModelLayer.WorkVista.MasterActivityLog
{
    public class StartActivityRequestModel
    {
        public int EmployeeId { get; set; }
        public int? EmployeeSystemInfoId { get; set; }
        public int CategoryId { get; set; }
        public long SessionId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime LogDate { get; set; }

        public int CreatedBy { get; set; }
    }
}
