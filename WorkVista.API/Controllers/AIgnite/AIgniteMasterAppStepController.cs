using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ServiceLayer.AIgnite.MasterAppStep;

namespace WorkVista.API.Controllers.AIgnite
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIgniteMasterAppStepController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<AIgniteMasterAppStepController> _logger;
        private readonly IMasterAppStepService _service;

        public AIgniteMasterAppStepController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<AIgniteMasterAppStepController> logger,
            IMasterAppStepService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET ALL BY MASTER APP ID

        [HttpGet]
        [Route("GetAll")]
        public Envelope GetAll([FromQuery] int masterAppId)
        {
            try
            {
                if (masterAppId <= 0)
                {
                    return new Envelope(false, null, "Invalid Master App Id", 0);
                }

                var data = _service.GetAllByMasterAppId(masterAppId, out string errorMessage);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    return new Envelope(false, null, errorMessage, 0);
                }

                return new Envelope(true, data, "Success", data.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MasterAppStep_GetAllByMasterAppId");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion
    }
}
