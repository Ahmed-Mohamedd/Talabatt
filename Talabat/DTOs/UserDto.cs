using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.DTOs
{
    public class UserDto
    {
      
        public string DisplayName { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
