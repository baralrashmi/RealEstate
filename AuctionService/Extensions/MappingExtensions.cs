using AuctionService.DTOs;
using AuctionService.Model;

namespace AuctionService.Extensions
{
    public static class MappingExtensions
    {

        public static AuctionDTO ToAuctionDTO(this Auction auction)
        {
            return new AuctionDTO
            {
                Id = auction.Id,
                ReservePrice = auction.ReservePrice,
                Seller = auction.Seller,
                Winner = auction.Winner,
                SoldAmount = auction.SoldAmount,
                CurrentHighBid = auction.CurrentHighBid,
                Status = auction.Status,
                Title = auction.property.Title,
                Description = auction.property.Description,
                Address = auction.property.Address,
                City = auction.property.City,
                State = auction.property.State,
                zipCode = auction.property.zipCode,
                Country = auction.property.Country,
                Bedrooms = auction.property.Bedrooms,
                Bathrooms = auction.property.Bathrooms,
                AreaSqFt = auction.property.AreaSqFt,
                ImageUrl = auction.property.ImageUrl
            };
        }
    }
}
