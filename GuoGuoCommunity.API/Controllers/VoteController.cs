using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.API.Models.Vote;
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
    /// 投票相关
    /// </summary>
    public class VoteController : ApiController
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IVoteQuestionRepository _voteQuestionRepository;
        private readonly IVoteQuestionOptionRepository _voteQuestionOptionRepository;
        private readonly IVoteAnnexRepository _voteAnnexRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voteRepository"></param>
        /// <param name="voteQuestionRepository"></param>
        /// <param name="voteQuestionOptionRepository"></param>
        /// <param name="voteAnnexRepository"></param>
        public VoteController(IVoteRepository voteRepository,
            IVoteQuestionRepository voteQuestionRepository,
            IVoteQuestionOptionRepository voteQuestionOptionRepository,
            IVoteAnnexRepository voteAnnexRepository)
        {
            _voteRepository = voteRepository;
            _voteQuestionRepository = voteQuestionRepository;
            _voteQuestionOptionRepository = voteQuestionOptionRepository;
            _voteAnnexRepository = voteAnnexRepository;
            _tokenManager = new TokenManager();
        }

        /*
         * 1.街道办发起投票
         * 2.物业发起投票
         * 3.业委会发起投票
         * 4.街道办展示投票列表
         * 5.物业展示投票列表
         * 6.业委会查看投票列表
         * 7.干预投票结果计算方式
         * 8.业主投票
         * 9.业主查看投票详情
         * 10.业主查看投票列表
         */

        /// <summary>
        /// 街道办发起投票
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("building/add")]
        public async Task<ApiResult<AddVoteForStreetOfficeOutput>> AddVoteForStreetOffice([FromBody]AddVoteForStreetOfficeInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Unknown, new AddVoteForStreetOfficeOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Summary))
                {
                    throw new NotImplementedException("投票摘要信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("投票标题信息为空！");
                }
                if (input.SmallDistricts.Count < 1)
                {
                    throw new NotImplementedException("投票范围小区信息为空！");
                }
                if (input.List.Count < 1)
                {
                    throw new NotImplementedException("投票问题信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Unknown, new AddVoteForStreetOfficeOutput { }, APIResultMessage.TokenError);
                }

                //增加投票主题
                var entity = await _voteRepository.AddAsync(new VoteDto
                {
                    Deadline = input.Deadline,
                    Title = input.Title,
                    Summary = input.Summary,
                    SmallDistrictArray = string.Join(",", input.SmallDistricts.ToArray()),
                    SmallDistrictId = user.SmallDistrictId,
                    CommunityId = user.CommunityId,
                    StreetOfficeId = user.StreetOfficeId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                //增加投票附件
                if (!string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    await _voteAnnexRepository.AddAsync(new VoteAnnexDto
                    {
                        AnnexContent = input.AnnexId,
                        VoteId = entity.Id.ToString(),
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);
                }

                //增加投票问题
                foreach (var item in input.List)
                {
                    var entityQuestion = await _voteQuestionRepository.AddAsync(new VoteQuestionDto
                    {
                        Title = item.Title,
                        OptionMode = item.OptionMode,
                        VoteId = entity.Id.ToString(),
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);

                    //增加投票问题选项
                    foreach (var questionOptions in item.List)
                    {
                        var questionOption = await _voteQuestionOptionRepository.AddAsync(new VoteQuestionOptionDto
                        {
                            Describe = questionOptions.Describe,
                            OperationTime = DateTimeOffset.Now,
                            OperationUserId = user.Id.ToString(),
                            VoteId = entity.Id.ToString(),
                            VoteQuestionId = entityQuestion.Id.ToString()
                        });
                    }
                }
                return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Success, new AddVoteForStreetOfficeOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Success_NoB, new AddVoteForStreetOfficeOutput { }, e.Message);
            }
        }


    }
}
