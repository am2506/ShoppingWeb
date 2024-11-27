using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Core.Models.OrderComponents
{
    public class ProductItemOrder
    {
        private ProductItemOrder()
        {
            
        }
        public ProductItemOrder(int productId, string productName, string? productImage)
        {
            ProductId = productId;
            ProductName = productName;
            ProductImage = productImage;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductImage { get; set; }
    }
}
