using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public delegate void VipOwnerStructureHandler(GuoGuoCommunityContext db, VipOwnerStructure vipOwnerStructure, CancellationToken token = default);

    public class VipOwnerStructureIncrementer
    {
        public event VipOwnerStructureHandler VipOwnerStructureEvent;
        public async Task OnUpdate(GuoGuoCommunityContext db, VipOwnerStructure vipOwnerStructure, CancellationToken token = default)
        {
            await Task.Run(() => VipOwnerStructureEvent(db, vipOwnerStructure, token));
        }
    }
}
