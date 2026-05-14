using AuctionService.Data;
using AuctionService.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace AuctionService.Repositories
{
    public class AuctionRepository(RealEstateContext realEstateContext) : IAuctionRepository
    {
        public Task<ActionResult<AuctionDTO>> CreateAuction(AuctionDTO auctionDto)
        {
            throw new NotImplementedException();
        }

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

        public async Task<ActionResult<AuctionDTO>> GetAuctionByIdAsync(Guid id)
        {
            var query = await  realEstateContext.auctions
                .Include(x => x.property)
                .Where(a => a.Id == id)
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
                .FirstOrDefaultAsync();
            return query != null ? new ActionResult<AuctionDTO>(query) : new NotFoundResult();
        }
    }
}
