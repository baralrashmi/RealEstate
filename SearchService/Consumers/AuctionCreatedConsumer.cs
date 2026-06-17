using AuctionService.Model;
using Contracts;
using MassTransit;
using SearchService.Model;

namespace SearchService.Consumers
{
    public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
    {
        public Task Consume(ConsumeContext<AuctionCreated> context)
        {
            var data = context.Message;
            var property = new SearchProperty
            {
                Id = data.Id,
                Seller = data.Seller,
                CreatedAt = data.CreatedAt,
                AuctionEnd = data.AuctionEnd,
                Status = Enum.Parse<AuctionStatus>(data.Status),
                ReservePrice = data.ReservePrice,
                Winner = data.Winner,
                SoldAmount = data.SoldAmount,
                CurrentHighBid = data.CurrentHighBid,
                UpdatedAt = data.UpdatedAt,
                Title = data.Title,
                Description = data.Description,
                Address = data.Address,
                City = data.City,
                State = data.State,
                zipCode = data.zipCode,
                Country = data.Country,
                Bedrooms = data.Bedrooms,
                Bathrooms = data.Bathrooms,
                AreaSqFt = data.AreaSqFt,
                ImageUrl = data.ImageUrl
            };


            return Task.CompletedTask;
        }
    }
}
