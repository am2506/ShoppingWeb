using Shopping.Core.Models.OrderComponents;

namespace RouteWebAPI.Dtos.OrderDto
{
    public class OrderDto
    {
        public string basketId { get; set; } = null!;
        public AddressDto shippingAddress { get; set; } = null!;
        public int deliveryMethodId { get; set; }
    }
}
