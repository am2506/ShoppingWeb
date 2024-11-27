using System.ComponentModel.DataAnnotations;

namespace RouteWebAPI.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$")]
        public string password { get; set; } = null!;
    }
}
