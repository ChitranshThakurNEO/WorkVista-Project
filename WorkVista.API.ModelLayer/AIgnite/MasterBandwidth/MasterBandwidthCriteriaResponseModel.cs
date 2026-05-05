using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.AIgnite.MasterBandwidth
{
    public class MasterBandwidthCriteriaResponseModel : MasterAuditDto
    {
        public int MasterBandwidthCriteriaID { get; set; }

        public decimal DownloadMinMbps { get; set; }
        public decimal UploadMinMbps { get; set; }
        public int PingMaxMs { get; set; }
        public int JitterMaxMs { get; set; }

        //public string Col1 { get; set; }
        //public string Col2 { get; set; }
        //public string Col3 { get; set; }
        //public string Col4 { get; set; }
        //public string Col5 { get; set; }
        //public string Col6 { get; set; }
        //public string Col7 { get; set; }
        //public string Col8 { get; set; }
        //public string Col9 { get; set; }
        //public string Col10 { get; set; }
    }
}
