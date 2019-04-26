using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
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
    public class VipOwnerApplicationRecordController : ApiController
    {
        private readonly IVipOwnerApplicationRecordRepository _vipOwnerApplicationRecordRepository;
        private readonly IVipOwnerCertificationAnnexRepository _vipOwnerCertificationAnnexRepository;
        private readonly IVipOwnerCertificationConditionRepository _vipOwnerCertificationConditionRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerApplicationRecordRepository"></param>
        /// <param name="vipOwnerCertificationAnnexRepository"></param>
        /// <param name="vipOwnerCertificationConditionRepository"></param>
        public VipOwnerApplicationRecordController(
            IVipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository,
            IVipOwnerCertificationAnnexRepository vipOwnerCertificationAnnexRepository,
            IVipOwnerCertificationConditionRepository vipOwnerCertificationConditionRepository)
        {
            _vipOwnerCertificationConditionRepository = vipOwnerCertificationConditionRepository;
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
        public async Task<ApiResult<AddVipOwnerApplicationOutput>> Add([FromBody]AddVipOwnerApplicationRecordInput input, CancellationToken cancelToken)
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
                var data = await _vipOwnerCertificationConditionRepository.GetListAsync(cancelToken);
                foreach (var item in data)
                {
                    var entity = input.Models.Where(x => x.ConditionId == item.Id.ToString()).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(entity?.AnnexContent))
                    {
                        throw new NotImplementedException("提交" + item.Title + "申请凭证信息不准确！");
                    }
                }

                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVipOwnerApplicationOutput>(APIResultCode.Unknown, new AddVipOwnerApplicationOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVipOwnerApplicationOutput>(APIResultCode.Unknown, new AddVipOwnerApplicationOutput { }, APIResultMessage.TokenError);
                }

                //TODO 添加业委会成员申请记录
                var vipOwnerApplicationRecord = await _vipOwnerApplicationRecordRepository.AddAsync(new VipOwnerApplicationRecordDto
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
                    await _vipOwnerCertificationAnnexRepository.AddAsync(new VipOwnerCertificationAnnexDto
                    {
                        ApplicationRecordId = vipOwnerApplicationRecord.Id.ToString(),
                        CertificationConditionId = item.ConditionId,
                        AnnexContent = item.AnnexContent,
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString()
                    });
                }
                return new ApiResult<AddVipOwnerApplicationOutput>(APIResultCode.Success, new AddVipOwnerApplicationOutput { Id = vipOwnerApplicationRecord.Id.ToString() }, APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<AddVipOwnerApplicationOutput>(APIResultCode.Success_NoB, new AddVipOwnerApplicationOutput { }, e.Message);
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
        public async Task<ApiResult<GetAllVipOwnerApplicationRecordOutput>> GetAll([FromUri]GetAllVipOwnerApplicationRecordInput input, CancellationToken cancelToken)
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
                if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
                {
                    throw new NotImplementedException("小区Id信息为空！");
                }
                var data = await _vipOwnerApplicationRecordRepository.GetAllAsync(new VipOwnerApplicationRecordDto
                {
                    SmallDistrictId = input.SmallDistrictId
                }, cancelToken);

                var listCount = data.Count();
                var list = data.Skip(startRow).Take(input.PageSize);

                return new ApiResult<GetAllVipOwnerApplicationRecordOutput>(APIResultCode.Success, new GetAllVipOwnerApplicationRecordOutput
                {
                    List = list.Select(x => new GetVipOwnerApplicationRecordOutput
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
                    }).ToList(),
                    TotalCount = listCount
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllVipOwnerApplicationRecordOutput>(APIResultCode.Success_NoB, new GetAllVipOwnerApplicationRecordOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 街道办查询详情
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerCertification/get")]
        public async Task<ApiResult<GetVipOwnerApplicationRecordOutput>> Get([FromUri]GetVipOwnerApplicationRecordInput input, CancellationToken cancelToken)
        {
            try
            {
                var data = await _vipOwnerApplicationRecordRepository.GetAsync(input.Id, cancelToken);
                GetVipOwnerApplicationRecordOutput output = new GetVipOwnerApplicationRecordOutput
                {
                    Id = data.Id.ToString(),
                    Name = data.Name,
                    SmallDistrictId = data.SmallDistrictId,
                    SmallDistrictName = data.SmallDistrictName,
                    IsAdopt = data.IsAdopt,
                    Reason = data.Reason,
                    StructureId = data.StructureId,
                    StructureName = data.StructureName,
                    UserId = data.UserId
                };
                var annexList = await _vipOwnerCertificationAnnexRepository.GetListAsync(new VipOwnerCertificationAnnexDto
                {
                    ApplicationRecordId = data.Id.ToString()
                }, cancelToken);
                var certificationConditionList = await _vipOwnerCertificationConditionRepository.GetAllAsync(new VipOwnerCertificationConditionDto { }, cancelToken);
                List<AnnexModel> list = new List<AnnexModel>();
                foreach (var item in annexList)
                {
                    if (Guid.TryParse(item.CertificationConditionId, out var uid))
                    {
                        //return new ApiResult<GetVipOwnerCertificationOutput>(APIResultCode.Success_NoB, output,"高级认证申请条件Id不准确，获取失败");
                    } 
                    list.Add(new AnnexModel
                    {
                        CertificationConditionName = certificationConditionList.Where(x=>x.Id== uid).FirstOrDefault().Title,
                        CertificationConditionId = item.ApplicationRecordId,
                        ID = item.Id.ToString(),
                        // Url = _vipOwnerCertificationAnnexRepository.GetUrlAsync(item.Id.ToString(), cancelToken),
                        Url = item.AnnexContent
                    });
                }
                output.AnnexModels = list;
                return new ApiResult<GetVipOwnerApplicationRecordOutput>(APIResultCode.Success, output);
            }
            catch (Exception e)
            {
                return new ApiResult<GetVipOwnerApplicationRecordOutput>(APIResultCode.Success_NoB, new GetVipOwnerApplicationRecordOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 街道办通过高级认证申请
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vipOwnerCertification/adopt")]
        public async Task<ApiResult> Adopt([FromBody]AdoptVipOwnerApplicationRecordInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.Id))
                {
                    throw new NotImplementedException("高级认证Id信息为空！");
                }
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
                await _vipOwnerApplicationRecordRepository.Adopt(new VipOwnerApplicationRecordDto
                {
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    Id = input.Id
                }, cancelToken);

                return new ApiResult(APIResultCode.Success, APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }


        }

        /// <summary>
        /// 查询高级申请列表(小程序用户)
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("weiXin/vipOwnerCertification/getAll")]
        public async Task<ApiResult<GetAllForWeiXinVipOwnerApplicationRecordOutput>> GetAllForWeiXin(CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetAllForWeiXinVipOwnerApplicationRecordOutput>(APIResultCode.Unknown, new GetAllForWeiXinVipOwnerApplicationRecordOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetAllForWeiXinVipOwnerApplicationRecordOutput>(APIResultCode.Unknown, new GetAllForWeiXinVipOwnerApplicationRecordOutput { }, APIResultMessage.TokenError);
                }
                if (user.DepartmentName != Department.YeZhu.Value)
                {
                    throw new NotImplementedException("用户权限不正确！");
                }
                var data = await _vipOwnerApplicationRecordRepository.GetListAsync(user.Id.ToString(), cancelToken);

                return new ApiResult<GetAllForWeiXinVipOwnerApplicationRecordOutput>(APIResultCode.Success, new GetAllForWeiXinVipOwnerApplicationRecordOutput
                {
                    List = data.Select(x => new GetVipOwnerApplicationRecordOutput
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
                return new ApiResult<GetAllForWeiXinVipOwnerApplicationRecordOutput>(APIResultCode.Success_NoB, new GetAllForWeiXinVipOwnerApplicationRecordOutput { }, e.Message);
            }
        }
    }
}
