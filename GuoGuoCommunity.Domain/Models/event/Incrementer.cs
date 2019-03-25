using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain.Models
{
    public  delegate void Handler(GuoGuoCommunityContext db, StreetOffice  streetOffice, CancellationToken token = default);


    public class Incrementer//发布者
    {
        public event  Handler CountedADozen;
        public async Task DoCount(GuoGuoCommunityContext db, StreetOffice streetOffice, CancellationToken token = default)//触发事件的方法
        {

            await Task.Run(() => CountedADozen(db, streetOffice, token)) ;

        }
    }


}
