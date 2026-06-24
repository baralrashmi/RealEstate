using BidService.DTOs;
using BidService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BidService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IBidRepository _bidRepository;

        public BidsController(IBidRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        [HttpGet("auction/{auctionId}")]
        public async Task<ActionResult<List<BidDTO>>> GetBidsByAuctionId(Guid auctionId)
        {
            var bids = await _bidRepository.GetBidsByAuctionIdAsync(auctionId);
            return Ok(bids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidDTO>> GetBidById(Guid id)
        {
            var bid = await _bidRepository.GetBidByIdAsync(id);
            if (bid == null)
            {
                return NotFound();
            }
            return Ok(bid);
        }

        [HttpPost]
        public async Task<ActionResult<BidDTO>> CreateBid(CreateBidDTO createBidDto)
        {
            var bid = await _bidRepository.CreateBidAsync(createBidDto);
            return CreatedAtAction(nameof(GetBidById), new { id = bid.Id }, bid);
        }

        [HttpGet("auction/{auctionId}/highest")]
        public async Task<ActionResult<BidDTO>> GetHighestBidByAuction(Guid auctionId)
        {
            var bid = await _bidRepository.GetHighestBidByAuctionAsync(auctionId);
            if (bid == null)
            {
                return NotFound(new { message = "No bids found for this auction" });
            }
            return Ok(bid);
        }
    }
}
