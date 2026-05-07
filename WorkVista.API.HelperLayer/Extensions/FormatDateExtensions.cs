namespace WorkVista.API.HelperLayer.Extensions
{
    public static class FormatDateExtensions
    {
        public static string ConvertHoursToHHMM(decimal hours)
        {
            var totalMinutes = (int)(hours * 60);
            return MinutesToHHMM(totalMinutes);
        }

        public static string MinutesToHHMM(int totalMinutes)
        {
            var hrs = totalMinutes / 60;
            var mins = totalMinutes % 60;

            return $"{hrs:D2}:{mins:D2}";
        }
    }
}
