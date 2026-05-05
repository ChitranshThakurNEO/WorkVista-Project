using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ServiceLayer.AIgnite.MasterApp;

namespace WorkVista.API.Controllers.AIgnite
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIgniteMasterAppController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<AIgniteMasterAppController> _logger;
        private readonly IMasterAppService _service;

        public AIgniteMasterAppController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<AIgniteMasterAppController> logger,
            IMasterAppService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET ALL BY MENU ID

        [HttpGet]
        [Route("GetAll")]
        public Envelope GetAll([FromQuery] int menuId)
        {
            try
            {
                if (menuId <= 0)
                {
                    return new Envelope(false, null, "Invalid Menu Id", 0);
                }

                var data = _service.GetAllByMenuId(menuId, out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new Envelope(false, null, errorMessage, 0);
                }

                return new Envelope(true, data, "Success", data.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MasterApp_GetAllByMenuId");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion
    }
}
