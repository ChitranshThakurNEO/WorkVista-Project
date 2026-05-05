using WorkVista.API.ModelLayer.AIgnite.MasterMenu;

namespace WorkVista.API.RepositoryLayer.AIgnite.MasterMenu
{
    public interface IMasterMenuRepository
    {
        List<MasterMenuResponseModel> GetAll(out string errorMessage);
    }
}
