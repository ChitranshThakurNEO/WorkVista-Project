using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeDEX;

namespace WorkVista.API.ServiceLayer.WorkVista.MasterEmployeeDEX
{
    public interface IMasterEmployeeDEXService
    {
        List<MasterEmployeeDEXResponseModel> GetAll(out string errorMessage);
        MasterEmployeeDEXResponseModel GetById(int id, out string errorMessage);
    }
}
