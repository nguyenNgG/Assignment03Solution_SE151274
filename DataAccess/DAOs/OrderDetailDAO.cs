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
            List<OrderDetail> books = null;
            books = await db.OrderDetails.Include(x => x.Product).ToListAsync();
            return books;
        }

        public async Task<OrderDetail> Get(int orderId, int productId)
        {
            var db = new eStoreDbContext();
            OrderDetail book = await db.OrderDetails.Include(x => x.Product).FirstOrDefaultAsync(x => x.ProductId == productId && x.OrderId == orderId);
            return book;
        }

        public async Task Add(OrderDetail book)
        {
            var db = new eStoreDbContext();
            db.OrderDetails.Add(book);
            await db.SaveChangesAsync();
        }

        public async Task Update(OrderDetail book)
        {
            var db = new eStoreDbContext();
            db.OrderDetails.Update(book);
            await db.SaveChangesAsync();
        }

        public async Task Delete(int orderId, int productId)
        {
            var db = new eStoreDbContext();
            OrderDetail book = new OrderDetail { OrderId = orderId, ProductId = productId };
            db.OrderDetails.Attach(book);
            db.OrderDetails.Remove(book);
            await db.SaveChangesAsync();
        }
    }
}
