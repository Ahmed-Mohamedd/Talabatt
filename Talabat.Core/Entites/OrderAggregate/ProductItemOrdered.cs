using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.OrderAggregate
{
    public  class ProductItemOrdered
    {
        public ProductItemOrdered(int productId, string productName, string pictureUrl)
        {
            ProductId=productId;
            ProductName=productName;
            PictureUrl=pictureUrl;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
