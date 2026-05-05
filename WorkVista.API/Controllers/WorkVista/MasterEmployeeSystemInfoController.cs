using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSystemInfo;
using WorkVista.API.ServiceLayer.WorkVista.MasterEmployeeSystemInfo;

namespace WorkVista.API.Areas.MasterEmployeeSystemInfo
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterEmployeeSystemInfoController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<MasterEmployeeSystemInfoController> _logger;
        private readonly IMasterEmployeeSystemInfoService _service;

        public MasterEmployeeSystemInfoController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<MasterEmployeeSystemInfoController> logger,
            IMasterEmployeeSystemInfoService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET ALL
        [HttpGet]
        [Route("GetAllMasterEmployeeSystemInfos")]
        public Envelope GetAll()
        {
            try
            {
                var data = _service.GetAll(out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                    return new Envelope(false, null, errorMessage, 0);

                return new Envelope(true,
                    JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                    "Success", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllMasterEmployeeSystemInfos");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("GetMasterEmployeeSystemInfoById")]
        public Envelope GetById([FromBody] int id)
        {
            try
            {
                var data = _service.GetById(id, out string errorMessage);

                if (data == null)
                    return new Envelope(false, null, "Record not found", 0);

                return new Envelope(true,
                    JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                    "Success", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMasterEmployeeSystemInfoById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region ADD / UPDATE
        [HttpPost]
        [Route("AddOrUpdateMasterEmployeeSystemInfo")]
        public Envelope AddOrUpdate([FromBody] MasterEmployeeSystemInfoRequestModel request)
        {
            try
            {
                var id = _service.AddOrUpdate(request, out string errorMessage);

                if (id <= 0)
                    return new Envelope(false, null, errorMessage ?? "Failed", 0);

                return new Envelope(true,
                    JsEncryption.EncryptStringAESToModel(id, _settings.JSEncryptionKey),
                    "Saved successfully", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddOrUpdateMasterEmployeeSystemInfo");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region DELETE
        [HttpPost]
        [Route("DeleteMasterEmployeeSystemInfo")]
        public Envelope Delete([FromBody] DeleteRequest request)
        {
            try
            {
                var result = _service.Delete(
                    request,
                    out string errorMessage
                );

                if (result <= 0)
                    return new Envelope(false, null, errorMessage ?? "Delete failed", 0);

                return new Envelope(true, null, "Deleted successfully", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteMasterEmployeeSystemInfo");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion
    }
}