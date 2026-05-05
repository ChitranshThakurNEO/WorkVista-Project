namespace WorkVista.API.RepositoryLayer.Common.AuditLogs
{
    public interface IAuditLogCommonRepository
    {
        void AddAuditLog(int userId, string action,
                                    string entityName,
                                    string entityId,
                                    object requestData,
                                    string description);
    }
}
