using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteWebAPI.Dtos.OrderDto;
using RouteWebAPI.Helpers.HandleErrors;
using System.Security.Claims;
using Shopping.Core.IServices;
using Shopping.Core.Models.OrderComponents;

namespace RouteWebAPI.Controllers
{
    [Authorize]
    public class OrdersController : BaseAPIController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> OrderCreate(OrderDto model)
        {
            var buyerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (buyerEmail is null)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest));
            var address = _mapper.Map<Address>(model.shippingAddress);
            var orderCreated = await _orderService.CreateOrderAsync(buyerEmail, model.basketId, address, model.deliveryMethodId);
            if (orderCreated is null)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest));
            if (orderCreated.OrderItems.Count <= 0)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest, "Products not Found try in other time"));
            var orderMapped = _mapper.Map<OrderResponseDto>(orderCreated);
            return Ok(orderMapped);
        }
        [HttpGet]
        public async Task<ActionResult<OrderResponseDto>> GetUserOrders()
        {
            var buyerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (buyerEmail is null)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest));
            var userOrders = await _orderService.GetUserOrdersByEmailAsync(buyerEmail);
            if (userOrders is null)
                return NotFound(new APIErrorResponse(StatusCodes.Status404NotFound));
            var userOrdersMapped = _mapper.Map<ICollection<OrderResponseDto>>(userOrders);
            return Ok(userOrdersMapped);

        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<OrderResponseDto>> GetUserOrderByOrderId(int Id)
        {
            var buyerEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (buyerEmail is null)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest));
            var userOrder = await _orderService.GetUserOrderByOrderIdAsync(buyerEmail, Id);
            if (userOrder is null)
                return NotFound(new APIErrorResponse(StatusCodes.Status404NotFound));
            var userOrderMappd = _mapper.Map<OrderResponseDto>(userOrder);
            return Ok(userOrderMappd);
        }
        [HttpGet("deliveryMethods")]
       public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDelivertMethods()
        {
            var deliveryMethods = await _orderService.GetDeliveryMethodAsync();
            return Ok(deliveryMethods);
        }
    }
}
