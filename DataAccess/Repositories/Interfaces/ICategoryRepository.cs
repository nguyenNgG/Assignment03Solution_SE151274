using BusinessObject;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetList();
        public Task<Category> Get(int id);
        public Task Add(Category obj);
        public Task Update(Category obj);
        public Task Delete(int id);
    }
}
