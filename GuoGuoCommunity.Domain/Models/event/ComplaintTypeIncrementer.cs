using GuoGuoCommunity.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace GuoGuoCommunity.Domain
{
    public delegate void ComplaintTypeHandler(GuoGuoCommunityContext db, ComplaintType complaintType, CancellationToken token = default);

    public class ComplaintTypeIncrementer
    {
        public event ComplaintTypeHandler ComplaintTypeEvent;

        public async Task OnUpdate(GuoGuoCommunityContext db, ComplaintType complaintType, CancellationToken token = default)
        {
            await Task.Run(() => ComplaintTypeEvent(db, complaintType, token));
        }
    }
}
