using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.OrderSpec;
using Talabat.Repository;
using Talabat.Repository.Data;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            ////1. Get Basket from basket Repo
            var basket = await _basketRepo.GetBasketAsync(basketId);


            ////2. Get Selected Items at basket from products Repo
            var orderItems = new List<OrderItem>();
            //we will trust just two properties coming from frontend [Id, Quantity]
            //orderItem[ProductItemOrdered(ProductName, PictureUrl, ProductId), Price, Quantity]
            //Protective Code
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                //remember we will take the product[returned from db] price not the price coming from frontend
                var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                orderItems.Add(orderItem);
            }


            ////3. Calculate SubTotal
            var subtotal = orderItems.Sum(oi => (oi.Price * oi.Quantity));




            ////4. Get Delivery Method from deliveryMethos Repo
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            
            
            ////5. Create Order Object  - Save To Database
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal);
            await _unitOfWork.Repository<Order>().AddAsync(order);

            ////6. Sava Changes to Database
            var result = await _unitOfWork.Complete();

            return result > 0 ? order : null;
        }

      

        public async Task<IReadOnlyList<Order>?> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var spec = new OrderSpecifications(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            return order is null? null : order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
              => await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            
        

    }
}
