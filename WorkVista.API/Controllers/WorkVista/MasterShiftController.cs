using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WorkVista.API.HelperLayer;
using WorkVista.API.ModelLayer;
using WorkVista.API.ModelLayer.AppSettings;
using WorkVista.API.ModelLayer.Common;
using WorkVista.API.ModelLayer.WorkVista.MasterShift;
using WorkVista.API.ServiceLayer.WorkVista.MasterShift;

namespace WorkVista.API.Areas.MasterShift
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterShiftController : ControllerBase
    {
        private readonly AppSettingsModel _settings;
        private readonly ILogger<MasterShiftController> _logger;
        private readonly IMasterShiftService _masterShiftService;

        public MasterShiftController(
            IOptionsMonitor<AppSettingsModel> options,
            ILogger<MasterShiftController> logger,
            IMasterShiftService masterShiftService)
        {
            _settings = options.CurrentValue;
            _logger = logger;
            _masterShiftService = masterShiftService;
        }

        #region GET ALL

        [HttpGet]
        [Route("GetAllMasterShifts")]
        public Envelope GetAllMasterShifts()
        {
            try
            {
                var data = _masterShiftService.GetAll(out string errorMessage);

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
                _logger.LogError(ex, "GetAllMasterShifts");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion

        #region GET BY ID

        [HttpGet]
        [Route("GetMasterShiftById")]
        public Envelope GetMasterShiftById([FromBody] int id)
        {
            try
            {
                var data = _masterShiftService.GetById(id, out string errorMessage);

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
                _logger.LogError(ex, "GetMasterShiftById");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion

        #region ADD OR UPDATE

        [HttpPost]
        [Route("AddOrUpdateMasterShift")]
        public Envelope AddOrUpdateMasterShift([FromBody] MasterShiftRequestModel request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Name))
                {
                    return new Envelope(false, null, "Shift name is required", 0);
                }

                var id = _masterShiftService.AddOrUpdate(request, out string errorMessage);

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
                _logger.LogError(ex, "AddOrUpdateMasterShift");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion

        #region DELETE

        [HttpPost]
        [Route("DeleteMasterShift")]
        public Envelope DeleteMasterShift([FromBody] DeleteRequest request)
        {
            try
            {
                if (request.Id <= 0)
                {
                    return new Envelope(false, null, "Invalid Shift Id", 0);
                }

                var result = _masterShiftService.Delete(
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
                _logger.LogError(ex, "DeleteMasterShift");
                return new Envelope(false, null, "Something went wrong", 0);
            }
        }

        #endregion
    }
}