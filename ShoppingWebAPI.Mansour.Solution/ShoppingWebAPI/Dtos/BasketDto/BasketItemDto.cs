using System.ComponentModel.DataAnnotations;

namespace RouteWebAPI.Dtos.BasketDto
{
    public class BasketItemDto
    {
        [Required]
        public int productId { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string PictureUrl { get; set; } = null!;
        [Range(0,double.MaxValue)]
        public decimal Price { get; set; } // Price will put in basket and display on system
        [Required]
        [Range(0.1,int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public string Category { get; set; } = null!;
        [Required]
        public string Brand { get; set; } = null!;
    }
}
