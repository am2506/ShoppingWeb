using Shopping.Core.Models.OrderComponents;

namespace RouteWebAPI.Dtos.OrderDto
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImage { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; } 
    }
}