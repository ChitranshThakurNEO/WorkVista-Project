using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeDEX;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterEmployeeDEX
{
    public interface IMasterEmployeeDEXRepository
    {
        List<MasterEmployeeDEXResponseModel> GetAll(out string errorMessage);
        MasterEmployeeDEXResponseModel GetById(int id, out string errorMessage);
    }
}
