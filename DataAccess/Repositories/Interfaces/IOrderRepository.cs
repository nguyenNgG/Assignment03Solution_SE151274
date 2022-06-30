using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<List<Order>> GetList();
        public Task<Order> Get(int id);
        public Task Add(Order obj);
        public Task Update(Order obj);
        public Task Delete(int id);
    }
}
