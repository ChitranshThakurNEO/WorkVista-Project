using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSession;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterEmployeeSession
{
    public interface IMasterEmployeeSessionRepository
    {
        List<MasterEmployeeSessionResponseModel> GetAll(out string errorMessage);

        MasterEmployeeSessionResponseModel GetById(long id, out string errorMessage);

        long Login(MasterEmployeeSessionLoginRequestModel request, out string errorMessage);

        bool Logout(long sessionId, DateTime logoutTime, int createdBy, out string errorMessage);

        int Delete(DeleteRequestLong deleteRequest, out string errorMessage);

        bool ToggleActive(long id, int createdBy, out string errorMessage);
    }
}