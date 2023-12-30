using System.ComponentModel.DataAnnotations;

namespace Talabat.Api.DTOs
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }

        public AddressDto ShipToAddress { get; set; }
    }
}
