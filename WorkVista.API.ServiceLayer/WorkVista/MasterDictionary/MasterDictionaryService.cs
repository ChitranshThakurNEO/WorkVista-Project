using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterDictionary;
using WorkVista.API.RepositoryLayer.WorkVista.MasterDictionary;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterDictionary
{
    public class MasterDictionaryService : IMasterDictionaryService
    {
        private readonly IMasterDictionaryRepository _repository;

        public MasterDictionaryService(IMasterDictionaryRepository masterDictionaryRepository)
        {
            _repository = masterDictionaryRepository;
        }

        public List<MasterDictionaryResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterDictionaryResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(MasterDictionaryRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }
    }
}
