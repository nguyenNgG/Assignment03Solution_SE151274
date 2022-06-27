using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    internal class OrderDetailDAO
    {
        private static OrderDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        private OrderDetailDAO() { }

        public static OrderDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new OrderDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<OrderDetail>> GetList()
        {
            var db = new eStoreDbContext();
            List<OrderDetail> objs = null;
            objs = await db.OrderDetails.Include(x => x.Product).ToListAsync();
            return objs;
        }

        public async Task<OrderDetail> Get(int orderId, int productId)
        {
            var db = new eStoreDbContext();
            OrderDetail obj = await db.OrderDetails.Include(x => x.Product).FirstOrDefaultAsync(x => x.ProductId == productId && x.OrderId == orderId);
            return obj;
        }

        public async Task Add(OrderDetail obj)
        {
            var db = new eStoreDbContext();
            db.OrderDetails.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(OrderDetail obj)
        {
            var db = new eStoreDbContext();
            db.OrderDetails.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int orderId, int productId)
        {
            var db = new eStoreDbContext();
            OrderDetail obj = new OrderDetail { OrderId = orderId, ProductId = productId };
            db.OrderDetails.Attach(obj);
            db.OrderDetails.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
