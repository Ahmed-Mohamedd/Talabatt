using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Api.Dtos;
using StackExchange.Redis;
using Talabat.API.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Repositories;

namespace Talabat.Api.Controllers
{
   
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository=basketRepository;
            _mapper=mapper;
        }

        // Get Basket

        [HttpGet("{BasketId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId) 
        {
            var CustomerBasket = await _basketRepository.GetBasket(BasketId);
            if(CustomerBasket is null) return NotFound(new ApiResponse(404));
            return Ok(CustomerBasket);
        }

        // update or create new basket

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);
            var CreatedOrUpdatedBasket = await _basketRepository.UpdateBasket(mappedBasket);
            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreatedOrUpdatedBasket);
        }

        // delete basket

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
             return await _basketRepository.DeleteBasket(BasketId);  
        }

    }
}
