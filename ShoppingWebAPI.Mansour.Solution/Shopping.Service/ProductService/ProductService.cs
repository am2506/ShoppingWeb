using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shopping.Core;
using Shopping.Core.IRepository;
using Shopping.Core.IServices;
using Shopping.Core.Models;
using Shopping.Core.Specification;
using Shopping.Repository.SpecificationDesignPattern;

namespace Shopping.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IGenericRepository<Product>ProductRepo { get; set; }

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            ProductRepo = _unitOfWork.Repository<Product>();
        }
        
        public async Task<IReadOnlyList<Product>> GetProductsAsync(SpecParams? specParams)
        {
            var spec = new ProductWithBrandAndCategory(specParams);
            var products = await ProductRepo.GetAllWithSpecAsync(spec);
            return products.ToList().AsReadOnly();
        } 
        public async Task<Product?> GetProductAsync(int productId)
        {
            var spec = new ProductWithBrandAndCategory(productId);
            var product = await ProductRepo.GetEntityWithSpec(spec);
            return product;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return brands.ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<ProductCategory>> GetProductCategories()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return categories.ToList().AsReadOnly();
        }

        public async Task<int> GetCount(SpecParams? specParams)
        {
            var spec = new ProductWithBrandAndCategory(specParams);
            return await _unitOfWork.Repository<Product>().GetCountAsync(spec);
        }
    }
}
