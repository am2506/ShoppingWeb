namespace RouteWebAPI.Helpers.HandleErrors
{
    public class APIValidationErrorResponse : APIErrorResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public APIValidationErrorResponse() : base(StatusCodes.Status400BadRequest)
        {
        }
    }
}
