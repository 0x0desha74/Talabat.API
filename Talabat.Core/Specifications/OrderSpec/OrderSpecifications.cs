using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        public OrderSpecifications(string email) : base(order => order.BuyerEmail == email)
        {
            Includes.Add(O => O.Items);
            Includes.Add(O => O.DeliveryMethod);
            AddOrderByDesc(order => order.OrderDate);
        }


        public OrderSpecifications(string email, int id) : base(order => order.BuyerEmail == email && order.Id == id)
        {
            Includes.Add(O => O.Items);
            Includes.Add(O => O.DeliveryMethod);
        }

      
    }
}
