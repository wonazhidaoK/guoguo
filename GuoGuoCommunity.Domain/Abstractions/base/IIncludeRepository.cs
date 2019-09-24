using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    /*
     * 抽象化的基类 
     * Include定义含义为：https://www.cnblogs.com/nlh774/p/3588286.html
     */
    public interface IIncludeRepository<T, Dto> : IRepository<T, Dto>
    {
        Task<List<T>> GetAllIncludeAsync(Dto dto, CancellationToken token = default);

        Task<T> GetIncludeAsync(string id, CancellationToken token = default);

        Task<List<T>> GetListIncludeAsync(Dto dto, CancellationToken token = default);
    }

    public interface IPageIncludeRepository<T, Dto, TForPage> :IIncludeRepository<T, Dto> 
    {
        Task<TForPage> GetAllIncludeForPageAsync(Dto dto, CancellationToken token = default);
    }
}
