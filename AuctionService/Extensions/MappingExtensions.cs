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

        public static Auction ToAuctionEntity(this CreateAuctionDTO createAuctionDto)
        {
            return new Auction
            {

                ReservePrice = createAuctionDto.ReservePrice,
                AuctionEnd = createAuctionDto.AuctionEnd,
                Seller = "DemoSeller", // In a real application, this would come from the authenticated user context
                property = new Property
                {
                    Title = createAuctionDto.property.Title,
                    Description = createAuctionDto.property.Description,
                    Address = createAuctionDto.property.Address,
                    City = createAuctionDto.property.City,
                    State = createAuctionDto.property.State,
                    zipCode = createAuctionDto.property.zipCode,
                    Country = createAuctionDto.property.Country,
                    Bedrooms = createAuctionDto.property.Bedrooms,
                    Bathrooms = createAuctionDto.property.Bathrooms,
                    AreaSqFt = createAuctionDto.property.AreaSqFt,
                    ImageUrl = createAuctionDto.property.ImageUrl
                }

            };
        }


    }
}
