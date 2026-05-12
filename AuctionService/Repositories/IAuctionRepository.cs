using AuctionService.DTOs;

namespace AuctionService.Repositories
{
    public interface IAuctionRepository
    {
        Task<List<AuctionDTO>> GetAuctionAsync();
    }
}
