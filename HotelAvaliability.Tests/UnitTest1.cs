using System.Text.Json;

namespace HotelAvaliability.Tests
{
    public class HotelDataFixture : IDisposable
    {
        public HotelDataFixture()
        {

            string jsonHotelString = "[ { \"id\": \"H1\", \"name\": \"Hotel California\", \"roomTypes\": [ { \"code\": \"SGL\", \"description\": \"Single Room\" }, { \"code\": \"DBL\", \"description\": \"Double Room\" } ], \"rooms\": [ { \"roomType\": \"SGL\", \"roomId\": \"101\" }, { \"roomType\": \"SGL\", \"roomId\": \"102\" }, { \"roomType\": \"DBL\", \"roomId\": \"201\" }, { \"roomType\": \"DBL\", \"roomId\": \"202\" } ] } ] ";

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            hotelList = JsonSerializer.Deserialize<List<Hotel>>(jsonHotelString, options) ?? new List<Hotel>();
        }

        public void Dispose()
        {
        }

        public List<Hotel> hotelList { get; private set; }
    }

    public class HotelAvaliabilityTests : IClassFixture<HotelDataFixture>
    {
        HotelDataFixture fixture;
        public HotelAvaliabilityTests(HotelDataFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void Deserialize_HotelData_ReturnsSameJsonstring()
        {

            string jsonHotelString1 = "[{\"Id\":\"H1\",\"Name\":\"Hotel California\",\"RoomTypes\":[{\"Code\":\"SGL\",\"Description\":\"Single Room\"},{\"Code\":\"DBL\",\"Description\":\"Double Room\"}],\"Rooms\":[{\"RoomType\":\"SGL\",\"RoomId\":\"101\"},{\"RoomType\":\"SGL\",\"RoomId\":\"102\"},{\"RoomType\":\"DBL\",\"RoomId\":\"201\"},{\"RoomType\":\"DBL\",\"RoomId\":\"202\"}]}]";

            List<Hotel> hotelList = JsonSerializer.Deserialize<List<Hotel>>(jsonHotelString1) ?? new List<Hotel>();

            string jsonHotelString2 = JsonSerializer.Serialize<List<Hotel>>(hotelList);

            Assert.Equal(jsonHotelString1, jsonHotelString2);
        }

        [Fact]
        public void Deserialize_BookingData_ReturnsSameJsonstring()
        {

            string jsonBookingsString1 = "[{\"HotelId\":\"H1\",\"Arrival\":\"20240901\",\"Departure\":\"20240903\",\"RoomType\":\"DBL\",\"RoomRate\":\"Prepaid\"},{\"HotelId\":\"H1\",\"Arrival\":\"20240902\",\"Departure\":\"20240905\",\"RoomType\":\"SGL\",\"RoomRate\":\"Standard\"}]";

            List<Booking> bookingList = JsonSerializer.Deserialize<List<Booking>>(jsonBookingsString1) ?? new List<Booking>();

            string jsonBookingsString2 = JsonSerializer.Serialize<List<Booking>>(bookingList);

            Assert.Equal(jsonBookingsString1, jsonBookingsString2);
        }

        [Fact]
        public void GetAvaliableRoomsCount_NothingReserved_ReturnsTwo()
        {

            Query query = new Query { QueryType = "None", HotelId = "H1", RoomType = "SGL", TimeRange = "20240202" };

            var result = BookingAnalyser.GetAvaliableRoomsCount(query, fixture.hotelList, new List<Booking>());

            Assert.Equal(2, result);
        }

        [Fact]
        public void GetAvaliableRoomsCount_OneReserved_ReturnsOne()
        {

            Query query = new Query { QueryType = "None", HotelId = "H1", RoomType = "SGL", TimeRange = "20240202" };

            List<Booking> bookings = new List<Booking> {
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240202", Departure = "20240202"}
            };

            var result = BookingAnalyser.GetAvaliableRoomsCount(query, fixture.hotelList, bookings);

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetAvaliableRoomsCount_TwoReserved_ReturnsZero()
        {

            Query query = new Query { QueryType = "None", HotelId = "H1", RoomType = "SGL", TimeRange = "20240202" };

            List<Booking> bookings = new List<Booking> {
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240202", Departure = "20240202"},
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240202", Departure = "20240202"},
            };

            var result = BookingAnalyser.GetAvaliableRoomsCount(query, fixture.hotelList, bookings);

            Assert.Equal(0, result);
        }

        [Fact]
        public void GetAvaliableRoomsCount_ThreeReserved_ReturnsMinusOne()
        {

            Query query = new Query { QueryType = "None", HotelId = "H1", RoomType = "SGL", TimeRange = "20240202" };

            List<Booking> bookings = new List<Booking> {
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240202", Departure = "20240202"},
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240202", Departure = "20240202"},
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240202", Departure = "20240202"},
            };

            var result = BookingAnalyser.GetAvaliableRoomsCount(query, fixture.hotelList, bookings);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void GetAvaliableRoomsCount_TwoReservedOutOfQueryRange_ReturnsTwo()
        {

            Query query = new Query { QueryType = "None", HotelId = "H1", RoomType = "SGL", TimeRange = "20240210-20240214" };

            List<Booking> bookings = new List<Booking> {
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240202", Departure = "20240209"},
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240215", Departure = "20240220"},
            };

            var result = BookingAnalyser.GetAvaliableRoomsCount(query, fixture.hotelList, bookings);

            Assert.Equal(2, result);
        }

        [Fact]
        public void GetAvaliableRoomsCount_TwoReservedOverlapEdgesOfQueryRange_ReturnsZero()
        {

            Query query = new Query { QueryType = "None", HotelId = "H1", RoomType = "SGL", TimeRange = "20240210-20240214" };

            List<Booking> bookings = new List<Booking> {
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240202", Departure = "20240210"},
                new Booking { HotelId="H1", RoomType="SGL", RoomRate = "None", Arrival = "20240214", Departure = "20240220"},
            };

            var result = BookingAnalyser.GetAvaliableRoomsCount(query, fixture.hotelList, bookings);

            Assert.Equal(0, result);
        }


    }
}
