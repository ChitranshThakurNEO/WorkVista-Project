namespace WorkVista.API.ModelLayer.WorkVista.Dashboard
{
    public class EmployeeApplicationUsageResponseModel
    {
        public string ApplicationName { get; set; }

        public string ProcessName { get; set; }

        public int DurationSeconds { get; set; }

        public decimal DurationMinutes { get; set; }

        public decimal DurationHours { get; set; }

        public string ProductivityType { get; set; }

        public string ColorHex { get; set; }
    }
}
