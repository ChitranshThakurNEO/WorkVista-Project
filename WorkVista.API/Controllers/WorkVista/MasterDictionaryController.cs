using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterDictionary;
using WorkVista.API.ServiceLayer.WorkVista.MasterDictionary;

namespace WorkVista.API.Controllers.WorkVista
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterDictionaryController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<MasterDictionaryController> _logger;
        private readonly IMasterDictionaryService _service;

        public MasterDictionaryController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<MasterDictionaryController> logger,
            IMasterDictionaryService service)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _service = service;
        }
            
        #region GET ALL

        [HttpGet]
        [Route("GetAllMasterDictionaries")]
        public Envelope GetAll()
        {
            try
            {
                var data = _service.GetAll(out string errorMessage);

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
                _logger.LogError(ex, "MasterDictionary_GetAll");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion

        #region GET BY ID

        [HttpGet]
        [Route("GetMasterDictionaryDetailsById")]
        public Envelope GetById([FromBody] int id)
        {
            try
            {
                var data = _service.GetById(id, out string errorMessage);

                if (data == null)
                {
                    return new Envelope(false, null, "Record not found", 0);
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
                _logger.LogError(ex, "MasterDictionary_GetById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion

        #region ADD OR UPDATE

        [HttpPost]
        [Route("AddOrUpdateMasterDictionaryDetails")]
        public Envelope AddOrUpdate([FromBody] MasterDictionaryRequestModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Key) ||
                    string.IsNullOrWhiteSpace(request.Value))
                {
                    return new Envelope(false, null, "Key and Value are required", 0);
                }

                var id = _service.AddOrUpdate(request, out string errorMessage);

                if (id <= 0)
                {
                    return new Envelope(false, null, errorMessage ?? "Failed to save", 0);
                }

                return new Envelope(
                    true,
                    JsEncryption.EncryptStringAESToModel(id, _settings.JSEncryptionKey),
                    "Saved successfully",
                    1
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MasterDictionary_AddOrUpdate");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion

        #region DELETE

        [HttpPost]
        [Route("DeleteMasterDictionaryDetails")]
        public Envelope Delete([FromBody] DeleteRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return new Envelope(false, null, "Invalid Id", 0);
                }

                var result = _service.Delete(
                    request,
                    out string errorMessage
                );

                if (result <= 0)
                {
                    return new Envelope(false, null, errorMessage ?? "Delete failed", 0);
                }

                return new Envelope(true, null, "Deleted successfully", 1);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MasterDictionary_Delete");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion
    }
}
