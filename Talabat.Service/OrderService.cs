using Microsoft.EntityFrameworkCore.Storage;
//using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Repository.Data;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        private readonly IGenericRepository<Order> _orderRepo;
        public OrderService(IBasketRepository basketRepo,
            IGenericRepository<Product> productRepo,
            IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            IGenericRepository<Order> orderRepo)
        {
            _basketRepo = basketRepo;
            _productRepo = productRepo;
            _deliveryMethodRepo = deliveryMethodRepo;
            _orderRepo = orderRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            ////1. Get Basket from basket Repo
            ///
            var basket = await _basketRepo.GetBasketAsync(basketId);

            ////2. Get Selected Items at basket from products Repo

            var orderItems = new List<OrderItem>();
            //we will trust just two properties coming from frontend [Id, Quantity]
            //orderItem[ProductItemOrdered(ProductName, PictureUrl, ProductId), Price, Quantity]
            foreach (var item in basket.Items)
            {
                var product = await _productRepo.GetByIdAsync(item.Id);
                //remember we will take the product[returned from db] price not the price coming from frontend
                var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }

            ////3. Calculate SubTotal
            var subtotal = orderItems.Sum(oi => (oi.Price * oi.Quantity));

            ////4. Get Delivery Method from deliveryMethos Repo

            var deliveryMethod = await _deliveryMethodRepo.GetByIdAsync(deliveryMethodId);
            ////5. Create Order Object  - Save To Database
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal);

            await _orderRepo.AddAsync(order);
            ////6. Sava Changes [When we implement Unit Of Work Design pattern]
            return order;
        }

        public Task<Order> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
