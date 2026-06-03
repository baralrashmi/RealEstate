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
                Title = auction.Property.Title,
                Description = auction.Property.Description,
                Address = auction.Property.Address,
                City = auction.Property.City,
                State = auction.Property.State,
                zipCode = auction.Property.zipCode,
                Country = auction.Property.Country,
                Bedrooms = auction.Property.Bedrooms,
                Bathrooms = auction.Property.Bathrooms,
                AreaSqFt = auction.Property.AreaSqFt,
                ImageUrl = auction.Property.ImageUrl
            };
        }

        public static Auction ToAuctionEntity(this CreateAuctionDTO createAuctionDto)
        {
            return new Auction
            {

                ReservePrice = createAuctionDto.ReservePrice,
                AuctionEnd = createAuctionDto.AuctionEnd,
                Seller = "DemoSeller", // In a real application, this would come from the authenticated user context
                Property = new Property
                {
                    Title = createAuctionDto.Property.Title,
                    Description = createAuctionDto.Property.Description,
                    Address = createAuctionDto.Property.Address,
                    City = createAuctionDto.Property.City,
                    State = createAuctionDto.Property.State,
                    zipCode = createAuctionDto.Property.zipCode,
                    Country = createAuctionDto.Property.Country,
                    Bedrooms = createAuctionDto.Property.Bedrooms,
                    Bathrooms = createAuctionDto.Property.Bathrooms,
                    AreaSqFt = createAuctionDto.Property.AreaSqFt,
                    ImageUrl = createAuctionDto.Property.ImageUrl
                }

            };
        }


    }
}
