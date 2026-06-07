using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SearchService.Data;
using SearchService.Model;

namespace SearchService.Service
{
    public class AuctionServiceHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly SearchDbContext _dbContext;

        public AuctionServiceHttpClient(HttpClient httpClient, IConfiguration configuration, SearchDbContext dbContext)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        //Get data from search service using http client
        public HttpClient GetHttpClient()
        {
            var baseUrl = _configuration.GetValue<string>("AuctionService:baseUrl");
            _httpClient.BaseAddress = new Uri(baseUrl);
            return _httpClient;
        }

        //Get auction data from auction service and insert into database
        public async Task<List<SearchProperty>> GetItemsforSearchAsync()
        {
            //Check last updated date and filter the record based on updated date
            //Get the date
            var lastupdated = await _dbContext.SearchProperties
                .OrderByDescending(x => x.UpdatedAt)
                .Select(x => x.UpdatedAt.ToString())
                .FirstOrDefaultAsync();

            var baseUrl = _configuration.GetValue<string>("AuctionService:baseUrl");
            string url = baseUrl+ "/api/auctions";

            if (!string.IsNullOrEmpty(lastupdated))
            {
                url += $"?date={lastupdated}";
            }

            // Make HTTP GET request to auction service

            var items = await _httpClient.GetFromJsonAsync<List<SearchProperty>>(url);



            // Convert response to list of SearchProperty
            // List<SearchProperty> items = new List<SearchProperty>();
            //items = await response.Content.ReadFromJsonAsync<List<SearchProperty>>() ?? new List<SearchProperty>();

            // Insert data into database
            foreach (var item in items)
            {
                _dbContext.Add(item);
            }
            _dbContext.SaveChanges();

            return items;


        }




    }
}
