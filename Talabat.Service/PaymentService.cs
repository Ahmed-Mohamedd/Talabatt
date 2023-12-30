using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Entites.OrderAggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.DAL.Entities;
using Product = Talabat.DAL.Entities.Product;


namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly StripeSettings _stripe;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PaymentService(IOptions<StripeSettings> stripe, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _stripe=stripe.Value;
            _basketRepository=basketRepository;
            _unitOfWork=unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            // secret key
            StripeConfiguration.ApiKey = _stripe.Secretkey;
            // Get basket
            var basket = await _basketRepository.GetBasket(basketId);
            var ShippingPrice = 0M;
            if (basket == null) return null;
            if(basket.DeliveryMethodId.HasValue)
            {
                var deliverMethod = await _unitOfWork.Repository<DeliveryMethod>().GetById(basket.DeliveryMethodId.Value);
                ShippingPrice = deliverMethod.Cost;
            }
            if(basket.Items.Count>0)
            {
               foreach( var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetById(item.Id);
                    if(item.Price!= product.Price)
                        item.Price = product.Price; 
                }
            }

            var SubTotal = basket.Items.Sum(i => i.Price * i.Quantity);
            
            //create payment intent
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal*100 + ShippingPrice*100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card"}
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else //update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal*100 + ShippingPrice*100)
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            await _basketRepository.UpdateBasket(basket);
            return basket;
        }
    }
}
