namespace WorkVista.API.ModelLayer.Common
{
    public class DeleteRequest
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public int DeletedBy { get; set; }
    }

    public class DeleteRequestLong
    {
        public long Id { get; set; }
        public string Reason { get; set; }
        public int DeletedBy { get; set; }
    }
}
