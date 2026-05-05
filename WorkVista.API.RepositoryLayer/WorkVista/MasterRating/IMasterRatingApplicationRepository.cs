using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterRating;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterRatingApplication
{
    public interface IMasterRatingApplicationRepository
    {
        List<MasterRatingApplicationResponseModel> GetAll(out string errorMessage);

        MasterRatingApplicationResponseModel GetById(int id, out string errorMessage);

        int AddOrUpdate(MasterRatingApplicationRequestModel request, out string errorMessage);

        int Delete(DeleteRequest deleteRequest, out string errorMessage);

        bool ToggleActive(int id, int createdBy, out string errorMessage);
    }
}