using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterShift;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterShift
{
    public interface IMasterShiftService
    {
        List<MasterShiftResponseModel> GetAll(out string errorMessage);
        MasterShiftResponseModel GetById(int id, out string errorMessage);
        int AddOrUpdate(MasterShiftRequestModel request, out string errorMessage);
        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
