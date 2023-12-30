using SmartCart.BLL.Repositories.Specifications.Order_Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites.OrderAggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.specifications.Order_Specifications;
using Talabat.DAL.Entities;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _basketRepository=basketRepository;
            _unitOfWork=unitOfWork;
            _paymentService=paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodID, OrderAddress shipToAddress )
        {
            //1- get basket from basket repo
            var basket = await _basketRepository.GetBasket(basketId);

            //2- get selected items at basket from product repo 
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetById(item.Id);
                var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                orderItems.Add(orderItem);
            }

            //3- get delivery method from delivery method repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetById(deliveryMethodID);

            //4- calc subtotal
            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            //5- create order
            var spec = new OrderWithPaymentIntentSpec(basket.PaymentIntentId);
            var ExOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpec(spec);
            if(ExOrder is not null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basket.Id);
            }
            
            var order = new Order(buyerEmail, orderItems, shipToAddress, deliveryMethod, subtotal , basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().Add(order);

            //6- save to database
            int res = await _unitOfWork.Complete();
            if (res<=0) return null;

            return order;

        }


        public async Task<Order> GetOrderByIdForUser(int orderId, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndDeliveryMethodsSpecification(orderId, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpec(spec);
            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndDeliveryMethodsSpecification(buyerEmail);
            var orders = (IReadOnlyList<Order>) await _unitOfWork.Repository<Order>().GetAllWithSpec(spec);
            return orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
            => (IReadOnlyList<DeliveryMethod>)await _unitOfWork.Repository<DeliveryMethod>().GetAll();

    }
}
