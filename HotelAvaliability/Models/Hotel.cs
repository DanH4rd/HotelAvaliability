public class Hotel
{
    public class RoomType
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class Room
    {
        public string RoomType { get; set; }
        public string RoomId { get; set; }
    }

    public string Id { get; set; }
    public string Name { get; set; }

    public List<RoomType> RoomTypes { get; set; }
    public List<Room> Rooms { get; set; }
}