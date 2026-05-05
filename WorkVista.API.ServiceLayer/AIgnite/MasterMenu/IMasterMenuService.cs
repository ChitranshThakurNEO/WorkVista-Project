using WorkVista.API.ModelLayer.AIgnite.MasterMenu;

namespace WorkVista.API.ServiceLayer.AIgnite.MasterMenu
{
    public interface IMasterMenuService
    {
        List<MasterMenuResponseModel> GetAll(out string errorMessage);
    }
}
