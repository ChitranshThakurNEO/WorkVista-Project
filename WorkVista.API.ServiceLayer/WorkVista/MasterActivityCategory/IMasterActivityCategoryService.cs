using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityCategory;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterActivityCategory
{
    public interface IMasterActivityCategoryService
    {
        List<MasterActivityCategoryResponseModel> GetAll(out string errorMessage);

        MasterActivityCategoryResponseModel GetById(int id, out string errorMessage);

        int AddOrUpdate(MasterActivityCategoryRequestModel request, out string errorMessage);

        int Delete(DeleteRequest request, out string errorMessage);
    }
}
