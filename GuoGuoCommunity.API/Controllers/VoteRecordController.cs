using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class VoteRecordController : ApiController
    {
        private readonly IVoteRecordRepository voteRecordRepository;
        private readonly IVoteRecordDetailRepository voteRecordDetailRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voteRecordRepository"></param>
        /// <param name="voteRecordDetailRepository"></param>
        public VoteRecordController(IVoteRecordRepository voteRecordRepository,
            IVoteRecordDetailRepository voteRecordDetailRepository)
        {
            this.voteRecordRepository = voteRecordRepository;
            this.voteRecordDetailRepository = voteRecordDetailRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 街道办发起投票
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
                //if (string.IsNullOrWhiteSpace(input.Summary))
                //{
                //    throw new NotImplementedException("投票摘要信息为空！");
                //}
                //if (string.IsNullOrWhiteSpace(input.Title))
                //{
                //    throw new NotImplementedException("投票标题信息为空！");
                //}
                //if (input.SmallDistricts.Count < 1)
                //{
                //    throw new NotImplementedException("投票范围小区信息为空！");
                //}
                //if (input.List.Count < 1)
                //{
                //    throw new NotImplementedException("投票问题信息为空！");
                //}

                //var user = _tokenManager.GetUser(token);
                //if (user == null)
                //{
                //    return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Unknown, new AddVoteForStreetOfficeOutput { }, APIResultMessage.TokenError);
                //}

                ////增加投票主题
                //var entity = await _voteRepository.AddAsync(new VoteDto
                //{
                //    Deadline = input.Deadline,
                //    Title = input.Title,
                //    Summary = input.Summary,
                //    SmallDistrictArray = string.Join(",", input.SmallDistricts.ToArray()),
                //    SmallDistrictId = user.SmallDistrictId,
                //    CommunityId = user.CommunityId,
                //    StreetOfficeId = user.StreetOfficeId,
                //    OperationTime = DateTimeOffset.Now,
                //    OperationUserId = user.Id.ToString(),
                //    DepartmentValue = Department.JieDaoBan.Value,
                //    DepartmentName = Department.JieDaoBan.Name
                //}, cancelToken);

                ////增加投票附件
                //if (!string.IsNullOrWhiteSpace(input.AnnexId))
                //{
                //    await _voteAnnexRepository.AddAsync(new VoteAnnexDto
                //    {
                //        AnnexContent = input.AnnexId,
                //        VoteId = entity.Id.ToString(),
                //        OperationTime = DateTimeOffset.Now,
                //        OperationUserId = user.Id.ToString()
                //    }, cancelToken);
                //}

                ////增加投票问题
                //foreach (var item in input.List)
                //{
                //    var entityQuestion = await _voteQuestionRepository.AddAsync(new VoteQuestionDto
                //    {
                //        Title = item.Title,
                //        OptionMode = item.OptionMode,
                //        VoteId = entity.Id.ToString(),
                //        OperationTime = DateTimeOffset.Now,
                //        OperationUserId = user.Id.ToString()
                //    }, cancelToken);

                //    //增加投票问题选项
                //    foreach (var questionOptions in item.List)
                //    {
                //        var questionOption = await _voteQuestionOptionRepository.AddAsync(new VoteQuestionOptionDto
                //        {
                //            Describe = questionOptions.Describe,
                //            OperationTime = DateTimeOffset.Now,
                //            OperationUserId = user.Id.ToString(),
                //            VoteId = entity.Id.ToString(),
                //            VoteQuestionId = entityQuestion.Id.ToString()
                //        });
                //    }
                //}
                return new ApiResult<AddVoteRecordOutput>(APIResultCode.Success, new AddVoteRecordOutput { });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVoteRecordOutput>(APIResultCode.Success_NoB, new AddVoteRecordOutput { }, e.Message);
            }
        }
    }
}
