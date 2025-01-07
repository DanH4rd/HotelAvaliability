# HotelAvaliability

This project implements a console application that is able to analyze hotel booking data and return asked information to a user.

# Build and Run instructions

* Open it in Visual Studio 2022 (.NET 9) 
* Select HotelAvaliability as the main project
* Press F5 to run the application 


By default the application starts with these command line arguments `--hotels hotels.json --bookings bookings.json` which are set in Debug properties and refers to files from the `\data` folder. You may customise it as needed.

# Usage

To start application you need to provide paths to hotel and booking data files

```
HotelAvaliability.exe --hotels hotels.json --bookings bookings.json 
```

Commands to the app are given according to the corresponding template

```[CommandName]([HotelId], [TimeRange], [RoomType])```

[CommandName] - name of the command:
    Availability - shows avaliable rooms count for the given data
    Reservation - shows number of reservations for the given data

[HotelId] - id of the hotel

[TimeRange] - a date range given in either `yyyyMMdd` or `yyyyMMdd-yyyyMMdd` format.

[RoomType] - room type code used by the specified hotel

Example commands:

```
Availability(H1, 20240901, SGL) 
Availability(H1, 20240901-20240903, DBL)
Reservation(H1, 20241005-20241007, SGL)
```

Application terminates when user inputs an empty string.

# Tests

Project includes basic Unit tests to ensure the app works correctly.