namespace WorkVista.API.ModelLayer.WorkVista.MasterAlert
{
    public class MasterAlertRequestModel
    {
        public long? MasterAlertId { get; set; }

        public int EmployeeId { get; set; }
        public string AlertType { get; set; }
        public DateTime AlertDate { get; set; }
        public string Message { get; set; }

        public int CreatedBy { get; set; }
    }
}
