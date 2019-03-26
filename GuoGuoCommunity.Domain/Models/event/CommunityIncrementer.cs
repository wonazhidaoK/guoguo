using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public delegate void CommunityHandler(GuoGuoCommunityContext db, Community community, CancellationToken token = default);

    public class CommunityIncrementer//发布者
    {
        public event CommunityHandler CommunityEvent;

        public async Task OnUpdate(GuoGuoCommunityContext db, Community community, CancellationToken token = default)//触发事件的方法
        {
            await Task.Run(() => CommunityEvent(db, community, token));
        }
    }
}
