using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityLog;
using WorkVista.API.RepositoryLayer.WorkVista.MasterActivityLog;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterActivityLog
{
    public class MasterActivityLogService : IMasterActivityLogService
    {
        private readonly IMasterActivityLogRepository _repository;

        public MasterActivityLogService(IMasterActivityLogRepository repository)
        {
            _repository = repository;
        }

        public List<MasterActivityLogResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterActivityLogResponseModel GetById(long id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public List<MasterActivityLogResponseModel> GetByEmployee(int employeeId, out string errorMessage)
        {
            return _repository.GetByEmployee(employeeId, out errorMessage);
        }

        public List<MasterActivityLogResponseModel> GetByEmployeeAndDate(int employeeId, DateTime date, out string errorMessage)
        {
            return _repository.GetByEmployeeAndDate(employeeId, date, out errorMessage);
        }

        public int AddOrUpdate(MasterActivityLogRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public long StartActivity(StartActivityRequestModel request, out string errorMessage)
        {
            return _repository.StartActivity(request, out errorMessage);
        }

        public bool StopActivity(StopActivityRequestModel request, out string errorMessage)
        {
            return _repository.StopActivity(request, out errorMessage);
        }

        public int Delete(DeleteRequestLong deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }
    }
}
