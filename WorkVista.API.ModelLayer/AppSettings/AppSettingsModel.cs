namespace WorkVista.API.ModelLayer.AppSettings
{
    public class AppSettingsModel
    {
        public AppSettingsModel()
        {
            ConnectingString = string.Empty;
            JSEncryptionKey = string.Empty;
            AssessmentJSEncryptionKey = string.Empty;
            EnableCorsOriginsAllowed = string.Empty;
            WebBaseURL = string.Empty;
            LogoPath = string.Empty;
            AssessmentLink = string.Empty;
            JWTSettings = new();
            SMTPDetailsURL = string.Empty;
            SendMail = string.Empty;
            IsSGLCCLiveMail = string.Empty;
            SendCandidatemail = string.Empty;
        }

        public string ConnectingString { get; set; }
        public string JSEncryptionKey { get; set; }
        public string AssessmentJSEncryptionKey { get; set; }
        public string EnableCorsOriginsAllowed { get; set; }
        public string WebBaseURL { get; set; }
        public string LogoPath { get; set; }
        public string AssessmentLink { get; set; }
        public JwtSettingsModel JWTSettings { get; set; }
        public string SMTPDetailsURL { get; set; }
        public string SendMail { get; set; }
        public string IsSGLCCLiveMail { get; set; }
        public string SendCandidatemail { get; set; }
    }
}
