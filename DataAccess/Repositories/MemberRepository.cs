using BusinessObject;
using DataAccess.DAOs;
using DataAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        public Task<List<Member>> GetList() => MemberDAO.Instance.GetList();
        public Task<Member> Get(string id) => MemberDAO.Instance.Get(id);
        public Task Add(Member obj) => MemberDAO.Instance.Add(obj);
        public Task Update(Member obj) => MemberDAO.Instance.Update(obj);
        public Task Delete(string id) => MemberDAO.Instance.Delete(id);
    }
}
