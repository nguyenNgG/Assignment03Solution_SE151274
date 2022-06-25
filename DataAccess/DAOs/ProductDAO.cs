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
            List<Product> books = null;
            books = await db.Products.Include(x => x.Category).ToListAsync();
            return books;
        }

        public async Task<Product> Get(int id)
        {
            var db = new eStoreDbContext();
            Product book = await db.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.ProductId == id);
            return book;
        }

        public async Task Add(Product book)
        {
            var db = new eStoreDbContext();
            db.Products.Add(book);
            await db.SaveChangesAsync();
        }

        public async Task Update(Product book)
        {
            var db = new eStoreDbContext();
            db.Products.Update(book);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var db = new eStoreDbContext();
            Product book = new Product { ProductId = id };
            db.Products.Attach(book);
            db.Products.Remove(book);
            await db.SaveChangesAsync();
        }
    }
}
