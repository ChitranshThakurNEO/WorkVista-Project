namespace WorkVista.API.ModelLayer.WorkVista.MasterActivityLog
{
    public class MasterActivityLogRequestModel
    {
        public long? MasterActivityLogId { get; set; }
        public int EmployeeId { get; set; }
        public int? EmployeeSystemInfoId { get; set; }
        public int CategoryId { get; set; }
        public long SessionId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public string Status { get; set; }
        public DateTime LogDate { get; set; }

        public int CreatedBy { get; set; }
    }
}
