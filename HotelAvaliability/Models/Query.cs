public class Query
{
    public string QueryType { get; set; }
    public string HotelId { get; set; }
    public string TimeRange { get; set; }
    public string RoomType { get; set; }

    public (DateTime, DateTime) GetTimeRangeDateTime()
    {
        var dateTokens = TimeRange.Split('-');
        var start = DateTimeConverter.StringToDatetime(dateTokens[0]);
        var finish = dateTokens.Length == 2 ? DateTimeConverter.StringToDatetime(dateTokens[1]) : start;

        return (start, finish);
    }
}