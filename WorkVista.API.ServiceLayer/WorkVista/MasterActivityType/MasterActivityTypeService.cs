using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityType;
using WorkVista.API.RepositoryLayer.WorkVista.MasterActivityType;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterActivityType
{
    public class MasterActivityTypeService : IMasterActivityTypeService
    {
        private readonly IMasterActivityTypeRepository _repository;

        public MasterActivityTypeService(IMasterActivityTypeRepository repository)
        {
            _repository = repository;
        }

        public List<MasterActivityTypeResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterActivityTypeResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(MasterActivityTypeRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }
    }
}
