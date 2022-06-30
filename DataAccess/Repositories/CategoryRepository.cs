using BusinessObject;
using DataAccess.DAOs;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public Task<List<Category>> GetList() => CategoryDAO.Instance.GetList();
        public Task<Category> Get(int id) => CategoryDAO.Instance.Get(id);
        public Task Add(Category obj) => CategoryDAO.Instance.Add(obj);
        public Task Update(Category obj) => CategoryDAO.Instance.Update(obj);
        public Task Delete(int id) => CategoryDAO.Instance.Delete(id);
    }
}
