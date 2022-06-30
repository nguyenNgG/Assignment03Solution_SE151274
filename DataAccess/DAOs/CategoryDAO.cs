using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    internal class CategoryDAO
    {
        private static CategoryDAO instance = null;
        private static readonly object instanceLock = new object();
        private CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Category>> GetList()
        {
            var db = new eStoreDbContext();
            List<Category> objs = null;
            objs = await db.Categories.ToListAsync();
            return objs;
        }

        public async Task<Category> Get(int id)
        {
            var db = new eStoreDbContext();
            Category obj = await db.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
            return obj;
        }

        public async Task Add(Category obj)
        {
            var db = new eStoreDbContext();
            db.Categories.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Category obj)
        {
            var db = new eStoreDbContext();
            db.Categories.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var db = new eStoreDbContext();
            Category obj = new Category { CategoryId = id };
            db.Categories.Attach(obj);
            db.Categories.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
