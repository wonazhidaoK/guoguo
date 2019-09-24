using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    /*
     * 抽象化的基类 子类接口继承 在实现类 直接即可实现 减少代码量
     * dto 为定义的业务dto
     */
    public interface IRepository<T, Dto>
    {
        Task<T> AddAsync(Dto dto, CancellationToken token = default);

        Task UpdateAsync(Dto dto, CancellationToken token = default);

        Task<List<T>> GetAllAsync(Dto dto, CancellationToken token = default);

        Task DeleteAsync(Dto dto, CancellationToken token = default);

        Task<T> GetAsync(string id, CancellationToken token = default);

        Task<List<T>> GetListAsync(Dto dto, CancellationToken token = default);
    }

    /*
     *这是一个实现ef 分页传参的基类 
     * TForPage 包含实体类，分页参数的dto模型
     */
    public interface IPageRepository<T, Dto, TForPage> : IRepository<T, Dto>
    {
        Task<TForPage> GetListForPageAsync(Dto dto, CancellationToken token = default);
    }
}
