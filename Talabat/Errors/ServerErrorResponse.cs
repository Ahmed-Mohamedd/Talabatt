namespace Talabat.API.Errors
{
    public class ServerErrorResponse : ApiResponse
    {
        public string Details { get; set; }

        public ServerErrorResponse(int statusCode , string message = null  , string Details = null ):base(statusCode , message)
        {
            this.Details = Details;
        }
    }
}
