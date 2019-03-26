using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public delegate void StreetOfficeHandler(GuoGuoCommunityContext db, StreetOffice streetOffice, CancellationToken token = default);

    public class StreetOfficeIncrementer//发布者
    {
        public event StreetOfficeHandler StreetOfficeEvent;
        public async Task OnUpdate(GuoGuoCommunityContext db, StreetOffice streetOffice, CancellationToken token = default)//触发事件的方法
        {
            await Task.Run(() => StreetOfficeEvent(db, streetOffice, token));
        }
    }

}
