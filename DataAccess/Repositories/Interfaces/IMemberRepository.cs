using BusinessObject;
using DataAccess.DAOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        public Task<List<Member>> GetList();
        public Task<Member> Get(string id);
        public Task Add(Member obj);
        public Task Update(Member obj);
        public Task Delete(string id);
    }
}
