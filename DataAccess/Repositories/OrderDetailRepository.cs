using BusinessObject;
using DataAccess.DAOs;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        public Task<List<OrderDetail>> GetList() => OrderDetailDAO.Instance.GetList();
        public Task<OrderDetail> Get(int orderId, int productId) => OrderDetailDAO.Instance.Get(orderId, productId);
        public Task Add(OrderDetail obj) => OrderDetailDAO.Instance.Add(obj);
        public Task Update(OrderDetail obj) => OrderDetailDAO.Instance.Update(obj);
        public Task Delete(int orderId, int productId) => OrderDetailDAO.Instance.Delete(orderId, productId);
    }
}
