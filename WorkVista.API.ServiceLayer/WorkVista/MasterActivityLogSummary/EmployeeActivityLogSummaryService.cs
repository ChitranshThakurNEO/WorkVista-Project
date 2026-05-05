using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityLogSummary;
using WorkVista.API.RepositoryLayer.WorkVista.MasterActivityLogSummary;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterActivityLogSummary
{
    public class EmployeeActivityLogSummaryService : IEmployeeActivityLogSummaryService
    {
        private readonly IEmployeeActivityLogSummaryRepository _repository;

        public EmployeeActivityLogSummaryService(IEmployeeActivityLogSummaryRepository repository)
        {
            _repository = repository;
        }

        public List<EmployeeActivityLogSummaryResponseModel> GetAll(out string errorMessage)
        {
           return _repository.GetAll(out errorMessage);
        }

        public EmployeeActivityLogSummaryResponseModel GetById(long id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public List<EmployeeActivityLogSummaryResponseModel> GetByEmployeeDate(
            int employeeId, DateTime fromDate, DateTime toDate, out string errorMessage)
        {
            return _repository.GetByEmployeeDate(employeeId, fromDate, toDate, out errorMessage);
        }

        public long AddOrUpdate(EmployeeActivityLogSummaryRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequestLong deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }
    }
}
