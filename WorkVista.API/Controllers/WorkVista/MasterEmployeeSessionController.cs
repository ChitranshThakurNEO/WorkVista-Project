using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterEmployeeSession;
using WorkVista.API.ServiceLayer.WorkVista.MasterEmployeeSession;

namespace WorkVista.API.Areas.MasterEmployeeSession
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterEmployeeSessionController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<MasterEmployeeSessionController> _logger;
        private readonly IMasterEmployeeSessionService _service;

        public MasterEmployeeSessionController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<MasterEmployeeSessionController> logger,
            IMasterEmployeeSessionService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET
        [HttpGet]
        [Route("GetAllMasterEmployeeSessions")]
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
                _logger.LogError(ex, "GetAllMasterEmployeeSessions");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("GetMasterEmployeeSessionById")]
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
                _logger.LogError(ex, "GetMasterEmployeeSessionById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region LOGIN
        [HttpPost]
        [Route("LoginEmployeeSession")]
        public Envelope Login([FromBody] MasterEmployeeSessionLoginRequestModel request)
        {
            try
            {
                var id = _service.Login(request, out string errorMessage);

                if (id <= 0)
                    return new Envelope(false, null, errorMessage ?? "Login failed", 0);

                return new Envelope(true,
                    JsEncryption.EncryptStringAESToModel(id, _settings.JSEncryptionKey),
                    "Login successful", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoginEmployeeSession");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region LOGOUT
        [HttpPost]
        [Route("LogoutEmployeeSession")]
        public Envelope Logout([FromBody] LogoutRequest request)
        {
            try
            {
                var result = _service.Logout(
                    request.Id,
                    request.LogoutTime,
                    request.CreatedBy,
                    out string errorMessage
                );

                if (!result)
                    return new Envelope(false, null, errorMessage ?? "Logout failed", 0);

                return new Envelope(true, null, "Logout successful", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogoutEmployeeSession");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region DELETE
        [HttpPost]
        [Route("DeleteMasterEmployeeSession")]
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
                _logger.LogError(ex, "DeleteMasterEmployeeSession");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region TOGGLE ACTIVE
        [HttpPost]
        [Route("ToggleMasterEmployeeSessionActive")]
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
                    return new Envelope(false, null, errorMessage ?? "Operation failed", 0);

                return new Envelope(true, null, "Status updated successfully", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ToggleMasterEmployeeSessionActive");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion
    }
}