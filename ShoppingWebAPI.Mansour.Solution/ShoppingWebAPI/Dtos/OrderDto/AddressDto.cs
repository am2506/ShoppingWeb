using System.ComponentModel.DataAnnotations;

namespace RouteWebAPI.Dtos.OrderDto
{
    public class AddressDto
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        [Required]
        public string Country { get; set; } = null!;
    }
}