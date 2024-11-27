using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RouteWebAPI.Helpers.HandleErrors;
using Shopping.Repository.Data;

namespace RouteWebAPI.Controllers
{

    public class BuggyController : BaseAPIController
    {
        private readonly StoreDbContext _dbContext;

        /// Handle API Errors
        /// 1. NotFound, BadRequest, Unauthorized Error Handling 
        ///     Use Class APIErrorResponse
        /// 2. Vaildation Error Handling
        ///     
        /// 

        public BuggyController(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(1000);
            if (product is not null)
                return Ok(product);
            return NotFound(new APIErrorResponse(StatusCodes.Status404NotFound));
        }
        [HttpGet("badrequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new APIErrorResponse(StatusCodes.Status400BadRequest));
        }
        [HttpGet("badrequest/{Id}")]
        public ActionResult GetBadRequest(int Id) // Validation Error
        {
            return Ok();
        }
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
           var product = _dbContext.Products.Find(1000);
            var prodctReturn = product.ToString(); // Will Throw Exception 
            //return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok(prodctReturn);
        }
    }
}
