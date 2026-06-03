using AuctionService.DTOs;
using AuctionService.Extensions;
using AuctionService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionRepository _auctionRepository;

        public AuctionsController(IAuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
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

            var result = await _auctionRepository.CreateAuction(auction);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
    }

}
