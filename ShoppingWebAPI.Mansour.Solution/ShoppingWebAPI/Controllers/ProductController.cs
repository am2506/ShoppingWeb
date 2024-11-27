using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteWebAPI.Dtos;
using RouteWebAPI.Helpers.ApiResponse;
using RouteWebAPI.Helpers.Attributes;
using Shopping.Core.IRepository;
using Shopping.Core.IServices;
using Shopping.Core.Models;
using Shopping.Core.Specification;
using Shopping.Repository.SpecificationDesignPattern;

namespace RouteWebAPI.Controllers
{
    public class ProductsController : BaseAPIController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [Cache(60)]
        [HttpGet]
        public async Task<ActionResult> GetProducts([FromQuery] SpecParams? specParams)
        {
            var products = await _productService.GetProductsAsync(specParams);
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            var response = new ProductResponse()
            {
                PageIndex = specParams?.pageIndex ?? 0,
                PageSize = specParams?.pageSize ?? 0,
                Count = await _productService.GetCount(specParams),
                Data = productsDto

            };
            return Ok(response);
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult> GetProuctById(int Id)
        {
            var product = await _productService.GetProductAsync(Id);
            if (product is null)
                return NotFound(new { Message = "Not found product" });
            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetProductBrandsAsync();
            return Ok(brands);
        }
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _productService.GetProductCategories();
            return Ok(categories);
        }


    }
}
