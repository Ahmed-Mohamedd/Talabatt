using System;

namespace Talabat.API.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ApiResponse()
        {

        }
        public ApiResponse(int StatusCode , string Message = null)
        {
            this.StatusCode = StatusCode;  
            this.Message = Message ?? GetDefaultMessage(StatusCode);
        }

        private string GetDefaultMessage(int statusCode)
           => statusCode switch
           {
               400 => "A bad Request , You have made" ,
               401 => "Authorized! , you are not",
               404 => "Resources was not found" ,
               500 => "Errors Are path to dark side , Errors leads to anger , Anger leads to hate , Hate leads to change career ",
                _  => null
           };
    }
}
