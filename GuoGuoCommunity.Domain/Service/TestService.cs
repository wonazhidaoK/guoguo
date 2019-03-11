using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Models;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class TestService : ITestService
    {
        private GuoGuoCommunityContext db = new GuoGuoCommunityContext();
        public async Task<Test> GetAsync(int id)
        {
            return await db.Tests.FindAsync(id);
        }
    }
}
