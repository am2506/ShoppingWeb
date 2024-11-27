using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteWebAPI.Dtos.BasketDto;
using RouteWebAPI.Helpers.HandleErrors;
using Shopping.Core.IRepository;
using Shopping.Core.Models.Basket;

namespace RouteWebAPI.Controllers
{

    public class BasketController : BaseAPIController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            _basketRepository = basketRepository;
           _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string Id)
        {
            var basket = await _basketRepository.GetBasketAsync(Id);
            if (basket is null)
                return Ok(new CustomerBasket(Id));
            else
                return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var basketmodel = _mapper.Map<CustomerBasket>(basket);
            var updatebasket = await _basketRepository.UpdateBasketAsync(basketmodel);
            if (updatebasket is null)
                return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest));
            return Ok(updatebasket);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string Id)
        {
            return await _basketRepository.DeleteBasketAsync(Id);
        }
    
    }
}
