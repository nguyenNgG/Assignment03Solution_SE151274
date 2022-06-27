using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    internal class MemberDAO
    {
        private static MemberDAO instance = null;
        private static readonly object instanceLock = new object();
        private MemberDAO() { }

        public static MemberDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new MemberDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Member>> GetList()
        {
            var db = new eStoreDbContext();
            List<Member> objs = null;
            objs = await db.Members.ToListAsync();
            return objs;
        }

        public async Task<Member> Get(string id)
        {
            var db = new eStoreDbContext();
            Member obj = await db.Members.FirstOrDefaultAsync(x => x.Id == id);
            return obj;
        }

        public async Task Add(Member obj)
        {
            var db = new eStoreDbContext();
            db.Members.Add(obj);
            await db.SaveChangesAsync();
        }

        public async Task Update(Member obj)
        {
            var db = new eStoreDbContext();
            db.Members.Update(obj);
            await db.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var db = new eStoreDbContext();
            Member obj = new Member { Id = id };
            db.Members.Attach(obj);
            db.Members.Remove(obj);
            await db.SaveChangesAsync();
        }
    }
}
