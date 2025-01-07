public class Booking
{
    public string HotelId { get; set; }
    public string Arrival { get; set; }
    public string Departure { get; set; }
    public string RoomType { get; set; }
    public string RoomRate { get; set; }

    public DateTime GetArrivalDatetime()
    {
        return DateTimeConverter.StringToDatetime(Arrival);
    }
    public DateTime GetDepartureDatetime()
    {
        return DateTimeConverter.StringToDatetime(Departure);
    }
}