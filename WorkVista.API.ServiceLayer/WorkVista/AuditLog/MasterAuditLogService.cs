using WorkVista.API.ModelLayer.WorkVista.AuditLog;
using WorkVista.API.RepositoryLayer.WorkVista.AuditLog;

namespace WorkVista.API.ServiceLayer.WorkVista.AuditLog
{
    public class MasterAuditLogService : IMasterAuditLogService
    {
        private readonly IMasterAuditLogRepository _repository;

        public MasterAuditLogService(IMasterAuditLogRepository repository)
        {
            _repository = repository;
        }

        public void Log(MasterAuditLogRequestModel request)
        {
            _repository.Add(request, out string errorMessage);
        }
    }
}
