using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    internal class OrderDAO
    {
        private static OrderDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDAO() { }

        public static OrderDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Order>> GetList()
        {
            var db = new eStoreDbContext();
            List<Order> books = null;
            books = await db.Orders.Include(x => x.Member).ToListAsync();
            return books;
        }

        public async Task<Order> Get(int id)
        {
            var db = new eStoreDbContext();
            Order book = await db.Orders.Include(x => x.Member).Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.OrderId == id);
            return book;
        }

        public async Task Add(Order book)
        {
            var db = new eStoreDbContext();
            db.Orders.Add(book);
            await db.SaveChangesAsync();
        }

        public async Task Update(Order book)
        {
            var db = new eStoreDbContext();
            db.Orders.Update(book);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var db = new eStoreDbContext();
            Order book = new Order { OrderId = id };
            db.Orders.Attach(book);
            db.Orders.Remove(book);
            await db.SaveChangesAsync();
        }
    }
}
