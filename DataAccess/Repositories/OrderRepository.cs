using BusinessObject;
using DataAccess.DAOs;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public Task<List<Order>> GetList() => OrderDAO.Instance.GetList();
        public Task<Order> Get(int id) => OrderDAO.Instance.Get(id);
        public Task Add(Order obj) => OrderDAO.Instance.Add(obj);
        public Task Update(Order obj) => OrderDAO.Instance.Update(obj);
        public Task Delete(int id) => OrderDAO.Instance.Delete(id);
    }
}
