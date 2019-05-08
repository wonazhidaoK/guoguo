using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 高级认证记录
    /// </summary>
    public class VipOwnerCertificationRecordController : BaseController
    {
        private readonly IVipOwnerCertificationRecordRepository _vipOwnerCertificationRecordRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerCertificationRecordRepository"></param>
        /// <param name="ownerCertificationRecordRepository"></param>
        public VipOwnerCertificationRecordController(IVipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository)
        {
            _vipOwnerCertificationRecordRepository = vipOwnerCertificationRecordRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 查询高级认证列表(物业端)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerCertificationRecord/getAllForProperty")]
        public async Task<ApiResult<GetAllVipOwnerCertificationRecordOutpu>> GetAllForProperty([FromUri]GetAllVipOwnerCertificationRecordInput input, CancellationToken cancelToken)
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
            if (Authorization == null)
            {
                return new ApiResult<GetAllVipOwnerCertificationRecordOutpu>(APIResultCode.Unknown, new GetAllVipOwnerCertificationRecordOutpu { }, APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllVipOwnerCertificationRecordOutpu>(APIResultCode.Unknown, new GetAllVipOwnerCertificationRecordOutpu { }, APIResultMessage.TokenError);
            }
            if (user.DepartmentValue != Department.WuYe.Value)
            {
                return new ApiResult<GetAllVipOwnerCertificationRecordOutpu>(APIResultCode.Success_NoB, new GetAllVipOwnerCertificationRecordOutpu { }, "操作人没有权限操作！");
            }
            var startTime = DateTimeOffset.Parse("1997-01-01");

            var endTime = DateTimeOffset.Parse("2997-01-01");

            if (DateTimeOffset.TryParse(input.StartTime, out DateTimeOffset startTimeSet))
            {
                startTime = startTimeSet;
            }
            if (DateTimeOffset.TryParse(input.EndTime, out DateTimeOffset endTimeSet))
            {
                endTime = endTimeSet.AddDays(1).AddMinutes(-1);
            }
            var data = await _vipOwnerCertificationRecordRepository.GetAllForPropertyAsync(new VipOwnerCertificationRecordDto
            {
                OperationUserSmallDistrictId = user.SmallDistrictId,
                VipOwnerId = input.VipOwnerId,
                EndTime = endTime,
                StartTime = startTime
            }, cancelToken);

            var listCount = data.Count();
            var list = data.Skip(startRow).Take(input.PageSize);
            var ownerCertificationRecordList = await _ownerCertificationRecordRepository.GetListForIdArrayIncludeAsync(list.Select(x => x.OwnerCertificationId).ToList());
            return new ApiResult<GetAllVipOwnerCertificationRecordOutpu>(APIResultCode.Success, new GetAllVipOwnerCertificationRecordOutpu
            {
                List = list.Select(x => new GetVipOwnerCertificationRecordOutpu
                {
                    Id = x.Id.ToString(),
                    CreateTime = x.CreateOperationTime.Value,
                    IsInvalid = x.IsInvalid,
                    VipOwnerName = x.VipOwnerName,
                    VipOwnerStructureName = x.VipOwnerStructureName,
                    OwnerCertificationName = ownerCertificationRecordList.Where(o => o.Id.ToString() == x.OwnerCertificationId).Select(o => o.Owner.Name).FirstOrDefault()
                }).ToList(),
                TotalCount = listCount
            });
        }
    }
}
