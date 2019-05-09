using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IIncludeRepository<T, Dto>
    {
        Task<T> AddAsync(Dto dto, CancellationToken token = default);

        Task UpdateAsync(Dto dto, CancellationToken token = default);

        Task<List<T>> GetAllAsync(Dto dto, CancellationToken token = default);

        Task<List<T>> GetAllIncludeAsync(Dto dto, CancellationToken token = default);

        Task DeleteAsync(Dto dto, CancellationToken token = default);

        Task<T> GetAsync(string id, CancellationToken token = default);

        Task<T> GetIncludeAsync(string id, CancellationToken token = default);

        Task<List<T>> GetListAsync(Dto dto, CancellationToken token = default);

        Task<List<T>> GetListIncludeAsync(Dto dto, CancellationToken token = default);
    }
}
