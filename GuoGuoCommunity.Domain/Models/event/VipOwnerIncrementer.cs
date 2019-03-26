using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public delegate void VipOwnerHandler(GuoGuoCommunityContext db, VipOwner vipOwner, CancellationToken token = default);

    public class VipOwnerIncrementer//发布者
    {
        public event VipOwnerHandler VipOwnerEvent;
        public async Task OnUpdate(GuoGuoCommunityContext db, VipOwner vipOwner, CancellationToken token = default)//触发事件的方法
        {
            await Task.Run(() => VipOwnerEvent(db, vipOwner, token));
        }
    }

}
