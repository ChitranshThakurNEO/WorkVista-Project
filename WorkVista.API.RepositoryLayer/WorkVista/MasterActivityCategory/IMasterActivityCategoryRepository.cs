using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityCategory;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterActivityCategory
{
    public interface IMasterActivityCategoryRepository
    {
        List<MasterActivityCategoryResponseModel> GetAll(out string errorMessage);

        MasterActivityCategoryResponseModel GetById(int id, out string errorMessage);

        int AddOrUpdate(MasterActivityCategoryRequestModel request, out string errorMessage);

        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
