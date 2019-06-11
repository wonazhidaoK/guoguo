using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Abstractions
{
    public interface IPlatformCommodityRepository : IPageRepository<PlatformCommodity, PlatformCommodityDto,PlatformCommodityForPage>
    {
        /// <summary>
        /// 根据条码查询平台商品
        /// </summary>
        /// <param name="barCode"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<PlatformCommodity> GetForBarCodeAsync(string barCode, CancellationToken token = default);
    }
}
