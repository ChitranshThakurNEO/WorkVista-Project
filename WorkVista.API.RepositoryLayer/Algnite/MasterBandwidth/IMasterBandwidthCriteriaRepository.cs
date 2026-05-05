using WorkVista.API.ModelLayer.AIgnite.MasterBandwidth;

namespace WorkVista.API.RepositoryLayer.AIgnite.MasterBandwidth
{
    public interface IMasterBandwidthCriteriaRepository
    {
        List<MasterBandwidthCriteriaResponseModel> GetAll(out string errorMessage);
    }
}
