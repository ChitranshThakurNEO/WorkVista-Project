using WorkVista.API.HelperLayer.Constants;
using WorkVista.API.HelperLayer.Extensions;
using WorkVista.API.ModelLayer.WorkVista.Dashboard;
using WorkVista.API.RepositoryLayer.WorkVista.Dashboard;

namespace WorkVista.API.ServiceLayer.WorkVista.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;
        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }
        public ManagerDashboardSummaryResponseModel GetSummary( string managerEmployeeId, DateTime fromDate, DateTime toDate, out string errorMessage)
        {
            return _repository.GetSummary(
                managerEmployeeId,
                fromDate,
                toDate,
                out errorMessage);
        }

        public List<ManagerDashboardEmployeeGridResponseModel> GetGridData(string managerEmployeeId, DateTime fromDate, DateTime toDate, string teamLeadEmployeeId, string geo, string searchText, out string errorMessage)
        {
            var rawData = _repository.GetGridData(
                managerEmployeeId,
                fromDate,
                toDate,
                teamLeadEmployeeId,
                geo,
                searchText,
                out errorMessage);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return new List<ManagerDashboardEmployeeGridResponseModel>();
            }

            return rawData
                .GroupBy(x => new
                {
                    x.MasterEmployeeId,
                    x.FullName,
                    x.DESIGNATION,
                    x.GEO
                })
                .Select(group =>
                {
                    int totalMinutes = 0;
                    var dates = new Dictionary<string, string>();

                    foreach (var item in group.OrderBy(x => x.LoggedDate))
                    {
                        string value;

                        if (item.AttendanceStatus == DashboardConstants.AttendanceStatus.Leave)
                        {
                            value = DashboardConstants.DisplayValues.Leave;
                        }
                        else if (item.DayType == DashboardConstants.DayType.Weekend || item.AttendanceStatus == DashboardConstants.AttendanceStatus.Holiday)
                        {
                            value = DashboardConstants.DisplayValues.Holiday;
                        }
                        else
                        {
                            value = FormatDateExtensions.ConvertHoursToHHMM(item.TOT_ACTIVEHOURS);
                            totalMinutes += (int)(item.TOT_ACTIVEHOURS * 60);
                        }

                        dates[item.LoggedDate.ToString(DashboardConstants.DateFormats.GridDateFormat)] = value;
                    }

                    var presentDays = group.Count(x => x.AttendanceStatus == DashboardConstants.AttendanceStatus.Present);
                    var avgMinutes = presentDays == 0 ? 0 : totalMinutes / presentDays;

                    return new ManagerDashboardEmployeeGridResponseModel
                    {
                        EmployeeId = group.Key.MasterEmployeeId,
                        EmployeeName = group.Key.FullName,
                        Designation = group.Key.DESIGNATION,
                        Geo = group.Key.GEO,
                        Dates = dates,
                        Total = FormatDateExtensions.MinutesToHHMM(totalMinutes),
                        AveragePerDay = FormatDateExtensions.MinutesToHHMM(avgMinutes)
                    };
                })
                .ToList();
        }

        public EmployeeDayDetailsResponseModel GetEmployeeDayDetails(int employeeId, DateTime loggedDate, out string errorMessage)
        {
            return _repository.GetEmployeeDayDetails(
                employeeId,
                loggedDate,
                out errorMessage);
        }
    }
}
