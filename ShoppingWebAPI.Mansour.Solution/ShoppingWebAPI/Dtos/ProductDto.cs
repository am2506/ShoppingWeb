using Shopping.Core.Models;

namespace RouteWebAPI.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? PictureUrl { get; set; }
        public int CategoryId { get; set; }
        public string? Category { get; set; }
        public int BrandId { get; set; }
        public string? Brand { get; set; }
    }
}
