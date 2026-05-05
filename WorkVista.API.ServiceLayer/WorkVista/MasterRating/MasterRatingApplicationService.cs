using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterRating;
using WorkVista.API.RepositoryLayer.WorkVista.MasterRatingApplication;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterRating
{
    public class MasterRatingApplicationService : IMasterRatingApplicationService
    {
        private readonly IMasterRatingApplicationRepository _repository;

        public MasterRatingApplicationService(IMasterRatingApplicationRepository repository)
        {
            _repository = repository;
        }

        public List<MasterRatingApplicationResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public MasterRatingApplicationResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(MasterRatingApplicationRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public int Delete(DeleteRequest deleteRequest, out string errorMessage)
        {
            return _repository.Delete(deleteRequest, out errorMessage);
        }

        public bool ToggleActive(int id, int createdBy, out string errorMessage)
        {
            return _repository.ToggleActive(id, createdBy, out errorMessage);
        }
    }
}
