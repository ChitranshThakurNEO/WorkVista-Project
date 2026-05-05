using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterDictionary;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterDictionary
{
    public interface IMasterDictionaryService
    {
        List<MasterDictionaryResponseModel> GetAll(out string errorMessage);

        MasterDictionaryResponseModel GetById(int id, out string errorMessage);

        int AddOrUpdate(MasterDictionaryRequestModel request, out string errorMessage);

        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
