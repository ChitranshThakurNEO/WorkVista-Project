using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AIgnite.MasterApp;
using WorkVista.API.ModelLayer.AppSettings;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.AIgnite.MasterApp
{
    public class MasterAppRepository : IMasterAppRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;

        public MasterAppRepository(IOptionsMonitor<AppSettingsModel> options)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
        }

        #region GET ALL MASTER APP BY MENU ID

        public List<MasterAppResponseModel> GetAllByMenuId(int masterMenuId, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "MasterMenuID", masterMenuId }
                };

                var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                    AIgniteStoreProcedures.AIgnite_GetAllMasterAppByMenuId,
                    parameters,
                    out errorMessage
                );

                if (dt == null)
                {
                    if (string.IsNullOrEmpty(errorMessage))
                        errorMessage = "Stored procedure returned no result.";
                    return new List<MasterAppResponseModel>();
                }

                return dt.ToList<MasterAppResponseModel>();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return new List<MasterAppResponseModel>();
            }
        }

        #endregion
    }
}
