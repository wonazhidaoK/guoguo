using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public delegate void BuildingHandler(GuoGuoCommunityContext db, Building building, CancellationToken token = default);

    public class BuildingIncrementer//发布者
    {
        public event BuildingHandler BuildingEvent;

        public async Task OnUpdate(GuoGuoCommunityContext db, Building building, CancellationToken token = default)//触发事件的方法
        {
            await Task.Run(() => BuildingEvent(db, building, token));
        }
    }
}
