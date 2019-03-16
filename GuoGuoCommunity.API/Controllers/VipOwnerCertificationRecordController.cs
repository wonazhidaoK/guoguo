using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 高级申请
    /// </summary>
    public class VipOwnerCertificationRecordController : ApiController
    {
        private readonly IVipOwnerApplicationRecordRepository _vipOwnerApplicationRecordRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerApplicationRecordRepository"></param>
        public VipOwnerCertificationRecordController(IVipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository)
        {
            _vipOwnerApplicationRecordRepository = vipOwnerApplicationRecordRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 提交高级申请
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vipOwnerCertification/Add")]
        public async Task<ApiResult> Add([FromBody]AddVipOwnerCertificationRecordInput input)
        {
            var token = HttpContext.Current.Request.Headers["Authorization"];
            if (token == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(token);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            //TODO 解析token

            //TODO 添加业委会成员申请记录

            //TODO 添加业委会成员申请附件
            return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
        }
    }
}
