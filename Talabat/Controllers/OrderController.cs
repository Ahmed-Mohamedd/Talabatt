using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Api.DTOs;
using Talabat.API.Errors;
using Talabat.Core.Entites.OrderAggregate;
using Talabat.Core.Services;

namespace Talabat.Api.Controllers
{
    [Authorize]
    public class OrderController : APIBaseController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService=orderService;
            _mapper=mapper;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Order) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto) 
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orderAddress = _mapper.Map<AddressDto , OrderAddress >(orderDto.ShipToAddress);
            var order = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, orderAddress);

            if(order == null) return BadRequest(new ApiResponse(400, "An Error Occured During Creating the order"));

            return Ok(order);
        }

        [HttpGet("UserOrders")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);

            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);
            if (orders == null) return BadRequest(new ApiResponse(400, "The Current user hasn't confimed any orders yet!"));
            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("UserOrders/{id}")]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdForUser(id, buyerEmail);

            if (order == null) return NotFound(new ApiResponse(404, "Order not found"));
            return Ok(_mapper.Map<Order,OrderToReturnDto>(order));
        }


        [HttpGet("deliveryMethods")]
        [ProducesResponseType(typeof(DeliveryMethod), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliverMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodAsync());
        }
    }
}
