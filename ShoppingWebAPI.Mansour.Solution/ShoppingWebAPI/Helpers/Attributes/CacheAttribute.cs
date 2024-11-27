using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Shopping.Core.IServices;

namespace RouteWebAPI.Helpers.Attributes
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLive;

        public CacheAttribute(int timeToLive)
        {
           _timeToLive = timeToLive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            var _cacheResponseService = context.HttpContext.RequestServices.GetRequiredService<ICacheResponseService>();
            var cacheKey = GenerateCacheKey(context.HttpContext.Request);
            var cacheResponse = await _cacheResponseService.GetCachResponse(cacheKey);
            //If Response is Cached will set the Response in Request Response
            if (!string.IsNullOrEmpty(cacheResponse))
            {
                var result = new ContentResult();
                context.Result = new ContentResult()
                {
                    Content = cacheResponse,
                    StatusCode = 200,
                    ContentType= "application/json"
                };
                return;
            }
          var executeActionContext =  await next.Invoke(); //will Exexute The Next Action OR The Action Itself

            if (executeActionContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {
               await _cacheResponseService.CachResponse(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLive)); 
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var sortedQuery = request.Query.OrderBy(q => q.Key).ToList();
            // Generate a cache key based on the sorted query parameters
            // var cacheKey = string.Join("&", sortedQuery.Select(q => $"{q.Key}={q.Value}"));
            var cacheKey = new StringBuilder();
            string controllerName = request.Path.Value?.Split('/')[2] ?? string.Empty;
            cacheKey.Append(controllerName);
            foreach (var item in sortedQuery)
            {
                cacheKey.Append($"&{item.Key}-{item.Value}");
            }
            return cacheKey.ToString();
        }
    }
}
