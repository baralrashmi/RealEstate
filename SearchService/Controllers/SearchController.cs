using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SearchService.Data;
using SearchService.Model;
using SearchService.Params;
using System.Diagnostics.Eventing.Reader;
using System.Net.Quic;

namespace SearchService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly SearchDbContext _searchDbContext;

        //shortcut for constructor?
        //

        public SearchController(SearchDbContext searchDbContext)
        {
            _searchDbContext = searchDbContext;
        }

        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<List<SearchProperty>>> GetPropertySearchList([FromQuery] SearchParams searchParams)
        {
            var query = _searchDbContext.SearchProperties.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query = query.Where(x =>
                x.Title.Contains(searchParams.SearchTerm) ||
                x.Description.Contains(searchParams.SearchTerm)
                );

                query = searchParams.OrderBy switch
                {
                    "price" => query.OrderBy(x => x.ReservePrice),
                    "priceDesc" => query.OrderByDescending(x => x.ReservePrice),
                    "latest" => query.OrderByDescending(x => x.CreatedAt),
                    "bedrooms" => query.OrderByDescending(x => x.Bedrooms),
                    _ => query.OrderBy(x => x.CreatedAt)
                };

                query = searchParams.FilterBy switch
                {
                    "finished" => query.Where(x => x.AuctionEnd < DateTime.UtcNow),
                    "endingSoon" => query.Where(x => x.AuctionEnd < DateTime.UtcNow.AddHours(12)),
                    _ => query.OrderBy(x => x.CreatedAt)

                };
            }
            else
            {
                query = query.Where(x => x.AuctionEnd > DateTime.UtcNow);
            }



            //Add pagination filters. 
            var totalCount = await query.CountAsync();
            var items = await query.Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize).ToListAsync();
            var pageCount = (int)Math.Ceiling((double)totalCount / searchParams.PageSize);

            return Ok(new
            {
                results = items,
                pageCount,
                totalCount
            });

        }
    }
}
