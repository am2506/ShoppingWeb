using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core.Models;
using Shopping.Core.Specification;

namespace Shopping.Core.IServices
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(SpecParams? specParams);
        Task<int> GetCount(SpecParams? specParams);
        Task<Product?> GetProductAsync(int productId);
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetProductCategories();
    }
}
