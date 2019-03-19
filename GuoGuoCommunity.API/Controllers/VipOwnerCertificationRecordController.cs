using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Threading;
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
        private readonly IVipOwnerCertificationAnnexRepository _vipOwnerCertificationAnnexRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerApplicationRecordRepository"></param>
        /// <param name="vipOwnerCertificationAnnexRepository"></param>
        public VipOwnerCertificationRecordController(
            IVipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository,
            IVipOwnerCertificationAnnexRepository vipOwnerCertificationAnnexRepository)
        {
            _vipOwnerApplicationRecordRepository = vipOwnerApplicationRecordRepository;
            _vipOwnerCertificationAnnexRepository = vipOwnerCertificationAnnexRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 提交高级申请
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vipOwnerCertification/Add")]
        public async Task<ApiResult<AddVipOwnerCertificationRecordOutput>> Add([FromBody]AddVipOwnerCertificationRecordInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.StructureId))
                {
                    throw new NotImplementedException("职能Id信息为空！");
                }
                if (input.Models.Count < 3)
                {
                    throw new NotImplementedException("提交申请凭证信息不准确！");
                }
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVipOwnerCertificationRecordOutput>(APIResultCode.Unknown, new AddVipOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVipOwnerCertificationRecordOutput>(APIResultCode.Unknown, new AddVipOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
                }

                //TODO 添加业委会成员申请记录
                var vipOwnerApplicationRecord = await _vipOwnerApplicationRecordRepository.AddAsync(
                      new VipOwnerApplicationRecordDto
                      {
                          StructureId = input.StructureId,
                          OperationUserId = user.Id.ToString(),
                          Reason = input.Reason,
                          OperationTime = DateTimeOffset.Now,
                          UserId = user.Id.ToString()
                      }, cancelToken);


                //TODO 添加业委会成员申请附件

                foreach (var item in input.Models)
                {
                    await _vipOwnerCertificationAnnexRepository.AddAsync(
                        new VipOwnerCertificationAnnexDto
                        {
                            ApplicationRecordId = vipOwnerApplicationRecord.Id.ToString(),
                            CertificationConditionId = item.ConditionId,
                            UploadId = item.UploadId,
                            OperationTime = DateTimeOffset.Now,
                            OperationUserId = user.Id.ToString()
                        });
                }
                return new ApiResult<AddVipOwnerCertificationRecordOutput>(APIResultCode.Success, new AddVipOwnerCertificationRecordOutput { Id = vipOwnerApplicationRecord.Id.ToString() }, APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<AddVipOwnerCertificationRecordOutput>(APIResultCode.Success_NoB, new AddVipOwnerCertificationRecordOutput { }, e.Message);
            }


        }
    }
}
