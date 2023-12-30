using System;
using System.Collections.Generic;
using Talabat.Core.Entites.OrderAggregate;

namespace Talabat.Api.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public OrderAddress ShipToAddress { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public string Status { get; set; } 
        public string DeliveryMethod { get; set; }
        public decimal DeliveryCost { get; set; }
        public string PaymentIntentId { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public decimal Subtotal { get; set; }

        public decimal Total { get; set; }
            
    }
}
