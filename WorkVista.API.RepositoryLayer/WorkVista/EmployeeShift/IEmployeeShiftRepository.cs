using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.EmployeeShift;

namespace WorkVista.API.RepositoryLayer.WorkVista.EmployeeShift
{
    public interface IEmployeeShiftRepository
    {
        List<EmployeeShiftResponseModel> GetAll(out string errorMessage);
        EmployeeShiftResponseModel GetById(int id, out string errorMessage);
        int AddOrUpdate(EmployeeShiftRequestModel request, out string errorMessage);
        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
