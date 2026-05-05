using Microsoft.Extensions.Options;
using WorkVista.API.DBManager.SQLHelper;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.AIgnite.MasterBandwidth;
using WorkVista.API.ModelLayer.AppSettings;
using static WorkVista.API.HelperLayer.Constants.SqlMetaData;

namespace WorkVista.API.RepositoryLayer.AIgnite.MasterBandwidth
{
    public class MasterBandwidthCriteriaRepository : IMasterBandwidthCriteriaRepository
    {
        private readonly AppSettingsModel _appSettingsModel;
        private readonly SqlDbUtilities _sqlDbUtilities;

        public MasterBandwidthCriteriaRepository(IOptionsMonitor<AppSettingsModel> options)
        {
            _appSettingsModel = options.CurrentValue;
            _sqlDbUtilities = new SqlDbUtilities(_appSettingsModel.ConnectingString);
        }

        #region GET ALL BANDWIDTH CRITERIA

        public List<MasterBandwidthCriteriaResponseModel> GetAll(out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                var dt = _sqlDbUtilities.ExectuteStoredProcedure(
                    AIgniteStoreProcedures.AIgnite_GetAllBandwidthCriteria,
                    out errorMessage
                );

                if (dt == null)
                {
                    if (string.IsNullOrEmpty(errorMessage))
                        errorMessage = "Stored procedure returned no result.";
                    return new List<MasterBandwidthCriteriaResponseModel>();
                }

                return dt.ToList<MasterBandwidthCriteriaResponseModel>();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return new List<MasterBandwidthCriteriaResponseModel>();
            }
        }

        #endregion
    }
}
