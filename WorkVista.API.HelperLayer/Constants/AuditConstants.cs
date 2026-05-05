namespace WorkVista.API.HelperLayer.Constants
{
    public class AuditConstants
    {   
        public static class Actions
        {
            public const string Insert = "Insert";
            public const string Update = "Update";
            public const string Error = "Error";
            public const string Delete = "Delete";
        }

        public static class Entities
        {
            public const string MasterDictionary = "MasterDictionary";
            public const string MasterAlert = "MasterAlert";
            public const string MasterShift = "MasterShift";
            public const string EmployeeShift = "EmployeeShift";
            public const string MasterActivityType = "MasterActivityType";
            public const string MasterActivityCategory = "MasterActivityCategory";
            public const string MasterActivityLog = "MasterActivityLog";
            public const string EmployeeActivityLogSummary = "EmployeeActivityLogSummary";
            public const string MasterApplicationLicense = "MasterApplicationLicense";
            public const string LicenseAssignment = "LicenseAssignment";
            public const string MasterEmployeeSystemInfo = "MasterEmployeeSystemInfo";
            public const string MasterEmployeeSession = "MasterEmployeeSession";
            public const string MasterRatingApplication = "MasterRatingApplication";
        }
    }
}
