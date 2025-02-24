using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
   public interface IPaymentService
    {

        Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string basketId);
        Task<Order> UpdatePaymentIntentWithSucceededOrFailed(string paymentIntentId, bool flag);
    }
}
