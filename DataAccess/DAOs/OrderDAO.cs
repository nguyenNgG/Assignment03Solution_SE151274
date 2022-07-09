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
            List<Order> objs = null;
            objs = await db.Orders.Include(x => x.Member).Include(x => x.OrderDetails).ToListAsync();
            return objs;
        }

        public async Task<Order> Get(int id)
        {
            var db = new eStoreDbContext();
            Order obj = await db.Orders.Include(x => x.Member).Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.OrderId == id);
            return obj;
        }

        public async Task Add(Order obj)
        {
            var db = new eStoreDbContext();
            db.Orders.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Order obj)
        {
            var db = new eStoreDbContext();
            db.Orders.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var db = new eStoreDbContext();
            Order obj = new Order { OrderId = id };
            db.Orders.Attach(obj);
            db.Orders.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
