using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterAlert;
using WorkVista.API.RepositoryLayer.WorkVista.MasterAlert;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterAlert
{
    public class MasterAlertService : IMasterAlertService
    {
        private readonly IMasterAlertRepository _repository;

        public MasterAlertService(IMasterAlertRepository repository)
        {
            _repository = repository;
        }

        public List<MasterAlertResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterAlertResponseModel GetById(long id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public long AddOrUpdate(MasterAlertRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public bool MarkAsRead(long id, int createdBy, out string errorMessage)
        {
            return _repository.MarkAsRead(id, createdBy, out errorMessage);
        }

        public bool Resolve(long id, int createdBy, out string errorMessage)
        {
            return _repository.Resolve(id, createdBy, out errorMessage);
        }

        public int Delete(DeleteRequestLong request, out string errorMessage)
        {
            return _repository.Delete(request, out errorMessage);
        }
    }
}
