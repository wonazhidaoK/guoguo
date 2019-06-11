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
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 高级认证申请条件
    /// </summary>
    public class VipOwnerCertificationConditionController : BaseController
    {
        private readonly IVipOwnerCertificationConditionRepository _vipOwnerCertificationConditionRepository;
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vipOwnerCertificationConditionRepository"></param>
        public VipOwnerCertificationConditionController(IVipOwnerCertificationConditionRepository vipOwnerCertificationConditionRepository)
        {
            _vipOwnerCertificationConditionRepository = vipOwnerCertificationConditionRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        ///  添加高级认证申请条件
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vipOwnerCertificationCondition/add")]
        public async Task<ApiResult<AddVipOwnerCertificationConditionOutput>> Add([FromBody]AddVipOwnerCertificationConditionInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddVipOwnerCertificationConditionOutput>(APIResultCode.Unknown, new AddVipOwnerCertificationConditionOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Title))
            {
                throw new NotImplementedException("申请条件标题信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.TypeValue))
            {
                throw new NotImplementedException("附件类型值信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddVipOwnerCertificationConditionOutput>(APIResultCode.Unknown, new AddVipOwnerCertificationConditionOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _vipOwnerCertificationConditionRepository.AddAsync(new VipOwnerCertificationConditionDto
            {
                Description = input.Description,
                Title = input.Title,
                TypeValue = input.TypeValue,
                TypeName = FileType.GetAll().Where(x => x.Value == input.TypeValue).Select(x => x.Name).FirstOrDefault(),
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            return new ApiResult<AddVipOwnerCertificationConditionOutput>(APIResultCode.Success, new AddVipOwnerCertificationConditionOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 获取高级认证申请条件列表
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vipOwnerCertificationCondition/getList")]
        public async Task<ApiResult<List<GetListVipOwnerCertificationConditionOutpu>>> GetList(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListVipOwnerCertificationConditionOutpu>>(APIResultCode.Unknown, new List<GetListVipOwnerCertificationConditionOutpu> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListVipOwnerCertificationConditionOutpu>>(APIResultCode.Unknown, new List<GetListVipOwnerCertificationConditionOutpu> { }, APIResultMessage.TokenError);
            }

            var data = await _vipOwnerCertificationConditionRepository.GetListAsync(cancelToken);

            return new ApiResult<List<GetListVipOwnerCertificationConditionOutpu>>(APIResultCode.Success, data.Select(x => new GetListVipOwnerCertificationConditionOutpu
            {
                Id = x.Id.ToString(),
                Description = x.Description,
                Title = x.Title,
                TypeName = x.TypeName,
                TypeValue = x.TypeValue
            }).ToList());
        }
    }
}
