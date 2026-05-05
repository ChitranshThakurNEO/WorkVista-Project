using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSystemInfo;
using WorkVista.API.RepositoryLayer.WorkVista.MasterEmployeeSystemInfo;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterEmployeeSystemInfo
{
    public class MasterEmployeeSystemInfoService : IMasterEmployeeSystemInfoService
    {
        private readonly IMasterEmployeeSystemInfoRepository _repository;

        public MasterEmployeeSystemInfoService(IMasterEmployeeSystemInfoRepository repository)
        {
            _repository = repository;
        }

        public List<MasterEmployeeSystemInfoResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterEmployeeSystemInfoResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(MasterEmployeeSystemInfoRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }
    }
}