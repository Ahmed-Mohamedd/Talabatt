using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;
using Talabat.DAL.Entities;

namespace Talabat.Core.Entites.OrderAggregate
{
    public class Order:BaseEntity
    {
        public Order()
        {

        }
        public Order(string buyerEmail, List<OrderItem> items, OrderAddress shipToAddress, DeliveryMethod deliveryMethod, decimal subtotal , string paymentIntentId)
        {
            BuyerEmail=buyerEmail;
            Items=items;
            ShipToAddress=shipToAddress;
            DeliveryMethod=deliveryMethod;
            Subtotal=subtotal;
            PaymentIntentId=paymentIntentId;
        }

        public string BuyerEmail { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public OrderAddress ShipToAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string PaymentIntentId { get; set; }  

        public decimal Subtotal { get; set; }

        public decimal GetTotal()
            => Subtotal + DeliveryMethod.Cost;
    }
}
