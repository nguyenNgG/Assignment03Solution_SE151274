using BusinessObject;
using DataAccess.DAOs;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Task<List<Product>> GetList() => ProductDAO.Instance.GetList();
        public Task<Product> Get(int id) => ProductDAO.Instance.Get(id);
        public Task Add(Product obj) => ProductDAO.Instance.Add(obj);
        public Task Update(Product obj) => ProductDAO.Instance.Update(obj);
        public Task Delete(int id) => ProductDAO.Instance.Delete(id);
    }
}
