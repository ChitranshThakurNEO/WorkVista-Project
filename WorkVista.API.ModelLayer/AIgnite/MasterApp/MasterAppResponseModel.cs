using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.AIgnite.MasterApp
{
    public class MasterAppResponseModel : MasterAuditDto
    {
        public int MasterAppID { get; set; }
        public int MasterMenuID { get; set; }

        public string AppName { get; set; }
        public string ShortTitle { get; set; }
        public string AppDescription { get; set; }
        public string AppLogo { get; set; }
        public string AppLogoText { get; set; }
        public int Sequence { get; set; }

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
