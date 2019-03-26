using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public delegate void SmallDistrictHandler(GuoGuoCommunityContext db, SmallDistrict smallDistrict, CancellationToken token = default);

    public class SmallDistrictIncrementer//发布者
    {
        public event SmallDistrictHandler SmallDistrictEvent;

        public async Task OnUpdate(GuoGuoCommunityContext db, SmallDistrict smallDistrict, CancellationToken token = default)//触发事件的方法
        {
            await Task.Run(() => SmallDistrictEvent(db, smallDistrict, token));
        }
    }
}
