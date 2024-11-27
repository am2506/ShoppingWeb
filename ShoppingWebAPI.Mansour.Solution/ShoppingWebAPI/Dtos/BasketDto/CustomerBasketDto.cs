using System.ComponentModel.DataAnnotations;
using Shopping.Core.Models.Basket;

namespace RouteWebAPI.Dtos.BasketDto
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public List<BasketItemDto> Items { get; set; }
        public string? paymentIntendId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
