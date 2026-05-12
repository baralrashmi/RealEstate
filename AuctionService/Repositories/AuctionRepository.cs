using AuctionService.Data;
using AuctionService.DTOs;
using System.Data.Entity;

namespace AuctionService.Repositories
{
    public class AuctionRepository(RealEstateContext realEstateContext) : IAuctionRepository
    {

        public async Task<List<AuctionDTO>> GetAuctionAsync()
        {
            var auctions = await realEstateContext.auctions
                .Include(x => x.property)
                .OrderBy(x => x.property.Title)
                .Select(a => new AuctionDTO
                {
                    Id = a.Id,
                    ReservePrice = a.ReservePrice,
                    Seller = a.Seller,
                    Winner = a.Winner,
                    SoldAmount = a.SoldAmount,
                    CurrentHighBid = a.CurrentHighBid,
                    Status = a.Status,
                    Title = a.property.Title,
                    Description = a.property.Description,
                    Address = a.property.Address,
                    City = a.property.City,
                    State = a.property.State,
                    zipCode = a.property.zipCode,
                    Country = a.property.Country,
                    Bedrooms = a.property.Bedrooms,
                    Bathrooms = a.property.Bathrooms,
                    AreaSqFt = a.property.AreaSqFt,
                    ImageUrl = a.property.ImageUrl
                })
                .ToListAsync();

            return auctions;
        }
    }
}
