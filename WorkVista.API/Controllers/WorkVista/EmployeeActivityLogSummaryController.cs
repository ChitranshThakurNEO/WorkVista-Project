using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterActivityLogSummary;
using WorkVista.API.ServiceLayer.WorkVista.MasterActivityLogSummary;

namespace WorkVista.API.Areas.EmployeeActivityLogSummary
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeActivityLogSummaryController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<EmployeeActivityLogSummaryController> _logger;
        private readonly IEmployeeActivityLogSummaryService _service;

        public EmployeeActivityLogSummaryController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<EmployeeActivityLogSummaryController> logger,
            IEmployeeActivityLogSummaryService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET ALL
        [HttpGet]
        [Route("GetAllEmployeeActivityLogSummaries")]
        public Envelope GetAll()
        {
            var data = _service.GetAll(out string errorMessage);

            return new Envelope(true,
                JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                "Success", 1);
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("GetEmployeeActivityLogSummaryById")]
        public Envelope GetById([FromBody] long id)
        {
            var data = _service.GetById(id, out string errorMessage);

            return new Envelope(true,
                JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                "Success", 1);
        }
        #endregion

        #region GET BY EMPLOYEE DATE
        [HttpPost]
        [Route("GetEmployeeActivityLogSummaryByDate")]
        public Envelope GetByEmployeeDate([FromBody] dynamic req)
        {
            var data = _service.GetByEmployeeDate(
                (int)req.EmployeeId,
                (DateTime)req.FromDate,
                (DateTime)req.ToDate,
                out string errorMessage);

            return new Envelope(true,
                JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                "Success", 1);
        }
        #endregion

        #region LOG THE ACTIVITY
        [HttpPost]
        [Route("AddEmployeeActivityLogSummary")]
        public Envelope Save([FromBody] EmployeeActivityLogSummaryRequestModel request)
        {
            var id = _service.AddOrUpdate(request, out string errorMessage);

            return new Envelope(true,
                JsEncryption.EncryptStringAESToModel(id, _settings.JSEncryptionKey),
                "Saved", 1);
        }
        #endregion

        #region DELETE
        [HttpPost]
        [Route("DeleteEmployeeActivityLogSummary")]
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
                _logger.LogError(ex, "DeleteEmployeeActivityLogSummary");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion
    }
}