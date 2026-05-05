using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityLog;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterActivityLog
{
    public interface IMasterActivityLogService
    {
        List<MasterActivityLogResponseModel> GetAll(out string errorMessage);

        MasterActivityLogResponseModel GetById(long id, out string errorMessage);

        List<MasterActivityLogResponseModel> GetByEmployee(int employeeId, out string errorMessage);

        List<MasterActivityLogResponseModel> GetByEmployeeAndDate(int employeeId, DateTime date, out string errorMessage);

        int AddOrUpdate(MasterActivityLogRequestModel request, out string errorMessage);

        long StartActivity(StartActivityRequestModel request, out string errorMessage);

        bool StopActivity(StopActivityRequestModel request, out string errorMessage);

        int Delete(DeleteRequestLong deleteRequest, out string errorMessage);
    }
}
