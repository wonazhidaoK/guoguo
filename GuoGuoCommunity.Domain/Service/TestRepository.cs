using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Models;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Service
{
    public class TestRepository : ITestRepository 
    {
        private GuoGuoCommunityContext db = new GuoGuoCommunityContext();

        public async Task<Test> Add(string str)
        {
            var entity=  db.Tests.Add(new Test {Id=0, Name=str });
            await db.SaveChangesAsync();
            return entity;
        }

        public async Task<Test> GetAsync(int id)
        {
            return await db.Tests.FindAsync(id);
        }
    }
}
