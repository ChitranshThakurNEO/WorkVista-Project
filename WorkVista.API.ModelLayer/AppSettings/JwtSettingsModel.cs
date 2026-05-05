namespace WorkVista.API.ModelLayer.AppSettings
{
    public class JwtSettingsModel
    {
        public JwtSettingsModel()
        {
            Key = string.Empty;
            Issuer = string.Empty;
            Audience = string.Empty;
            MinutesToExpiration = 1;
        }

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int MinutesToExpiration { get; set; }
    }
}
