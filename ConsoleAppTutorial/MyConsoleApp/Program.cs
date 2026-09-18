Console.WriteLine("Hello, World!");
Console.WriteLine($"The current time is {DateTime.Now}.");

// Get days until Christmas given a certain date
static int DaysUntilChristmas(DateTime date)
{
    int year = date > new DateTime(date.Year, 12, 25) ? date.Year + 1 : date.Year;
    DateTime christmas = new(year, 12, 25);
    var span = christmas - date;
    int days = span.Days;
    return days;
}

Console.WriteLine($"The next Christmas is in {DaysUntilChristmas(DateTime.Now)} days.");