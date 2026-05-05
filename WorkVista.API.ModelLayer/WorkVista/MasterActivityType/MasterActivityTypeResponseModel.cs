using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterActivityType
{
    public class MasterActivityTypeResponseModel : MasterAuditDto
    {
        public int MasterActivityTypeId { get; set; }

        public string Name { get; set; }

    }
}
