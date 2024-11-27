namespace Shopping.Core.Models.Basket
{
    public class BasketItem
    {
        public int ProductId  { get; set; }
        public string ProductName { get; set; } =null!;
        public string PictureUrl { get; set; } = null!;
        public decimal Price { get; set; } // Price will put in basket and display on system
        public int Quantity { get; set; }
        public string Category { get; set; }   =null!;
        public string Brand { get; set; } = null!;
    }
}