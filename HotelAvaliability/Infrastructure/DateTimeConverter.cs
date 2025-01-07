
using System.Globalization;

public static class DateTimeConverter
{
    readonly public static string TIME_FORMAT = "yyyyMMdd";
    public static DateTime StringToDatetime(string dateString)
    {
        DateTime dt;
        if (DateTime.TryParseExact(dateString, TIME_FORMAT, CultureInfo.InvariantCulture,
                                   DateTimeStyles.None,
                                   out dt))
        {
            return dt;
        }
        else
        {
            throw new Exception($"Failed to parse date: {dateString}");
        }
    }
}