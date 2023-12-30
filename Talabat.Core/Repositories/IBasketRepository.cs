using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasket(string BasketId);

        Task<CustomerBasket?> UpdateBasket(CustomerBasket basket);
        
        Task<bool> DeleteBasket(string BasketId);
    }
}
