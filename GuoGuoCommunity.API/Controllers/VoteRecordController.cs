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
    /// 投票记录管理
    /// </summary>
    public class VoteRecordController : ApiController
    {
        private readonly IVoteRecordRepository _voteRecordRepository;
        private readonly IVoteRecordDetailRepository _voteRecordDetailRepository;
        private readonly IVoteQuestionOptionRepository _voteQuestionOptionRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voteRecordRepository"></param>
        /// <param name="voteRecordDetailRepository"></param>
        /// <param name="voteQuestionOptionRepository"></param>
        public VoteRecordController(IVoteRecordRepository voteRecordRepository,
            IVoteRecordDetailRepository voteRecordDetailRepository,
            IVoteQuestionOptionRepository voteQuestionOptionRepository)
        {
            _voteRecordRepository = voteRecordRepository;
            _voteRecordDetailRepository = voteRecordDetailRepository;
            _voteQuestionOptionRepository = voteQuestionOptionRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 业主投票
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("voteRecord/add")]
        public async Task<ApiResult<AddVoteRecordOutput>> Add([FromBody]AddVoteRecordIntput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVoteRecordOutput>(APIResultCode.Unknown, new AddVoteRecordOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("投票业主认证Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.VoteId))
                {
                    throw new NotImplementedException("投票Id信息为空！");
                }
                if (input.List.Count < 1)
                {
                    throw new NotImplementedException("投票详情信息不准确！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteRecordOutput>(APIResultCode.Unknown, new AddVoteRecordOutput { }, APIResultMessage.TokenError);
                }

                //增加投票记录主体
                var entity = await _voteRecordRepository.AddAsync(new VoteRecordDto
                {
                    VoteId = input.VoteId,
                    OwnerCertificationId = input.OwnerCertificationId,
                    Feedback = input.Feedback,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                //增加投票记录详情
                foreach (var item in input.List)
                {
                    var entityQuestion = await _voteRecordDetailRepository.AddAsync(new VoteRecordDetailDto
                    {
                        VoteQuestionId = item.VoteQuestionId,
                        VoteQuestionOptionId = item.VoteQuestionOptionId,
                        VoteId = entity.Id.ToString(),
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);
                    await _voteQuestionOptionRepository.AddCountAsync(item.VoteQuestionOptionId, cancelToken);
                }
                return new ApiResult<AddVoteRecordOutput>(APIResultCode.Success, new AddVoteRecordOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVoteRecordOutput>(APIResultCode.Success_NoB, new AddVoteRecordOutput { }, e.Message);
            }
        }
    }
}
