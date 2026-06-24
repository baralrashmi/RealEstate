using AuctionService.DTOs;
using AuctionService.Extensions;
using AuctionService.Repositories;
using AuctionService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IAuctionEventPublisher _eventPublisher;

        public AuctionsController(IAuctionRepository auctionRepository, IAuctionEventPublisher eventPublisher)
        {
            _auctionRepository = auctionRepository;
            _eventPublisher = eventPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDTO>>> GetAuctions()
        {
            var auctions = await _auctionRepository.GetAuctionAsync();
            return Ok(auctions);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDTO>> GetAuctionById(Guid id)
        {
            var auction = await _auctionRepository.GetAuctionByIdAsync(id);
            if (auction == null)
            {
                return NotFound();
            }
            return Ok(auction);
        }

        [HttpPost]
        //[Authorize(Roles = "Seller")] 

        public async Task<ActionResult<AuctionDTO>> CreateAuction(CreateAuctionDTO createAuctionDto)
        {
            var auction = createAuctionDto.ToAuctionEntity();
            var actionResult = await _auctionRepository.CreateAuction(auction);
            
            if (actionResult == null || actionResult.Value == null)
            {
                return NotFound();
            }

            var result = actionResult.Value;

            // Publish auction created event
            var auctionEvent = new
            {
                Id = result.Id,
                Seller = result.Seller,
                ReservePrice = result.ReservePrice,
                AuctionEnd = result.AuctionEnd,
                Status = "Live"
            };
            
            await _eventPublisher.PublishAuctionCreatedAsync(auctionEvent);

            return Ok(result);
        }
    }

}
