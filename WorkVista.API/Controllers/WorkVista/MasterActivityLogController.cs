using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityLog;
using WorkVista.API.ServiceLayer.WorkVista.MasterActivityLog;

[Route("api/[controller]")]
[ApiController]
public class MasterActivityLogController : ControllerBase
{
    private readonly AppSettingsModel _settings;
    private readonly ILogger<MasterActivityLogController> _logger;
    private readonly IMasterActivityLogService _service;

    public MasterActivityLogController(
        IOptionsMonitor<AppSettingsModel> options,
        ILogger<MasterActivityLogController> logger,
        IMasterActivityLogService service)
    {
        _settings = options.CurrentValue;
        _logger = logger;
        _service = service;
    }

    #region GET ALL
    [HttpGet]
    [Route("GetAllMasterActivityLogs")]
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
            _logger.LogError(ex, "GetAllMasterActivityLogs");
            return new Envelope(false, null, "Something went wrong", 0);
        }
    }
    #endregion

    #region GET BY ID
    [HttpGet]
    [Route("GetMasterActivityLogById")]
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
            _logger.LogError(ex, "GetMasterActivityLogById");
            return new Envelope(false, null, "Something went wrong", 0);
        }
    }
    #endregion

    #region GET BY EMPLOYEE
    [HttpGet]
    [Route("GetLogsByEmployee")]
    public Envelope GetByEmployee([FromBody] int employeeId)
    {
        try
        {
            var data = _service.GetByEmployee(employeeId, out string errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
                return new Envelope(false, null, errorMessage, 0);

            return new Envelope(true,
                JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                "Success", 1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetLogsByEmployee");
            return new Envelope(false, null, "Something went wrong", 0);
        }
    }
    #endregion

    #region GET BY EMPLOYEE + DATE
    [HttpGet]
    [Route("GetLogsByEmployeeAndDate")]
    public Envelope GetByEmployeeAndDate(int employeeId, DateTime logDate)
    {
        try
        {
            var data = _service.GetByEmployeeAndDate(employeeId, logDate, out string errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
                return new Envelope(false, null, errorMessage, 0);

            return new Envelope(true,
                JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                "Success", 1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetLogsByEmployeeAndDate");
            return new Envelope(false, null, "Something went wrong", 0);
        }
    }
    #endregion

    #region START
    [HttpPost]
    [Route("StartActivity")]
    public Envelope Start([FromBody] StartActivityRequestModel request)
    {
        try
        {
            var id = _service.StartActivity(request, out string errorMessage);

            if (id <= 0)
                return new Envelope(false, null, errorMessage ?? "Failed", 0);

            return new Envelope(true,
                JsEncryption.EncryptStringAESToModel(id, _settings.JSEncryptionKey),
                "Started successfully", 1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "StartActivity");
            return new Envelope(false, null, "Something went wrong", 0);
        }
    }
    #endregion

    #region STOP
    [HttpPost]
    [Route("StopActivity")]
    public Envelope Stop([FromBody] StopActivityRequestModel request)
    {
        try
        {
            var result = _service.StopActivity(request, out string errorMessage);

            if (!result)
                return new Envelope(false, null, errorMessage ?? "Failed", 0);

            return new Envelope(true, null, "Stopped successfully", 1);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "StopActivity");
            return new Envelope(false, null, "Something went wrong", 0);
        }
    }
    #endregion

    #region DELETE
    [HttpPost]
    [Route("DeleteMasterActivityLog")]
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
            _logger.LogError(ex, "DeleteMasterActivityLog");
            return new Envelope(false, null, "Something went wrong", 0);
        }
    }
    #endregion
}