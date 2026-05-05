using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSession;
using WorkVista.API.RepositoryLayer.WorkVista.MasterEmployeeSession;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterEmployeeSession
{
    public class MasterEmployeeSessionService : IMasterEmployeeSessionService
    {
        private readonly IMasterEmployeeSessionRepository _repository;

        public MasterEmployeeSessionService(IMasterEmployeeSessionRepository repository)
        {
            _repository = repository;
        }

        public List<MasterEmployeeSessionResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterEmployeeSessionResponseModel GetById(long id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public long Login(MasterEmployeeSessionLoginRequestModel request, out string errorMessage)
        {
            return _repository.Login(request, out errorMessage);
        }

        public bool Logout(long sessionId, DateTime logoutTime, int createdBy, out string errorMessage)
        {
            return _repository.Logout(sessionId, logoutTime, createdBy, out errorMessage);
        }

        public int Delete(DeleteRequestLong deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }

        public bool ToggleActive(long id, int createdBy, out string errorMessage)
        {
            return _repository.ToggleActive(id, createdBy, out errorMessage);
        }
    }
}
