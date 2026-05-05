using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.AIgnite.MasterAppStep
{
    public class MasterAppStepResponseModel : MasterAuditDto
    {
        public int MasterAppStepID { get; set; }
        public int MasterAppID { get; set; }

        public string StepTitle { get; set; }
        public string ShortTitle { get; set; }
        public string StepDescription { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public int Sequence { get; set; }
        public string SequenceColorCode { get; set; }

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
