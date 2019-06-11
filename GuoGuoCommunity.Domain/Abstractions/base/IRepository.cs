using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IRepository<T, Dto>
    {
        Task<T> AddAsync(Dto dto, CancellationToken token = default);

        Task UpdateAsync(Dto dto, CancellationToken token = default);

        Task<List<T>> GetAllAsync(Dto dto, CancellationToken token = default);

        Task DeleteAsync(Dto dto, CancellationToken token = default);

        Task<T> GetAsync(string id, CancellationToken token = default);

        Task<List<T>> GetListAsync(Dto dto, CancellationToken token = default);
    }

    public interface IPageRepository<T, Dto, TForPage> : IRepository<T, Dto>
    {
        Task<TForPage> GetListForPageAsync(Dto dto, CancellationToken token = default);
    }
}
