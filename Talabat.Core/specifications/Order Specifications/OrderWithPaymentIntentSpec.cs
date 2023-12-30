using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.OrderAggregate;

namespace Talabat.Core.specifications.Order_Specifications
{
    public class OrderWithPaymentIntentSpec:BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string PaymentIntentId):base(o => o.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
