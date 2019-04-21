using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
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
    /// 投票记录管理
    /// </summary>
    public class VoteRecordController : ApiController
    {
        private readonly IVoteRecordRepository _voteRecordRepository;
        private readonly IVoteRecordDetailRepository _voteRecordDetailRepository;
        private readonly IVoteQuestionOptionRepository _voteQuestionOptionRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IVoteAssociationVipOwnerRepository _voteAssociationVipOwnerRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voteRecordRepository"></param>
        /// <param name="voteRecordDetailRepository"></param>
        /// <param name="voteQuestionOptionRepository"></param>
        /// <param name="ownerCertificationRecordRepository"></param>
        /// <param name="voteAssociationVipOwnerRepository"></param>
        public VoteRecordController(IVoteRecordRepository voteRecordRepository,
            IVoteRecordDetailRepository voteRecordDetailRepository,
            IVoteQuestionOptionRepository voteQuestionOptionRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IVoteAssociationVipOwnerRepository voteAssociationVipOwnerRepository)
        {
            _voteRecordRepository = voteRecordRepository;
            _voteRecordDetailRepository = voteRecordDetailRepository;
            _voteQuestionOptionRepository = voteQuestionOptionRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _voteAssociationVipOwnerRepository = voteAssociationVipOwnerRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 业主投票(一个问题两个选项)
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
                    throw new NotImplementedException("请勾选选项再进行投票！");
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
                        VoteId = input.VoteId,
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

        /// <summary>
        /// 业主投票(一个问题多选项,业委会选举用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("voteRecord/addIndefinite")]
        public async Task<ApiResult<AddVoteRecordOutput>> AddIndefinite([FromBody]AddIndefiniteVoteRecordIntput input, CancellationToken cancelToken)
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
                    throw new NotImplementedException("请勾选选项再进行投票！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteRecordOutput>(APIResultCode.Unknown, new AddVoteRecordOutput { }, APIResultMessage.TokenError);
                }
                var voteAssociationVipOwner = await _voteAssociationVipOwnerRepository.GetForVoteIdAsync(input.VoteId, cancelToken);
                if (voteAssociationVipOwner.ElectionNumber != input.List[0].VoteQuestionOptionId.Count)
                {
                    throw new NotImplementedException("请选择"+ voteAssociationVipOwner.ElectionNumber+"项");
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
                    foreach (var itemOptionId in item.VoteQuestionOptionId)
                    {
                        var entityQuestion = await _voteRecordDetailRepository.AddAsync(new VoteRecordDetailDto
                        {
                            VoteQuestionId = item.VoteQuestionId,
                            VoteQuestionOptionId = itemOptionId,
                            VoteId = input.VoteId,
                            OperationTime = DateTimeOffset.Now,
                            OperationUserId = user.Id.ToString()
                        }, cancelToken);
                        await _voteQuestionOptionRepository.AddCountAsync(itemOptionId, cancelToken);
                    }

                }
                return new ApiResult<AddVoteRecordOutput>(APIResultCode.Success, new AddVoteRecordOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVoteRecordOutput>(APIResultCode.Success_NoB, new AddVoteRecordOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 获取投票建议
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("voteRecord/getFeedback")]
        public async Task<ApiResult<GetAllFeedbackOutput>> GetFeedback([FromUri]GetFeedbackInput input, CancellationToken cancellationToken)
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
            var data = ((await _voteRecordRepository.GetListAsync(new VoteRecordDto
            {
                VoteId = input.Id
            }, cancellationToken))).Where(x=>x.Feedback!="");
            List<GetFeedbackOutput> list = new List<GetFeedbackOutput>();
            foreach (var item in data)
            {
                string OperationName = (await _ownerCertificationRecordRepository.GetAsync(item.OwnerCertificationId, cancellationToken))?.OwnerName;

                list.Add(new GetFeedbackOutput
                {
                    Feedback = item.Feedback,
                    ReleaseTime = item.CreateOperationTime.Value,
                    OperationName = OperationName
                });
            }
            return new ApiResult<GetAllFeedbackOutput>(APIResultCode.Success, new GetAllFeedbackOutput
            {
                List = list.Skip(startRow).Take(input.PageSize).ToList(),
                TotalCount = list.Count
            });
        }
    }
}
