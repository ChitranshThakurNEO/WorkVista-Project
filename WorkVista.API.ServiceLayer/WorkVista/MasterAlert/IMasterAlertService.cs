using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterAlert;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterAlert
{
    public interface IMasterAlertService
    {
        List<MasterAlertResponseModel> GetAll(out string errorMessage);

        MasterAlertResponseModel GetById(long id, out string errorMessage);

        long AddOrUpdate(MasterAlertRequestModel request, out string errorMessage);

        bool MarkAsRead(long id, int createdBy, out string errorMessage);

        bool Resolve(long id, int createdBy, out string errorMessage);

        int Delete(DeleteRequestLong request, out string errorMessage);
    }
}
