using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSession;
using WorkVista.API.ModelLayer.WorkVista.MasterRating;
using WorkVista.API.ServiceLayer.WorkVista.MasterRating;

namespace WorkVista.API.Areas.MasterRatingApplication
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterRatingApplicationController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<MasterRatingApplicationController> _logger;
        private readonly IMasterRatingApplicationService _service;

        public MasterRatingApplicationController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<MasterRatingApplicationController> logger,
            IMasterRatingApplicationService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET
        [HttpGet]
        [Route("GetAllMasterRatingApplications")]
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
                _logger.LogError(ex, "GetAllMasterRatingApplications");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("GetMasterRatingApplicationById/{id}")]
        public Envelope GetById(int id)
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
                _logger.LogError(ex, "GetMasterRatingApplicationById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region ADD / UPDATE
        [HttpPost]
        [Route("AddOrUpdateMasterRatingApplication")]
        public Envelope AddOrUpdate([FromBody] MasterRatingApplicationRequestModel request)
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
                _logger.LogError(ex, "AddOrUpdateMasterRatingApplication");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region DELETE
        [HttpPost]
        [Route("DeleteMasterRatingApplication")]
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
                _logger.LogError(ex, "DeleteMasterRatingApplication");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region TOGGLE ACTIVE
        [HttpPost]
        [Route("ToggleMasterRatingApplicationActive")]
        public Envelope ToggleActive([FromBody] ToggleRequest request)
        {
            try
            {
                var result = _service.ToggleActive(
                    request.Id,
                    request.CreatedBy,
                    out string errorMessage
                );

                if (!result)
                    return new Envelope(false, null, errorMessage ?? "Failed", 0);

                return new Envelope(true, null, "Status updated successfully", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ToggleMasterRatingApplicationActive");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion
    }
}