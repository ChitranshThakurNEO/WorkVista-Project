using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterDictionary
{
    public class MasterDictionaryResponseModel : MasterAuditDto
    {
        public int MasterDictionaryId { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }

        public string Geo { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string SubProcess { get; set; }
    }
}
