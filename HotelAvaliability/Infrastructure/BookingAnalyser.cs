using System.Collections.Generic;

public static class BookingAnalyser 
{
    public static int? GetAvaliableRoomsCount(Query query, List<Hotel> hotelList, List<Booking> bookingList) 
    {
        var targetHotel = hotelList.FirstOrDefault(o => o?.Id == query.HotelId, null);

        if (targetHotel == null)
            return null;

        if (!targetHotel.RoomTypes.Any(rt => rt.Code == query.RoomType))
            return null;

        var fittableRooms = targetHotel.Rooms.FindAll(r => r.RoomType == query.RoomType);

        if (fittableRooms.Count == 0)
            return null;

        int avaliableRoomCount = fittableRooms.Count - GetReservedRoomsCount(query, bookingList);

        return avaliableRoomCount;
    }



    public static int GetReservedRoomsCount(Query query, List<Booking> bookingList) 
    {
        var hotelRoomBookings = bookingList.FindAll(b => b.HotelId == query.HotelId && b.RoomType == query.RoomType);

        if (hotelRoomBookings.Count > 0)
        {
            (DateTime queryBookStart, DateTime queryBookFinish) = query.GetTimeRangeDateTime();

            int reservedRoomsCount = hotelRoomBookings.Count(
                        o => !(queryBookStart > o.GetDepartureDatetime() || queryBookFinish < o.GetArrivalDatetime())
                        );

            return reservedRoomsCount;
        }
        else
            return 0;
    }
}