using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IOrderDetailRepository
    {
        public Task<List<OrderDetail>> GetList();
        public Task<OrderDetail> Get(int orderId, int productId);
        public Task Add(OrderDetail obj);
        public Task Update(OrderDetail obj);
        public Task Delete(int orderId, int productId);
    }
}
