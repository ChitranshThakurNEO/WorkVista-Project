using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterRating
{
    public class MasterRatingApplicationResponseModel : MasterAuditDto
    {
        public int MasterRatingApplicationId { get; set; }
        public int ActivityCategoryId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }
}
