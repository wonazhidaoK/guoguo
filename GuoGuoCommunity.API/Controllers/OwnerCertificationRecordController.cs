using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using Hangfire;
using System;
using System.Diagnostics;
using System.Linq;
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
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IOwnerCertificationAnnexRepository _ownerCertificationAnnexRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerCertificationRecordRepository"></param>
        /// <param name="ownerCertificationAnnexRepository"></param>
        public OwnerCertificationRecordController(
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IOwnerCertificationAnnexRepository ownerCertificationAnnexRepository)
        {
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _ownerCertificationAnnexRepository = ownerCertificationAnnexRepository;
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
                if (input.Models.Count < 2)
                {
                    throw new NotImplementedException("提交附件信息不准确！");
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

                foreach (var item in input.Models)
                {
                    var itemEntity = await _ownerCertificationAnnexRepository.AddAsync(
                          new OwnerCertificationAnnexDto
                          {
                              ApplicationRecordId = entity.Id.ToString(),
                              OwnerCertificationAnnexTypeValue = item.OwnerCertificationAnnexTypeValue,
                              AnnexContent = item.AnnexContent,
                              OperationTime = DateTimeOffset.Now,
                              OperationUserId = user.Id.ToString()
                          });
                    if (itemEntity.OwnerCertificationAnnexTypeValue == OwnerCertificationAnnexType.IDCardFront.Value)
                    {
                        BackgroundJob.Enqueue(() => Send(itemEntity.Id.ToString()));
                    }
                }

                return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Success, new AddOwnerCertificationRecordOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Success_NoB, new AddOwnerCertificationRecordOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 获取用户认证列表
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ownerCertificationRecord/getList")]
        public async Task<ApiResult<GetListOwnerCertificationRecordInput>> GetList(CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetListOwnerCertificationRecordInput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordInput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetListOwnerCertificationRecordInput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordInput { }, APIResultMessage.TokenError);
                }
                var data = await _ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto
                {
                    CertificationStatusValue = OwnerCertification.Success.Value,
                    UserId = user.Id.ToString()
                }, cancelToken);
                return new ApiResult<GetListOwnerCertificationRecordInput>(APIResultCode.Success, new GetListOwnerCertificationRecordInput
                {
                    List = data.Select(x => new GetOwnerCertificationRecordInput
                    {
                        BuildingId = x.BuildingId,
                        BuildingName = x.BuildingName,
                        BuildingUnitId = x.BuildingUnitId,
                        BuildingUnitName = x.BuildingUnitName,
                        CertificationResult = x.CertificationResult,
                        CertificationStatusName = x.CertificationStatusName,
                        CertificationStatusValue = x.CertificationStatusValue,
                        CertificationTime = x.CertificationTime,
                        CommunityId = x.CommunityId,
                        CommunityName = x.CommunityName,
                        Id = x.Id.ToString(),
                        IndustryId = x.IndustryId,
                        IndustryName = x.IndustryName,
                        OwnerId = x.OwnerId,
                        OwnerName = x.OwnerName,
                        SmallDistrictId = x.SmallDistrictId,
                        SmallDistrictName = x.SmallDistrictName,
                        StreetOfficeId = x.StreetOfficeId,
                        StreetOfficeName = x.StreetOfficeName,
                        UserId = x.UserId
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetListOwnerCertificationRecordInput>(APIResultCode.Success_NoB, new GetListOwnerCertificationRecordInput { }, e.Message);
            }
        }

        /// <summary>
        /// 这个是用来发送消息的静态方法
        /// </summary>
        /// <param name="message"></param>
        public static void Send(string message)
        {
            EventLog.WriteEntry("EventSystem", string.Format("这里要处理一个图像识别任务:{0},时间为:{1}", message, DateTime.Now));
        }
    }
}
