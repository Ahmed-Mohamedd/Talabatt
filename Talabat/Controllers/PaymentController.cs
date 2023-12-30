using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartCart.Api.Dtos;
using Talabat.API.Errors;
using Talabat.Core.Entites;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers
{
    [Authorize]
    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        public PaymentController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService=paymentService;
            _mapper=mapper;
        }



        [HttpPost("{basketId}")]
        [ProducesResponseType(typeof(CustomerBasketDto) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var customerBasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (customerBasket is null) return BadRequest(new ApiResponse(400, "There is aproblem with your basket"));
            return Ok(_mapper.Map<CustomerBasketDto>(customerBasket));
        }

    }
}
