using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ServiceLayer.WorkVista.MasterEmployeeDEX;

namespace WorkVista.API.Areas.MasterEmployeeDEX
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterEmployeeDEXController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<MasterEmployeeDEXController> _logger;
        private readonly IMasterEmployeeDEXService _service;

        public MasterEmployeeDEXController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<MasterEmployeeDEXController> logger,
            IMasterEmployeeDEXService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }

        #region GET ALL
        [HttpGet]
        [Route("GetAllEmployees")]
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
                _logger.LogError(ex, "GetAllEmployees");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region GET BY ID
        [HttpGet]
        [Route("GetEmployeeById")]
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
                _logger.LogError(ex, "GetEmployeeById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion

        #region IMPORT EMPLOYEE DATA FROM DEX
        [HttpGet]
        [Route("ImportEmployeeFromDex")]
        public Envelope ImportEmployeeDataFromDex()
        {
            try
            {
                //logic to implement import functionality to fetch data from DEX
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ImportEmployeeFromDex");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }
        #endregion
    }
}