namespace WorkVista.API.ModelLayer.WorkVista.MasterActivityLog
{
    public class StopActivityRequestModel
    {
        public long MasterActivityLogId { get; set; }
        public DateTime EndTime { get; set; }
        public int CreatedBy { get; set; }
    }
}
