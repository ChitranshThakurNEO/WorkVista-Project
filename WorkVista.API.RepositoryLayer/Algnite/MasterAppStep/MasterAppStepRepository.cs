using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AIgnite;
using WorkVista.API.ModelLayer.AIgnite.MasterAppStep;
using WorkVista.API.ModelLayer.AppSettings;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.AIgnite.MasterAppStep
{
    public class MasterAppStepRepository : IMasterAppStepRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;

        public MasterAppStepRepository(IOptionsMonitor<AppSettingsModel> options)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
        }

        #region GET ALL MASTER APP STEP BY MASTER APP ID

        public List<MasterAppStepResponseModel> GetAllByMasterAppId(int masterAppId, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "MasterAppID", masterAppId }
                };

                var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                    AIgniteStoreProcedures.AIgnite_GetAllMasterAppStepByMasterAppId,
                    parameters,
                    out errorMessage
                );

                if (dt == null)
                {
                    if (string.IsNullOrEmpty(errorMessage))
                        errorMessage = "Stored procedure returned no result.";
                    return new List<MasterAppStepResponseModel>();
                }

                return dt.ToList<MasterAppStepResponseModel>();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return new List<MasterAppStepResponseModel>();
            }
        }

        #endregion
    }
}
