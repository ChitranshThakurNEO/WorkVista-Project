using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterApplicationLicense;
using WorkVista.API.RepositoryLayer.WorkVista.MasterApplicationLicense;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterApplicationLicense
{
    public class MasterApplicationLicenseService : IMasterApplicationLicenseService
    {
        private readonly IMasterApplicationLicenseRepository _repository;

        public MasterApplicationLicenseService(IMasterApplicationLicenseRepository repository)
        {
            _repository = repository;
        }

        public List<MasterApplicationLicenseResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterApplicationLicenseResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(MasterApplicationLicenseRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }
    }
}
