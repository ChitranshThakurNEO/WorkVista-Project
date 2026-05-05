using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterApplicationLicense;
using WorkVista.API.ServiceLayer.WorkVista.MasterApplicationLicense;

namespace WorkVista.API.Controllers.WorkVista
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterApplicationLicenseController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<MasterApplicationLicenseController> _logger;
        private readonly IMasterApplicationLicenseService _service;

        public MasterApplicationLicenseController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<MasterApplicationLicenseController> logger,
            IMasterApplicationLicenseService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET
        [HttpGet]
        [Route("GetAllMasterApplicationLicenses")]
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
                _logger.LogError(ex, "GetAllMasterApplicationLicenses");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("GetMasterApplicationLicenseById")]
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
                _logger.LogError(ex, "GetMasterApplicationLicenseById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region ADD / UPDATE
        [HttpPost]
        [Route("AddOrUpdateMasterApplicationLicense")]
        public Envelope AddOrUpdate([FromBody] MasterApplicationLicenseRequestModel request)
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
                _logger.LogError(ex, "AddOrUpdateMasterApplicationLicense");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region DELETE
        [HttpPost]
        [Route("DeleteMasterApplicationLicense")]
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
                _logger.LogError(ex, "DeleteMasterApplicationLicense");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion
    }
}
