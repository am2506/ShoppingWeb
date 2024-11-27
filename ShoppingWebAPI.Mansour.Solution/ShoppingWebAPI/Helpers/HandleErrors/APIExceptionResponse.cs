namespace RouteWebAPI.Helpers.HandleErrors
{
    public class APIExceptionResponse : APIErrorResponse
    {
        public string? Description { get; set; }
        public APIExceptionResponse(int statusCode, string?message = null, string? description = null)
            : base(statusCode, message)
        {
            Description = description;
        }
    }
}
