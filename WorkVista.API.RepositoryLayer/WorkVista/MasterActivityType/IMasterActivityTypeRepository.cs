using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityType;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterActivityType
{
    public interface IMasterActivityTypeRepository
    {
        List<MasterActivityTypeResponseModel> GetAll(out string errorMessage);

        MasterActivityTypeResponseModel GetById(int id, out string errorMessage);

        int AddOrUpdate(MasterActivityTypeRequestModel request, out string errorMessage);

        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
