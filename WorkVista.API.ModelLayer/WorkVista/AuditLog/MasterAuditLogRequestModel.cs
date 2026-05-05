namespace WorkVista.API.ModelLayer.WorkVista.AuditLog
{
    public class MasterAuditLogRequestModel
    {
        public int? UserId { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string EntityJsonData { get; set; }
        public string Description { get; set; }
    }
}
