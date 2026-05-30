using SearchService.Data;
using SearchService.Model;
using System.Text.Json;

namespace SearchService.Services
{
    // This class is responsible for communicating with the AuctionService via HTTP.
    //http
    public class AuctionServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly SearchDbContext _dbContext;

        public AuctionServiceHttpClient(HttpClient httpClient, IConfiguration config, SearchDbContext dbContext)
        {
            _httpClient = httpClient;
            _config = config;
            _dbContext = dbContext;
        }

        public async Task<List<SearchProperty>> GetItemsFromAuctionDb()
        {
            var auctionServiceUrl = _config["AuctionServiceUrl"];
            var response = await _httpClient.GetAsync($"{auctionServiceUrl}/api/auctions");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var auctionItems = JsonSerializer.Deserialize<List<SearchProperty>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return auctionItems ?? new List<SearchProperty>();
            }
            else
            {
                // Log the error or handle it as needed
                throw new Exception($"Failed to retrieve items from AuctionService. Status Code: {response.StatusCode}");
            }
        }
    }
}
