namespace Dima.Core.Common;

public static class DateTimeExtension
{
    public static DateTime GetFirstDay(this DateTime date, int? year = null, int? month = null)
        => new(year ?? date.Year, month ?? date.Month, 1);
    
    public static DateTime GetLastDay(this DateTime date, int? year = null, int? month = null)
        => new(
            year ?? date.Year,
            month ?? date.Month,
            DateTime.DaysInMonth(year ?? date.Year, month ?? date.Month));
}