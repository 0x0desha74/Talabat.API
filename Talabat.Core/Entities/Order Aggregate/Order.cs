using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public int MyProperty { get; set; }
        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; } //NP [one]
        public ICollection<OrderItem> Item { get; set; } = new HashSet<OrderItem>(); //NP [Many]
        public decimal SubTotal { get; set; } //  = ProductPrice * Qantity
        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; }
    }


  
}
