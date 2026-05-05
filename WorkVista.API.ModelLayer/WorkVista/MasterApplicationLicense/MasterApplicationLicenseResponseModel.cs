using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterApplicationLicense
{
    public class MasterApplicationLicenseResponseModel : MasterAuditDto
    {
        public int MasterApplicationLicenseId { get; set; }
        public int ActivityCategoryId { get; set; }
        public int TotalLicenses { get; set; }
        public decimal CostPerLicense { get; set; }
        public string BillingCycle { get; set; }
    }
}
