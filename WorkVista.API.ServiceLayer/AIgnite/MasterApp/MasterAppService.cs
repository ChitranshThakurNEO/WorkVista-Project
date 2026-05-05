using WorkVista.API.ModelLayer.AIgnite.MasterApp;
using WorkVista.API.RepositoryLayer.AIgnite.MasterApp;

namespace WorkVista.API.ServiceLayer.AIgnite.MasterApp
{
    public class MasterAppService : IMasterAppService
    {
        private readonly IMasterAppRepository _masterAppRepository;

        public MasterAppService(IMasterAppRepository masterAppRepository)
        {
            _masterAppRepository = masterAppRepository;
        }

        public List<MasterAppResponseModel> GetAllByMenuId(int masterMenuId, out string errorMessage)
        {
            return _masterAppRepository.GetAllByMenuId(masterMenuId, out errorMessage);
        }
    }
}
