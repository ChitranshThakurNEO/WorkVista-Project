using WorkVista.API.ModelLayer.Common;

namespace WorkVista.API.ModelLayer.WorkVista.MasterEmployeeDEX
{
    public class MasterEmployeeDEXResponseModel : MasterAuditDto
    {
        public int MasterEmployeeDEXId { get; set; }
        public string NetworkId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public decimal? DailyCapacity { get; set; }
        public int? GeoLocationId { get; set; }
        public DateTime? SESSION_DATE { get; set; }

        public string MANAGER1 { get; set; }
        public string MANAGER1_EMPLOYEE_ID { get; set; }
        public string MANAGER2 { get; set; }
        public string MANAGER2_EMPLOYEE_ID { get; set; }

        public string CONSOLE_LOGIN_ID { get; set; }
        public string ROLE_NAME { get; set; }
        public string EMPLOYEE_ID { get; set; }

        public string DESIGNATION { get; set; }
        public string EMAIL_ID { get; set; }
        public string USER_GROUPS { get; set; }
        public string CLIENT { get; set; }
        public string PORTFOLIO { get; set; }
        public string GEO { get; set; }

        public DateTime? DATE_OF_JOINING { get; set; }
        public int? TENURE { get; set; }

        public string Department { get; set; }
    }
}
