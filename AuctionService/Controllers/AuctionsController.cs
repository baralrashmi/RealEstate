using AuctionService.DTOs;
using AuctionService.Extensions;
using AuctionService.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public AuctionsController(IAuctionRepository auctionRepository, IPublishEndpoint publishEndpoint)
        {
            _auctionRepository = auctionRepository;
            _publishEndpoint = publishEndpoint;
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
            // Implementation for creating a new auction
            //return CreatedAtAction(nameof(GetAuctionById), new { id = Guid.NewGuid() }, createAuctionDto);
            var auction = createAuctionDto.ToAuctionEntity();


            await _publishEndpoint.Publish(auction.ToAuctionDTO().ToAuctionCreated());

            var result = await _auctionRepository.CreateAuction(auction);

            if (result == null)
            {
                return NotFound();
            }
            // CreatedAtAction means

            //
            return CreatedAtAction(nameof(GetAuctionById), new { id = Guid.NewGuid() }, createAuctionDto);
            //return Ok(result);

        }
    }

}
