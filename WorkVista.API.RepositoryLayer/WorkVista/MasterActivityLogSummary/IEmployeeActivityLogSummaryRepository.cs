using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityLogSummary;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterActivityLogSummary
{
    public interface IEmployeeActivityLogSummaryRepository
    {
        List<EmployeeActivityLogSummaryResponseModel> GetAll(out string errorMessage);

        EmployeeActivityLogSummaryResponseModel GetById(long id, out string errorMessage);

        List<EmployeeActivityLogSummaryResponseModel> GetByEmployeeDate(
            int employeeId, DateTime fromDate, DateTime toDate, out string errorMessage);

        long AddOrUpdate(EmployeeActivityLogSummaryRequestModel request, out string errorMessage);

        int Delete(DeleteRequestLong request, out string errorMessage);
    }
}
