using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterActivityCategory
{
    public class MasterActivityCategoryResponseModel : MasterAuditDto
    {
        public int MasterActivityCategoryId { get; set; }

        public int ActivityTypeId { get; set; }

        public string Geo { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }

        public string ProcessName { get; set; }
        public string SubCategoryName { get; set; }
        public string SubCategoryContent { get; set; }

        public string ColorHex { get; set; }
        public int SortOrder { get; set; }

        public bool IsProductive { get; set; }
        public bool IsNonProductive { get; set; }
        public bool IsNoImpact { get; set; }
    }
}
