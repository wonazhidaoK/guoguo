using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Service;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 业主认证
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OwnerCertificationRecordController : ApiController
    {
        private readonly OwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerCertificationRecordRepository"></param>
        public OwnerCertificationRecordController(OwnerCertificationRecordRepository ownerCertificationRecordRepository)
        {
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 添加业主认证记录信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ownerCertificationRecord/add")]
        public async Task<ApiResult<AddOwnerCertificationRecordOutput>> Add([FromBody]AddOwnerCertificationRecordInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Unknown, new AddOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.StreetOfficeId))
                {
                    throw new NotImplementedException("街道办Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.CommunityId))
                {
                    throw new NotImplementedException("社区Id信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
                {
                    throw new NotImplementedException("小区Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.BuildingId))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.BuildingUnitId))
                {
                    throw new NotImplementedException("单元Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.IndustryId))
                {
                    throw new NotImplementedException("业户Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Unknown, new AddOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
                }

                var entity = await _ownerCertificationRecordRepository.AddAsync(new OwnerCertificationRecordDto
                {

                    SmallDistrictId = input.SmallDistrictId,
                    IndustryId = input.IndustryId,
                    CommunityId = input.CommunityId,
                    StreetOfficeId = input.StreetOfficeId,
                    BuildingUnitId = input.BuildingUnitId,
                    BuildingId = input.BuildingId,
                    UserId = user.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Success, new AddOwnerCertificationRecordOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Success_NoB, new AddOwnerCertificationRecordOutput { }, e.Message);
            }
        }
    }
}
