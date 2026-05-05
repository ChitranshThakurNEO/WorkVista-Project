using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.LicenseAssignment;
using WorkVista.API.RepositoryLayer.WorkVista.LicenseAssignment;

namespace WorkVista.API.ServiceLayer.WorkVista.LicenseAssignment
{
    public class LicenseAssignmentService : ILicenseAssignmentService
    {
        private readonly ILicenseAssignmentRepository _repository;

        public LicenseAssignmentService(ILicenseAssignmentRepository repository)
        {
            _repository = repository;
        }

        public List<LicenseAssignmentResponseModel> GetAll(out string errorMessage)
        {
            return _repository.GetAll(out errorMessage);
        }

        public LicenseAssignmentResponseModel GetById(int id, out string errorMessage)
        {
            return _repository.GetById(id, out errorMessage);
        }

        public int AddOrUpdate(LicenseAssignmentRequestModel request, out string errorMessage)
        {
            return _repository.AddOrUpdate(request, out errorMessage);
        }

        public bool Release(int id, DateTime releasedDate, int createdBy, out string errorMessage)
        {
            return _repository.Release(id, releasedDate, createdBy, out errorMessage);
        }

        public int Delete(DeleteRequest request, out string errorMessage)
        {
            return _repository.Delete(request, out errorMessage);
        }
    }
}
