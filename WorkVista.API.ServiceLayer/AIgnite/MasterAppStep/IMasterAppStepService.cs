using WorkVista.API.ModelLayer.AIgnite.MasterAppStep;

namespace WorkVista.API.ServiceLayer.AIgnite.MasterAppStep
{
    public interface IMasterAppStepService
    {
        List<MasterAppStepResponseModel> GetAllByMasterAppId(int masterAppId, out string errorMessage);
    }
}
