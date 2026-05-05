using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ServiceLayer.AIgnite.MasterMenu;

namespace WorkVista.API.Controllers.AIgnite
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIgniteMasterMenuController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<AIgniteMasterMenuController> _logger;
        private readonly IMasterMenuService _service;

        public AIgniteMasterMenuController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<AIgniteMasterMenuController> logger,
            IMasterMenuService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET ALL

        [HttpGet]
        [Route("GetAll")]
        public Envelope GetAll()
        {
            try
            {
                var data = _service.GetAll(out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new Envelope(false, null, errorMessage, 0);
                }

                return new Envelope(true, data, "Success", data.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MasterMenu_GetAll");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion
    }
}
