using WorkVista.API.ModelLayer.AIgnite.MasterMenu;
using WorkVista.API.RepositoryLayer.AIgnite.MasterMenu;

namespace WorkVista.API.ServiceLayer.AIgnite.MasterMenu
{
    public class MasterMenuService : IMasterMenuService
    {
        private readonly IMasterMenuRepository _masterMenuRepository;

        public MasterMenuService(IMasterMenuRepository masterMenuRepository)
        {
            _masterMenuRepository = masterMenuRepository;
        }

        public List<MasterMenuResponseModel> GetAll(out string errorMessage)
        {
            return _masterMenuRepository.GetAll(out errorMessage);
        }
    }
}
