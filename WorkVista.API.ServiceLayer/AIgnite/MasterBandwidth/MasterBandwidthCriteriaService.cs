using WorkVista.API.ModelLayer.AIgnite.MasterBandwidth;
using WorkVista.API.RepositoryLayer.AIgnite.MasterBandwidth;

namespace WorkVista.API.ServiceLayer.AIgnite.MasterBandwidth
{
    public class MasterBandwidthCriteriaService : IMasterBandwidthCriteriaService
    {
        private readonly IMasterBandwidthCriteriaRepository _masterBandwidthCriteriaRepository;

        public MasterBandwidthCriteriaService(IMasterBandwidthCriteriaRepository masterBandwidthCriteriaRepository)
        {
            _masterBandwidthCriteriaRepository = masterBandwidthCriteriaRepository;
        }

        public List<MasterBandwidthCriteriaResponseModel> GetAll(out string errorMessage)
        {
            return _masterBandwidthCriteriaRepository.GetAll(out errorMessage);
        }
    }
}
