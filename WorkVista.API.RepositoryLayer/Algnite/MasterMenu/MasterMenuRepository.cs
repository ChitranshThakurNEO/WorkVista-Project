using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AIgnite.MasterMenu;
using WorkVista.API.ModelLayer.AppSettings;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.AIgnite.MasterMenu
{
    public class MasterMenuRepository : IMasterMenuRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;

        public MasterMenuRepository(IOptionsMonitor<AppSettingsModel> options)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
        }

        #region GET ALL MASTER MENU

        public List<MasterMenuResponseModel> GetAll(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                    AIgniteStoreProcedures.AIgnite_GetAllMasterMenu,
                    out errorMessage
                );

                if (dt == null)
                {
                    if (string.IsNullOrEmpty(errorMessage))
                        errorMessage = "Stored procedure returned no result.";
                    return new List<MasterMenuResponseModel>();
                }

                return dt.ToList<MasterMenuResponseModel>();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return new List<MasterMenuResponseModel>();
            }
        }

        #endregion
    }
}
