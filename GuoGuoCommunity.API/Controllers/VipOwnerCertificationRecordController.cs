using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Linq;
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
        /// 提交高级认证申请
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
                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息为空！");
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
                          SmallDistrictId = input.SmallDistrictId,
                          Reason = input.Reason,
                          OperationTime = DateTimeOffset.Now,
                          UserId = user.Id.ToString(),
                          OwnerCertificationId = input.OwnerCertificationId
                      }, cancelToken);


                //TODO 添加业委会成员申请附件

                foreach (var item in input.Models)
                {
                    await _vipOwnerCertificationAnnexRepository.AddAsync(
                        new VipOwnerCertificationAnnexDto
                        {
                            ApplicationRecordId = vipOwnerApplicationRecord.Id.ToString(),
                            CertificationConditionId = item.ConditionId,
                            AnnexContent = item.AnnexContent,
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

        /// <summary>
        /// 查询高级申请列表(街道办指定小区)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerCertification/getAll")]
        public async Task<ApiResult<GetAllVipOwnerCertificationOutput>> GetAll([FromUri]GetAllVipOwnerCertificationInput input, CancellationToken cancelToken)
        {
            try
            {
                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var data = await _vipOwnerApplicationRecordRepository.GetAllAsync(new VipOwnerApplicationRecordDto
                {
                    SmallDistrictId = input.SmallDistrictId
                }, cancelToken);

                return new ApiResult<GetAllVipOwnerCertificationOutput>(APIResultCode.Success, new GetAllVipOwnerCertificationOutput
                {
                    List = data.Select(x => new GetVipOwnerCertificationOutput
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        SmallDistrictId = x.SmallDistrictId,
                        SmallDistrictName = x.SmallDistrictName,
                        IsAdopt = x.IsAdopt,
                        Reason = x.Reason,
                        StructureId = x.StructureId,
                        StructureName = x.StructureName,
                        UserId = x.UserId
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllVipOwnerCertificationOutput>(APIResultCode.Success_NoB, new GetAllVipOwnerCertificationOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 查询高级申请列表(小程序用户)
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("weiXin/vipOwnerCertification/getAll")]
        public async Task<ApiResult<GetAllForWeiXinVipOwnerCertificationOutput>> GetAllForWeiXin(CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetAllForWeiXinVipOwnerCertificationOutput>(APIResultCode.Unknown, new GetAllForWeiXinVipOwnerCertificationOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetAllForWeiXinVipOwnerCertificationOutput>(APIResultCode.Unknown, new GetAllForWeiXinVipOwnerCertificationOutput { }, APIResultMessage.TokenError);
                }
                if (user.DepartmentName != Department.YeZhu.Value)
                {
                    throw new NotImplementedException("用户权限不正确！");
                }
                var data = await _vipOwnerApplicationRecordRepository.GetListAsync(user.Id.ToString(), cancelToken);

                return new ApiResult<GetAllForWeiXinVipOwnerCertificationOutput>(APIResultCode.Success, new GetAllForWeiXinVipOwnerCertificationOutput
                {
                    List = data.Select(x => new GetVipOwnerCertificationOutput
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        SmallDistrictId = x.SmallDistrictId,
                        SmallDistrictName = x.SmallDistrictName,
                        IsAdopt = x.IsAdopt,
                        Reason = x.Reason,
                        StructureId = x.StructureId,
                        StructureName = x.StructureName,
                        UserId = x.UserId
                    }).ToList()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllForWeiXinVipOwnerCertificationOutput>(APIResultCode.Success_NoB, new GetAllForWeiXinVipOwnerCertificationOutput { }, e.Message);
            }
        }
    }
}
