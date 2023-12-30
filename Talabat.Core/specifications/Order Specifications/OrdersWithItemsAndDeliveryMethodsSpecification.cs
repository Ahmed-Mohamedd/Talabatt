using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.OrderAggregate;
using Talabat.Core.specifications;

namespace SmartCart.BLL.Repositories.Specifications.Order_Specifications
{
    public class OrdersWithItemsAndDeliveryMethodsSpecification:BaseSpecifications<Order>
    {

        // this constructor is used to get All orders for a specific User
        public OrdersWithItemsAndDeliveryMethodsSpecification(string buyerEmail):base(o => o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate);

        }

        // this constructor is used to get An order for a specific User
        public OrdersWithItemsAndDeliveryMethodsSpecification( int orderId, string buyerEmail) 
            :base(o => (o.BuyerEmail == buyerEmail && o.Id == orderId))
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);


        }
    }
}
