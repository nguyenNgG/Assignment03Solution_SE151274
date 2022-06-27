using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    internal class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }

        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Product>> GetList()
        {
            var db = new eStoreDbContext();
            List<Product> objs = null;
            objs = await db.Products.Include(x => x.Category).ToListAsync();
            return objs;
        }

        public async Task<Product> Get(int id)
        {
            var db = new eStoreDbContext();
            Product obj = await db.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.ProductId == id);
            return obj;
        }

        public async Task Add(Product obj)
        {
            var db = new eStoreDbContext();
            db.Products.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Product obj)
        {
            var db = new eStoreDbContext();
            db.Products.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var db = new eStoreDbContext();
            Product obj = new Product { ProductId = id };
            db.Products.Attach(obj);
            db.Products.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
