using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IProductRepository
    {
        public Task<List<Product>> GetList();
        public Task<Product> Get(int id);
        public Task Add(Product obj);
        public Task Update(Product obj);
        public Task Delete(int id);
    }
}
