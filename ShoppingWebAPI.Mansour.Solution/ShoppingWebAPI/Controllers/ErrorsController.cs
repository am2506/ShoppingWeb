using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteWebAPI.Helpers.HandleErrors;

namespace RouteWebAPI.Controllers
{
    [Route("[controller]/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return code switch
            {
                401 => Unauthorized(new APIErrorResponse(StatusCodes.Status401Unauthorized)),
                404 => NotFound(new APIErrorResponse(StatusCodes.Status404NotFound)),
                _ => StatusCode(code, new APIErrorResponse(code))
            };
        }
    }
}
