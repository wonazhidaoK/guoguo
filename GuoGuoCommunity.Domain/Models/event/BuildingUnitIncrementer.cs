using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public delegate void BuildingUnitHandler(GuoGuoCommunityContext db, BuildingUnit buildingUnit, CancellationToken token = default);

    public class BuildingUnitIncrementer//发布者
    {
        public event BuildingUnitHandler BuildingUnitEvent;

        public async Task OnUpdate(GuoGuoCommunityContext db, BuildingUnit buildingUnit, CancellationToken token = default)//触发事件的方法
        {
            await Task.Run(() => BuildingUnitEvent(db, buildingUnit, token));
        }
    }
}
