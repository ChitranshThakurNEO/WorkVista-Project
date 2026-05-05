namespace WorkVista.API.ModelLayer.WorkVista.MasterRating
{
    public class MasterRatingApplicationRequestModel
    {
        public int? MasterRatingApplicationId { get; set; }
        public int ActivityCategoryId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int CreatedBy { get; set; }
    }
}
