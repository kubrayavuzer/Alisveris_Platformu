using Alisveris_Platformu.Business.Operations.Order.Dtos;
using Alisveris_Platformu.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alisveris_Platformu.Business.Operations.Order
{
    public interface IOrderService
    {
        Task<ServiceMessage> AddOrder(AddOrderDto order);
        Task<OrderDto> GetOrder(int id);

        Task<List<OrderDto>> GetOrders();

        Task<ServiceMessage> DeleteOrder(int id);
        Task<ServiceMessage> UpdateOrder(UpdateOrderDto order);

    }
}
