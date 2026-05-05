using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.LicenseAssignment;

namespace WorkVista.API.RepositoryLayer.WorkVista.LicenseAssignment
{
    public interface ILicenseAssignmentRepository
    {
        List<LicenseAssignmentResponseModel> GetAll(out string errorMessage);
        LicenseAssignmentResponseModel GetById(int id, out string errorMessage);
        int AddOrUpdate(LicenseAssignmentRequestModel request, out string errorMessage);
        bool Release(int id, DateTime releasedDate, int createdBy, out string errorMessage);
        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
