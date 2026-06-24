using BidService.Data;
using BidService.DTOs;
using BidService.Model;
using Microsoft.EntityFrameworkCore;

namespace BidService.Repositories
{
    public interface IBidRepository
    {
        Task<List<BidDTO>> GetBidsByAuctionIdAsync(Guid auctionId);
        Task<BidDTO?> GetBidByIdAsync(Guid id);
        Task<BidDTO> CreateBidAsync(CreateBidDTO createBidDto);
        Task<BidDTO?> GetHighestBidByAuctionAsync(Guid auctionId);
    }

    public class BidRepository : IBidRepository
    {
        private readonly BidDbContext _context;

        public BidRepository(BidDbContext context)
        {
            _context = context;
        }

        public async Task<List<BidDTO>> GetBidsByAuctionIdAsync(Guid auctionId)
        {
            return await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.BidAmount)
                .Select(b => MapToBidDto(b))
                .ToListAsync();
        }

        public async Task<BidDTO?> GetBidByIdAsync(Guid id)
        {
            var bid = await _context.Bids.FindAsync(id);
            return bid == null ? null : MapToBidDto(bid);
        }

        public async Task<BidDTO> CreateBidAsync(CreateBidDTO createBidDto)
        {
            var bid = new Bid
            {
                Id = Guid.NewGuid(),
                AuctionId = createBidDto.AuctionId,
                BidderName = createBidDto.BidderName,
                BidAmount = createBidDto.BidAmount,
                PlacedAt = DateTime.UtcNow
            };

            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();

            return MapToBidDto(bid);
        }

        public async Task<BidDTO?> GetHighestBidByAuctionAsync(Guid auctionId)
        {
            var bid = await _context.Bids
                .Where(b => b.AuctionId == auctionId)
                .OrderByDescending(b => b.BidAmount)
                .FirstOrDefaultAsync();

            return bid == null ? null : MapToBidDto(bid);
        }

        private static BidDTO MapToBidDto(Bid bid)
        {
            return new BidDTO
            {
                Id = bid.Id,
                AuctionId = bid.AuctionId,
                BidderName = bid.BidderName,
                BidAmount = bid.BidAmount,
                PlacedAt = bid.PlacedAt,
                IsWinning = bid.IsWinning
            };
        }
    }
}
