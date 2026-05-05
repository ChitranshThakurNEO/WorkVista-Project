using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityCategory;
using WorkVista.API.RepositoryLayer.WorkVista.MasterActivityCategory;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterActivityCategory
{
    public class MasterActivityCategoryService : IMasterActivityCategoryService
    {
        private readonly IMasterActivityCategoryRepository _repository;

        public MasterActivityCategoryService(IMasterActivityCategoryRepository repository)
        {
            _repository = repository;
        }

        public List<MasterActivityCategoryResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterActivityCategoryResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(MasterActivityCategoryRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }
    }
}
