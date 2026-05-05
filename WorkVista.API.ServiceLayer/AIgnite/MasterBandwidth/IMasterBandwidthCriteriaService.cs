using WorkVista.API.ModelLayer.AIgnite.MasterBandwidth;

namespace WorkVista.API.ServiceLayer.AIgnite.MasterBandwidth
{
    public interface IMasterBandwidthCriteriaService
    {
        List<MasterBandwidthCriteriaResponseModel> GetAll(out string errorMessage);
    }
}
