using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.LicenseAssignment;

namespace WorkVista.API.ServiceLayer.WorkVista.LicenseAssignment
{
    public interface ILicenseAssignmentService
    {
        List<LicenseAssignmentResponseModel> GetAll(out string errorMessage);
        LicenseAssignmentResponseModel GetById(int id, out string errorMessage);
        int AddOrUpdate(LicenseAssignmentRequestModel request, out string errorMessage);
        bool Release(int id, DateTime releasedDate, int createdBy, out string errorMessage);
        int Delete(DeleteRequest deleteRequest, out string errorMessage);
    }
}
