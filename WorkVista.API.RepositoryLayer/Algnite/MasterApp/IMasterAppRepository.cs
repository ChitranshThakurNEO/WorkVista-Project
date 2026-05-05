using WorkVista.API.ModelLayer.AIgnite.MasterApp;

namespace WorkVista.API.RepositoryLayer.AIgnite.MasterApp
{
    public interface IMasterAppRepository
    {
        List<MasterAppResponseModel> GetAllByMenuId(int masterMenuId, out string errorMessage);
    }
}
