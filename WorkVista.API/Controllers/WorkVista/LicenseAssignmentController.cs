using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.LicenseAssignment;
using WorkVista.API.ServiceLayer.WorkVista.LicenseAssignment;

namespace WorkVista.API.Areas.LicenseAssignment
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseAssignmentController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<LicenseAssignmentController> _logger;
        private readonly ILicenseAssignmentService _service;

        public LicenseAssignmentController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<LicenseAssignmentController> logger,
            ILicenseAssignmentService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET
        [HttpGet]
        [Route("GetAllLicenseAssignments")]
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
                _logger.LogError(ex, "GetAllLicenseAssignments");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("GetLicenseAssignmentById")]
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
                _logger.LogError(ex, "GetLicenseAssignmentById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region ADD / UPDATE
        [HttpPost]
        [Route("AddOrUpdateLicenseAssignment")]
        public Envelope AddOrUpdate([FromBody] LicenseAssignmentRequestModel request)
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
                _logger.LogError(ex, "AddOrUpdateLicenseAssignment");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region RELEASE
        [HttpPost]
        [Route("ReleaseLicenseAssignment")]
        public Envelope Release([FromBody] LicenseAssignmentReleaseRequest request)
        {
            try
            {
                var result = _service.Release(
                    request.LicenseAssignmentId,
                    request.ReleasedDate,
                    request.CreatedBy,
                    out string errorMessage
                );

                if (!result)
                    return new Envelope(false, null, errorMessage ?? "Release failed", 0);

                return new Envelope(true, null, "Released successfully", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReleaseLicenseAssignment");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region DELETE
        [HttpPost]
        [Route("DeleteLicenseAssignment")]
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
                _logger.LogError(ex, "DeleteLicenseAssignment");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion
    }
}