using WorkVista.API.ModelLayer.AIgnite.MasterAppStep;
using WorkVista.API.RepositoryLayer.AIgnite.MasterAppStep;

namespace WorkVista.API.ServiceLayer.AIgnite.MasterAppStep
{
    public class MasterAppStepService : IMasterAppStepService
    {
        private readonly IMasterAppStepRepository _masterAppStepRepository;

        public MasterAppStepService(IMasterAppStepRepository masterAppStepRepository)
        {
            _masterAppStepRepository = masterAppStepRepository;
        }

        public List<MasterAppStepResponseModel> GetAllByMasterAppId(int masterAppId, out string errorMessage)
        {
            return _masterAppStepRepository.GetAllByMasterAppId(masterAppId, out errorMessage);
        }
    }
}
