using GuoGuoCommunity.Domain.Models;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface ITestRepository
    {
        Task<Test> GetAsync(int id);
    }
}
