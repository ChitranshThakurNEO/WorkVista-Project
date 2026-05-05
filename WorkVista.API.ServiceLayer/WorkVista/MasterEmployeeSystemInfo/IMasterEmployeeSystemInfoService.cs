using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSystemInfo;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterEmployeeSystemInfo
{
    public interface IMasterEmployeeSystemInfoService
    {
        List<MasterEmployeeSystemInfoResponseModel> GetAll(out string errorMessage);

        MasterEmployeeSystemInfoResponseModel GetById(int id, out string errorMessage);

        int AddOrUpdate(MasterEmployeeSystemInfoRequestModel request, out string errorMessage);

        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
