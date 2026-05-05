using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterAlert;
using WorkVista.API.ServiceLayer.WorkVista.MasterAlert;

namespace WorkVista.API.Areas.MasterAlert
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterAlertController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<MasterAlertController> _logger;
        private readonly IMasterAlertService _service;

        public MasterAlertController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<MasterAlertController> logger,
            IMasterAlertService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET ALL
        [HttpGet]
        [Route("GetAllMasterAlerts")]
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
                _logger.LogError(ex, "GetAllMasterAlerts");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("GetMasterAlertById")]
        public Envelope GetById([FromBody] long id)
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
                _logger.LogError(ex, "GetMasterAlertById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region ADD / UPSERT
        [HttpPost]
        [Route("AddOrUpdateMasterAlert")]
        public Envelope AddOrUpdate([FromBody] MasterAlertRequestModel request)
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
                _logger.LogError(ex, "AddOrUpdateMasterAlert");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region MARK AS READ
        [HttpPost]
        [Route("MarkMasterAlertAsRead")]
        public Envelope MarkAsRead([FromBody] MasterAlertActionRequestModel request)
        {
            try
            {
                var result = _service.MarkAsRead(
                    request.MasterAlertId,
                    request.CreatedBy,
                    out string errorMessage
                );

                if (!result)
                    return new Envelope(false, null, errorMessage ?? "Failed", 0);

                return new Envelope(true, null, "Marked as read", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MarkMasterAlertAsRead");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region RESOLVE
        [HttpPost]
        [Route("ResolveMasterAlert")]
        public Envelope Resolve([FromBody] MasterAlertActionRequestModel request)
        {
            try
            {
                var result = _service.Resolve(
                    request.MasterAlertId,
                    request.CreatedBy,
                    out string errorMessage
                );

                if (!result)
                    return new Envelope(false, null, errorMessage ?? "Failed", 0);

                return new Envelope(true, null, "Resolved successfully", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ResolveMasterAlert");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region DELETE
        [HttpPost]
        [Route("DeleteMasterAlert")]
        public Envelope Delete([FromBody] DeleteRequestLong request)
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
                _logger.LogError(ex, "DeleteMasterAlert");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion
    }
}