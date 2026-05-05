using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;

namespace WorkVista.API.RepositoryLayer.Common.AuditLogs
{
    public class AuditLogCommonRepository : IAuditLogCommonRepository
    {
        private readonly IMasterAuditLogRepository _masterAuditLogRepository;
        public AuditLogCommonRepository(IMasterAuditLogRepository masterAuditLogRepository)
        {
            _masterAuditLogRepository = masterAuditLogRepository;
        }
        public void AddAuditLog(   int userId,
                                    string action,
                                    string entityName,
                                    string entityId,
                                    object requestData,
                                    string description)
        {
                var auditRequest = new MasterAuditLogRequestModel
                {
                    UserId = userId,
                    Action = action,
                    EntityName = entityName,
                    EntityId = entityId,
                    EntityJsonData = Newtonsoft.Json.JsonConvert.SerializeObject(requestData),
                    Description = description
                };

                _masterAuditLogRepository.Add(auditRequest, out _);
        }
    }
}
