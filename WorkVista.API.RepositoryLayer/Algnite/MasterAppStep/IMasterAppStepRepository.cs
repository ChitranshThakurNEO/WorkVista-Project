using WorkVista.API.ModelLayer.AIgnite.MasterAppStep;

namespace WorkVista.API.RepositoryLayer.AIgnite.MasterAppStep
{
    public interface IMasterAppStepRepository
    {
        List<MasterAppStepResponseModel> GetAllByMasterAppId(int masterAppId, out string errorMessage);
    }
}
