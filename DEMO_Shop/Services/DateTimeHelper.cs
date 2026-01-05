namespace DEMO_Shop.Services
{
    public static class DateTimeHelper
    {
        public static DateTime VietNameNow()
        {
            var vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
        }
    }
}
