using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterApplicationLicense;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterApplicationLicense
{
    public interface IMasterApplicationLicenseService
    {
        List<MasterApplicationLicenseResponseModel> GetAll(out string errorMessage);
        MasterApplicationLicenseResponseModel GetById(int id, out string errorMessage);
        int AddOrUpdate(MasterApplicationLicenseRequestModel request, out string errorMessage);
        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
