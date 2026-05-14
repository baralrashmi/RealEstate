using AuctionService.DTOs;
using AuctionService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            auctionRepository = auctionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<AuctionDTO>> GetAuctions()
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
            return CreatedAtAction(nameof(GetAuctionById), new { id = Guid.NewGuid() }, createAuctionDto);
        }
    }

}
