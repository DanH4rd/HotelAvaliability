using System.Text.Json;
using System.Text.RegularExpressions;


var argDict = ArgumentsParser.Parse(args);

string jsonHotelString;
string jsonBookString;
using (StreamReader sr = File.OpenText(argDict["hotels"]))
{
    jsonHotelString = sr.ReadToEnd();
}
using (StreamReader sr = File.OpenText(argDict["bookings"]))
{
    jsonBookString = sr.ReadToEnd();
}

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
List<Hotel> hotelList =
                JsonSerializer.Deserialize<List<Hotel>>(jsonHotelString, options) ?? new List<Hotel>();
List<Booking> bookingList =
                JsonSerializer.Deserialize<List<Booking>>(jsonBookString, options) ?? new List<Booking>();


string queryTemplate = @"^\s*(\S+)\(\s*(\S+)\s*,\s*(\S+)\s*,\s*(\S+)\s*\)\s*$";
Regex queryRegex = new Regex(queryTemplate);

bool isWorking = true;
while (isWorking) 
{
    string input = Console.ReadLine() ?? "";

    if (input == "")
        isWorking = false;
    else 
    {
        if (queryRegex.IsMatch(input))
        {
            var matchResult = queryRegex.Match(input);
            Query query = new Query
            {
                QueryType = matchResult.Groups[1].Value,
                HotelId = matchResult.Groups[2].Value,
                TimeRange = matchResult.Groups[3].Value,
                RoomType = matchResult.Groups[4].Value,
            };

            if (query.QueryType == "Availability")
                Console.WriteLine(BookingAnalyser.GetAvaliableRoomsCount(query, hotelList, bookingList));
            else if (query.QueryType == "Reservation")
                Console.WriteLine(BookingAnalyser.GetReservedRoomsCount(query, bookingList));
            else
                Console.WriteLine($"Unsupported query type: {query.QueryType}");
        }
        else
            Console.WriteLine("Bad request");
    }
}





