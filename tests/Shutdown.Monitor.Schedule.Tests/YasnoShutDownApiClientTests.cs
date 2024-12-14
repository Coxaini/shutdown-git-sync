using Shutdown.Monitor.Schedule.Clients;

namespace Shutdown.Monitor.Schedule.Tests;

public class YasnoShutDownApiClientTests
{
   private readonly HttpClient _httpClient;

   public YasnoShutDownApiClientTests()
   {
      _httpClient = new HttpClient()
      {
         BaseAddress = new Uri("https://api.yasno.com.ua/api/v1/")
      };
   }
   
   [Fact]
   public async Task GetStreetsAsync_ShouldReturnStreets()
   {
      var client = new YasnoShutDownApiClient(_httpClient);
      var city = "kiev";
      
      var result = await client.GetStreetsAsync(city);
      
      Assert.NotNull(result);
      Assert.NotEmpty(result);
   }
   
   [Fact]
   public async Task GetHousesAsync_ShouldReturnHouses()
   {
      var client = new YasnoShutDownApiClient(_httpClient);
      var city = "kiev";
      var streetId = 1;
      
      var result = await client.GetHousesAsync(city, streetId);
      
      Assert.NotNull(result);
      Assert.NotEmpty(result);
   }
}