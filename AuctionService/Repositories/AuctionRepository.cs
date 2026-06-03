using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Extensions;
using AuctionService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AuctionService.Repositories
{
    public class AuctionRepository(RealEstateContext realEstateContext) : IAuctionRepository
    {
        //explain below line
        // This line defines a constructor for the AuctionRepository class that takes a RealEstateContext object as a parameter. The RealEstateContext is likely a class that represents the database context for the application, allowing the repository to interact with the database. By using this constructor, an instance of AuctionRepository can be created with a specific RealEstateContext, enabling it to perform database operations related to auctions.

        //
        public async Task<ActionResult<AuctionDTO>> CreateAuction(Auction auction)
        // This method, CreateAuction, is an asynchronous function that takes an
        // Auction object as a parameter and
        // returns a Task containing an ActionResult of type AuctionDTO.
        // The method is responsible for adding the provided Auction object
        // to the database context (realEstateContext) and saving the changes asynchronously.
        // After successfully saving the auction to the database,
        // it returns an ActionResult containing the newly created auction converted to an AuctionDTO
        // using the ToAuctionDTO extension method.
        // This allows the caller to receive a representation of the created auction
        // in a format suitable for data transfer (DTO).
        {
            realEstateContext.Auctions.Add(auction);
            await realEstateContext.SaveChangesAsync();
            return new ActionResult<AuctionDTO>(auction.ToAuctionDTO());
        }

        public async Task<List<AuctionDTO>> GetAuctionAsync()
        {
            var auctions = await realEstateContext.Auctions
                .Include(x => x.Property)
                .OrderBy(x => x.Property.Title)
                .AsQueryable().ToListAsync();


            return auctions.Select(a => a.ToAuctionDTO()).ToList();

        }

        public async Task<ActionResult<AuctionDTO>> GetAuctionByIdAsync(Guid id)
        {
            var query = await realEstateContext.Auctions
                .Include(x => x.Property)
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            if (query == null)
            {
                return new NotFoundResult();
            }


            return new ActionResult<AuctionDTO>(query.ToAuctionDTO());
        }

    }
}
