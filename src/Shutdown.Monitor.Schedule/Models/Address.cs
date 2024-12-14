namespace Shutdown.Monitor.Schedule.Models;

public readonly struct Address
{
    public Address(string street, string city, string house)
    {
        Street = street;
        City = city;
        House = house;
    }

    public string Street { get; }
    public string City { get; }
    public string House { get; }
    
}