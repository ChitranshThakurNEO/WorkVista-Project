using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeDEX;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.WorkVista.MasterEmployeeDEX
{
    public class MasterEmployeeDEXRepository : IMasterEmployeeDEXRepository
    {
        private readonly SqlDbUtilities _sqlDbUtilities;

        public MasterEmployeeDEXRepository(IOptionsMonitor<AppSettingsModel> options)
        {
            _sqlDbUtilities = new SqlDbUtilities(options.CurrentValue.ConnectingString);
        }

        public List<MasterEmployeeDEXResponseModel> GetAll(out string errorMessage)
        {
            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetAllMasterEmployeeDEX,
                new Dictionary<string, object>(),
                out errorMessage);

            return dt.ToList<MasterEmployeeDEXResponseModel>();
        }

        public MasterEmployeeDEXResponseModel GetById(int id, out string errorMessage)
        {
            var param = new Dictionary<string, object>
            {
                { "MasterEmployeeDEXId", id }
            };

            var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                WorkVistaStoreProcedures.WorkVista_GetMasterEmployeeDEXById,
                param,
                out errorMessage);

            if (dt.Rows.Count > 0)
                return dt.ToList<MasterEmployeeDEXResponseModel>().FirstOrDefault();

            return null;
        }
    }
}
