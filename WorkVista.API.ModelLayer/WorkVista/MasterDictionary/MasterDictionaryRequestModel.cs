namespace WorkVista.API.ModelLayer.WorkVista.MasterDictionary
{
    public class MasterDictionaryRequestModel
    {
        public int? MasterDictionaryId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Geo { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string SubProcess { get; set; }
        public int CreatedBy { get; set; }
    }
}
