namespace Shopping.Core.IdentityModels
{
    public class Address
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;

        //Navigation Property
        public ApplicationUser ApplicationUser { get; set; } = null!;
        //Foreign Key
        public string ApplicationUserId { get; set; } = null!;
    }
}