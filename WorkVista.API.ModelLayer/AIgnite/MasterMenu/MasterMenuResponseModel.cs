using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.AIgnite.MasterMenu
{
    public class MasterMenuResponseModel : MasterAuditDto
    {
        public int MasterMenuID { get; set; }

        public string CategoryName { get; set; }
        public string IconUrl { get; set; }
        public int Sequence { get; set; }
        public bool? IsMasterAppLinked { get; set; }

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
