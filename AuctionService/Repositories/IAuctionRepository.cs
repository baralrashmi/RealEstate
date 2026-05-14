using AuctionService.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Repositories
{
    public interface IAuctionRepository
    {
        Task<List<AuctionDTO>> GetAuctionAsync();
        Task<ActionResult<AuctionDTO>> GetAuctionByIdAsync(Guid id);
        Task<ActionResult<AuctionDTO>> CreateAuction(AuctionDTO auctionDto);

    }
}
