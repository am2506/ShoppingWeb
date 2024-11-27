using RouteWebAPI.Dtos;

namespace RouteWebAPI.Helpers.ApiResponse
{
    public class ProductResponse
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IEnumerable<ProductDto> Data { get; set; }
    }
}
