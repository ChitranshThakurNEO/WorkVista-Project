using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Runtime;
using WorkVista.API.Areas.EmployeeShift;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ServiceLayer.WorkVista.Dashboard;
using WorkVista.API.ServiceLayer.WorkVista.EmployeeShift;

namespace WorkVista.API.Controllers.WorkVista
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _service;

        public DashboardController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<DashboardController> logger,
            IDashboardService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("GetManagerDashboardSummary")]
        public Envelope GetManagerDashboardSummary(string managerEmployeeId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var data = _service.GetSummary(
                    managerEmployeeId,
                    fromDate,
                    toDate,
                    out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new Envelope(false, null, errorMessage, 0);
                }

                return new Envelope(
                    true,
                    JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                    "Success",
                    1
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetManagerDashboardSummary");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        [HttpGet]
        [Route("GetManagerDashboardGridData")]
        public Envelope GetManagerDashboardGridData(string managerEmployeeId, DateTime fromDate, DateTime toDate, string teamLeadEmployeeId = null, string geo = null, string searchText = null)
        {
            try
            {
                var data = _service.GetGridData(
                    managerEmployeeId,
                    fromDate,
                    toDate,
                    teamLeadEmployeeId,
                    geo,
                    searchText,
                    out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new Envelope(false, null, errorMessage, 0);
                }

                return new Envelope(
                    true,
                    JsEncryption.EncryptStringAESToModel(data, _settings.JSEncryptionKey),
                    "Success",
                    1
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetManagerDashboardGridData");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        [HttpGet]
        [Route("GetEmployeeDayDetails")]
        public Envelope GetEmployeeDayDetails(int employeeId, DateTime loggedDate)
        {
            try
            {
                var data = _service.GetEmployeeDayDetails(
                    employeeId,
                    loggedDate,
                    out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new Envelope(false, null, errorMessage, 0);
                }

                return new Envelope(
                    true,
                    JsEncryption.EncryptStringAESToModel(
                        data,
                        _settings.JSEncryptionKey),
                    "Success",
                    1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetEmployeeDayDetails");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
    }
}
