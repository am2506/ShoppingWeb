
namespace RouteWebAPI.Helpers.HandleErrors
{
    public class APIErrorResponse
    {
        public int StatusCode { get; set; }
        public string ?Message { get; set; } 
        public APIErrorResponse(int statusCode , string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageByStatusCode(statusCode);
            
        }

        private string? GetDefaultMessageByStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Invalid syntax for this request was provided.",
                401 => "You are unauthorized to access the requested resource. " +
                       "Please log in.",
                404 => "We could not find the resource you requested." +
                " Please refer to the documentation for the list of resources.",
                _ => null
            };
        }
    }
}
