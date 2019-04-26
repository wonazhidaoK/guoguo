using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 高级认证记录
    /// </summary>
    public class VipOwnerCertificationRecordController : ApiController
    {
        private readonly IVipOwnerCertificationRecordRepository  _vipOwnerCertificationRecordRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerCertificationRecordRepository"></param>
        public VipOwnerCertificationRecordController(IVipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository)
        {
            _vipOwnerCertificationRecordRepository = vipOwnerCertificationRecordRepository;
            _tokenManager = new TokenManager();
        }

        ///// <summary>
        ///// 查询高级认证列表(物业端)
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="cancelToken"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("vipOwnerCertificationRecord/getAll")]
        //public async Task<ApiResult<GetAllVipOwnerCertificationRecordOutpu>> GetAllForProperty([FromUri]GetAllVipOwnerCertificationRecordInput input, CancellationToken cancelToken)
        //{
        //    try
        //    {
        //        if (input.PageIndex < 1)
        //        {
        //            input.PageIndex = 1;
        //        }
        //        if (input.PageSize < 1)
        //        {
        //            input.PageSize = 10;
        //        }
        //        int startRow = (input.PageIndex - 1) * input.PageSize;
        //        var token = HttpContext.Current.Request.Headers["Authorization"];
        //        if (token == null)
        //        {
        //            return new ApiResult<GetAllVipOwnerCertificationRecordOutpu>(APIResultCode.Unknown, new GetAllVipOwnerCertificationRecordOutpu { }, APIResultMessage.TokenNull);
        //        }
        //        var user = _tokenManager.GetUser(token);
        //        if (user == null)
        //        {
        //            return new ApiResult<GetAllVipOwnerCertificationRecordOutpu>(APIResultCode.Unknown, new GetAllVipOwnerCertificationRecordOutpu { }, APIResultMessage.TokenError);
        //        }
        //        if(user.DepartmentValue!= Department.WuYe.Value)
        //        {
        //            return new ApiResult<GetAllVipOwnerCertificationRecordOutpu>(APIResultCode.Success_NoB, new GetAllVipOwnerCertificationRecordOutpu { }, "操作人没有权限操作！");
        //        }
        //        //if (string.IsNullOrWhiteSpace(input.VipOwnerId))
        //        //{
        //        //    throw new NotImplementedException("公告内容信息为空！");
        //        //}
        //        //if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
        //        //{
        //        //    throw new NotImplementedException("小区Id信息为空！");
        //        //}
        //        var data = await _vipOwnerApplicationRecordRepository.GetAllAsync(new VipOwnerApplicationRecordDto
        //        {
        //            SmallDistrictId = input.SmallDistrictId
        //        }, cancelToken);

        //        var listCount = data.Count();
        //        var list = data.Skip(startRow).Take(input.PageSize);

        //        return new ApiResult<GetAllVipOwnerCertificationRecordOutpu>(APIResultCode.Success, new GetAllVipOwnerCertificationRecordOutpu
        //        {
        //            List = list.Select(x => new GetVipOwnerCertificationOutput
        //            {
        //                Id = x.Id.ToString(),
        //                Name = x.Name,
        //                SmallDistrictId = x.SmallDistrictId,
        //                SmallDistrictName = x.SmallDistrictName,
        //                IsAdopt = x.IsAdopt,
        //                Reason = x.Reason,
        //                StructureId = x.StructureId,
        //                StructureName = x.StructureName,
        //                UserId = x.UserId
        //            }).ToList(),
        //            TotalCount = listCount
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return new ApiResult<GetAllVipOwnerCertificationOutput>(APIResultCode.Success_NoB, new GetAllVipOwnerCertificationOutput { }, e.Message);
        //    }
        //}
    }
}
