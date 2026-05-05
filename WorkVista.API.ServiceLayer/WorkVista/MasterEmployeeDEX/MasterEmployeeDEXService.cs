using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeDEX;
using WorkVista.API.RepositoryLayer.WorkVista.MasterEmployeeDEX;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterEmployeeDEX
{
    public class MasterEmployeeDEXService : IMasterEmployeeDEXService
    {
        private readonly IMasterEmployeeDEXRepository _repository;

        public MasterEmployeeDEXService(IMasterEmployeeDEXRepository repository)
        {
            _repository = repository;
        }

        public List<MasterEmployeeDEXResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterEmployeeDEXResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }
    }
}
