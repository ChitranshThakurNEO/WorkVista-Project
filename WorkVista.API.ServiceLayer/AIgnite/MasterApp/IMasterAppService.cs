using WorkVista.API.ModelLayer.AIgnite.MasterApp;

namespace WorkVista.API.ServiceLayer.AIgnite.MasterApp
{
    public interface IMasterAppService
    {
        List<MasterAppResponseModel> GetAllByMenuId(int masterMenuId, out string errorMessage);
    }
}
