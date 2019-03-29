using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.API.Models.Vote;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using GuoGuoCommunity.Domain.Service;
using Hangfire;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using Senparc.Weixin.MP.Containers;
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
         *  1.街道办发起投票
         *  2.物业发起投票
         *  3.业委会发起投票
         *  4.街道办展示投票列表
         *  5.物业展示投票列表
         *  6.业委会查看投票列表
         *  7.干预投票结果计算方式
         *  8.业主投票
         * 9.业主查看投票详情
         *  10.业主查看投票列表
         *  11.投票详情
         */

        /// <summary>
        /// 街道办发起投票
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vote/addVoteForStreetOffice")]
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
                if (string.IsNullOrWhiteSpace(input.SmallDistrict))
                {
                    throw new NotImplementedException("投票范围小区信息为空！");
                }
                if (input.List.Count == 1)
                {
                    throw new NotImplementedException("投票问题数量信息不正确！");
                }
                if (input.List[0].List.Count == 2)
                {
                    throw new NotImplementedException("投票问题选项数量信息不正确！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Unknown, new AddVoteForStreetOfficeOutput { }, APIResultMessage.TokenError);
                }

                //增加投票主体
                var entity = await _voteRepository.AddAsync(new VoteDto
                {
                    Deadline = input.Deadline,
                    Title = input.Title,
                    Summary = input.Summary,
                    SmallDistrictArray = input.SmallDistrict,
                    SmallDistrictId = user.SmallDistrictId,
                    CommunityId = user.CommunityId,
                    StreetOfficeId = user.StreetOfficeId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    DepartmentValue = Department.JieDaoBan.Value,
                    DepartmentName = Department.JieDaoBan.Name
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

                //TODO发布投票同步推送
                //TODO发布投票添加定时任务计算投票结果
                BackgroundJob.Schedule(() => AddVoteResultRecordAsync(entity.Id), entity.Deadline);
                return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Success, new AddVoteForStreetOfficeOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Success_NoB, new AddVoteForStreetOfficeOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 物业发起投票
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vote/addVoteForProperty")]
        public async Task<ApiResult<AddVoteForPropertyOutput>> AddVoteForProperty([FromBody]AddVoteForPropertyInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVoteForPropertyOutput>(APIResultCode.Unknown, new AddVoteForPropertyOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Summary))
                {
                    throw new NotImplementedException("投票摘要信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("投票标题信息为空！");
                }

                if (input.List.Count == 1)
                {
                    throw new NotImplementedException("投票问题数量信息不正确！");
                }
                if (input.List[0].List.Count == 2)
                {
                    throw new NotImplementedException("投票问题选项数量信息不正确！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForPropertyOutput>(APIResultCode.Unknown, new AddVoteForPropertyOutput { }, APIResultMessage.TokenError);
                }

                //增加投票主体
                var entity = await _voteRepository.AddAsync(new VoteDto
                {
                    Deadline = input.Deadline,
                    Title = input.Title,
                    Summary = input.Summary,
                    SmallDistrictArray = user.SmallDistrictId,
                    SmallDistrictId = user.SmallDistrictId,
                    CommunityId = user.CommunityId,
                    StreetOfficeId = user.StreetOfficeId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    DepartmentValue = Department.WuYe.Value,
                    DepartmentName = Department.WuYe.Name
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
                return new ApiResult<AddVoteForPropertyOutput>(APIResultCode.Success, new AddVoteForPropertyOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVoteForPropertyOutput>(APIResultCode.Success_NoB, new AddVoteForPropertyOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 业委会发起投票(小程序用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vote/addVoteForVipOwner")]
        public async Task<ApiResult<AddVoteForVipOwnerOutput>> AddVoteForVipOwner([FromBody]AddVoteForVipOwnerInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVoteForVipOwnerOutput>(APIResultCode.Unknown, new AddVoteForVipOwnerOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Summary))
                {
                    throw new NotImplementedException("投票摘要信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("投票标题信息为空！");
                }

                if (input.List.Count == 1)
                {
                    throw new NotImplementedException("投票问题数量信息不正确！");
                }
                if (input.List[0].List.Count == 2)
                {
                    throw new NotImplementedException("投票问题选项数量信息不正确！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForVipOwnerOutput>(APIResultCode.Unknown, new AddVoteForVipOwnerOutput { }, APIResultMessage.TokenError);
                }

                //增加投票主体
                var entity = await _voteRepository.AddAsync(new VoteDto
                {
                    Deadline = input.Deadline,
                    Title = input.Title,
                    Summary = input.Summary,
                    SmallDistrictArray = user.SmallDistrictId,
                    SmallDistrictId = user.SmallDistrictId,
                    CommunityId = user.CommunityId,
                    StreetOfficeId = user.StreetOfficeId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    DepartmentValue = Department.YeZhuWeiYuanHui.Value,
                    DepartmentName = Department.YeZhuWeiYuanHui.Name
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
                return new ApiResult<AddVoteForVipOwnerOutput>(APIResultCode.Success, new AddVoteForVipOwnerOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVoteForVipOwnerOutput>(APIResultCode.Success_NoB, new AddVoteForVipOwnerOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 街道办查询投票列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/getAllForStreetOffice")]
        public async Task<ApiResult<GetAllForStreetOfficeOutput>> GetAllForStreetOffice([FromUri]GetAllForStreetOfficeInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetAllForStreetOfficeOutput>(APIResultCode.Unknown, new GetAllForStreetOfficeOutput { }, APIResultMessage.TokenNull);
                }

                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetAllForStreetOfficeOutput>(APIResultCode.Unknown, new GetAllForStreetOfficeOutput { }, APIResultMessage.TokenError);
                }
                var data = await _voteRepository.GetAllForStreetOfficeAsync(new VoteDto
                {
                    SmallDistrictArray = input.SmallDistrictArray,
                    Title = input.Title,
                    StreetOfficeId = user.StreetOfficeId,
                    DepartmentValue = Department.JieDaoBan.Value
                }, cancelToken);

                return new ApiResult<GetAllForStreetOfficeOutput>(APIResultCode.Success, new GetAllForStreetOfficeOutput
                {
                    List = data.Select(x => new GetForStreetOfficeOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Deadline = x.Deadline,
                        SmallDistrictArray = x.SmallDistrictArray,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        Summary = x.Summary
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllForStreetOfficeOutput>(APIResultCode.Success_NoB, new GetAllForStreetOfficeOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 物业查询投票列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/getAllForProperty")]
        public async Task<ApiResult<GetAllForPropertyOutput>> GetAllForProperty([FromUri]GetAllForPropertyInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetAllForPropertyOutput>(APIResultCode.Unknown, new GetAllForPropertyOutput { }, APIResultMessage.TokenNull);
                }

                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetAllForPropertyOutput>(APIResultCode.Unknown, new GetAllForPropertyOutput { }, APIResultMessage.TokenError);
                }
                var data = await _voteRepository.GetAllForPropertyAsync(new VoteDto
                {
                    Title = input.Title,
                    StreetOfficeId = user.StreetOfficeId,
                    CommunityId = user.CommunityId,
                    SmallDistrictId = user.SmallDistrictId,
                    //DepartmentValue = Department.JieDaoBan.Value
                }, cancelToken);

                return new ApiResult<GetAllForPropertyOutput>(APIResultCode.Success, new GetAllForPropertyOutput
                {
                    List = data.Select(x => new GetForStreetOfficeOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Deadline = x.Deadline,
                        SmallDistrictArray = x.SmallDistrictArray,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        Summary = x.Summary
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllForPropertyOutput>(APIResultCode.Success_NoB, new GetAllForPropertyOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 业委会查询投票列表(小程序用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/getAllForVipOwner")]
        public async Task<ApiResult<GetAllForVipOwnerOutput>> GetAllForVipOwner([FromUri]GetAllForVipOwnerInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Unknown, new GetAllForVipOwnerOutput { }, APIResultMessage.TokenNull);
                }

                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Unknown, new GetAllForVipOwnerOutput { }, APIResultMessage.TokenError);
                }
                var data = await _voteRepository.GetAllForVipOwnerAsync(new VoteDto
                {
                    Title = input.Title,
                    StreetOfficeId = user.StreetOfficeId,
                    CommunityId = user.CommunityId,
                    SmallDistrictId = user.SmallDistrictId,
                    // DepartmentValue = Department.YeZhuWeiYuanHui.Value
                }, cancelToken);

                return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Success, new GetAllForVipOwnerOutput
                {
                    List = data.Select(x => new GetForStreetOfficeOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Deadline = x.Deadline,
                        SmallDistrictArray = x.SmallDistrictArray,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        Summary = x.Summary
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Success_NoB, new GetAllForVipOwnerOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 业主查询投票列表(小程序用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/getAllForOwner")]
        public async Task<ApiResult<GetAllForOwnerOutput>> GetAllForOwner([FromUri]GetAllForOwnerInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetAllForOwnerOutput>(APIResultCode.Unknown, new GetAllForOwnerOutput { }, APIResultMessage.TokenNull);
                }

                if (input.PageIndex < 1)
                {
                    input.PageIndex = 1;
                }
                if (input.PageSize < 1)
                {
                    input.PageSize = 10;
                }
                int startRow = (input.PageIndex - 1) * input.PageSize;
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetAllForOwnerOutput>(APIResultCode.Unknown, new GetAllForOwnerOutput { }, APIResultMessage.TokenError);
                }
                var data = await _voteRepository.GetAllForOwnerAsync(new VoteDto
                {
                    Title = input.Title,
                    StreetOfficeId = user.StreetOfficeId,
                    CommunityId = user.CommunityId,
                    SmallDistrictId = user.SmallDistrictId,
                    DepartmentValue = input.DepartmentValue
                    // DepartmentValue = Department.YeZhuWeiYuanHui.Value
                }, cancelToken);

                return new ApiResult<GetAllForOwnerOutput>(APIResultCode.Success, new GetAllForOwnerOutput
                {
                    List = data.Select(x => new GetForStreetOfficeOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Deadline = x.Deadline,
                        SmallDistrictArray = x.SmallDistrictArray,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        Summary = x.Summary
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllForOwnerOutput>(APIResultCode.Success_NoB, new GetAllForOwnerOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 获取投票详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/get")]
        public async Task<ApiResult<GetVoteOutput>> Get([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("投票Id信息为空！");
                }
                var vote = await _voteRepository.GetAsync(id, cancelToken);
                var voteQuestionList = await _voteQuestionRepository.GetListAsync(new VoteQuestionDto { VoteId = vote?.Id.ToString() }, cancelToken);
                List<GetVoteQuestionModel> list = new List<GetVoteQuestionModel>();
                foreach (var item in voteQuestionList)
                {
                    GetVoteQuestionModel questionModel = new GetVoteQuestionModel();
                    List<GetVoteQuestionOptionModel> voteQuestionOptionModels = new List<GetVoteQuestionOptionModel>();
                    var voteQuestionOptionList = await _voteQuestionOptionRepository.GetListAsync(new VoteQuestionOptionDto { VoteId = vote?.Id.ToString(), VoteQuestionId = item.Id.ToString() }, cancelToken);
                    foreach (var voteQuestionOptionItem in voteQuestionOptionList)
                    {
                        GetVoteQuestionOptionModel model = new GetVoteQuestionOptionModel
                        {
                            Describe = voteQuestionOptionItem.Describe,
                            Votes = voteQuestionOptionItem.Votes
                        };
                        voteQuestionOptionModels.Add(model);
                    }
                    questionModel.List = voteQuestionOptionModels;
                    questionModel.Title = item.Title;
                    list.Add(questionModel);
                }

                return new ApiResult<GetVoteOutput>(APIResultCode.Success, new GetVoteOutput
                {
                    Title = vote.Title,
                    List = list,
                    Deadline = vote.Deadline,
                    SmallDistrictArray = vote.SmallDistrictArray,
                    Summary = vote.Summary,
                    Url = _voteAnnexRepository.GetUrl(vote.Id.ToString())
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetVoteOutput>(APIResultCode.Success_NoB, new GetVoteOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 更改投票结果计算方式
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/updateCalculationMethod")]
        public async Task<ApiResult> UpdateCalculationMethod([FromUri]string id, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }

                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("投票Id信息为空！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
                }
                await _voteRepository.UpdateCalculationMethodAsync(new VoteDto
                {
                    Id = id,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
                return new ApiResult(APIResultCode.Success);

            }
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
        }

        /// <summary>
        /// 一个问题两个选项计算投票结果
        /// </summary>
        /// <param name="guid"></param>
        public static async Task AddVoteResultRecordAsync(Guid guid)
        {
            try
            {
                /*
                 * 查询投票主体
                 * 查询投票问题取第一条
                 * 根据投票问题查询投票选项取两条
                 * 比较两个投票选项结果
                 * 查询当前小区人数
                 * 记录投票结果和此投票期间有效人数
                 */
                IVoteRepository voteRepository = new VoteRepository();
                var voteEntity = await voteRepository.GetAsync(guid.ToString());

                IVoteQuestionRepository voteQuestionRepository = new VoteQuestionRepository();
                var voteQuestionList = await voteQuestionRepository.GetListAsync(new VoteQuestionDto() { VoteId = voteEntity.Id.ToString() });
                var voteQuestion = voteQuestionList[0];

                IVoteQuestionOptionRepository voteQuestionOptionRepository = new VoteQuestionOptionRepository();
                var voteQuestionOptionList = await voteQuestionOptionRepository.GetListAsync(new VoteQuestionOptionDto() { VoteId = voteEntity.Id.ToString(), VoteQuestionId = voteQuestion.Id.ToString() });
                var voteQuestionOption1 = voteQuestionOptionList[0];
                var voteQuestionOption2 = voteQuestionOptionList[1];

                IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                var ownerCertificationRecordList = await ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto { SmallDistrictId = voteEntity.SmallDistrictArray });

                VoteResult result = VoteResult.Overrule;
                if (voteEntity.DepartmentValue == CalculationMethod.EndorsedNumber.Value)
                {
                    if (voteQuestionOption1.Votes > (ownerCertificationRecordList.Count / 3) * 2)
                    {
                        result = VoteResult.Adopt;
                    }
                    result = VoteResult.Overrule;
                }
                if (voteEntity.DepartmentValue == CalculationMethod.Opposition.Value)
                {
                    if (voteQuestionOption2.Votes < (ownerCertificationRecordList.Count / 3))
                    {
                        result = VoteResult.Adopt;
                    }
                    result = VoteResult.Overrule;
                }
                IVoteResultRecordRepository voteResultRecordRepository = new VoteResultRecordRepository();
                var entity = await voteResultRecordRepository.AddAsync(new VoteResultRecordDto
                {
                    CalculationMethodValue = voteEntity.CalculationMethodValue,
                    CalculationMethodName = voteEntity.CalculationMethodName,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                    VoteId = voteEntity.Id.ToString(),
                    ResultValue = result.Value,
                    ResultName = result.Name
                });

            }
            catch (Exception)
            {

            }


        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly string AppId = "wx0bfc9becbe59d710";//与微信公众账号后台的AppId设置保持一致，区分大小写。

        /// <summary>
        /// 根据投票Id发送推送
        /// </summary>
        /// <param name="guid"></param>
        public async Task SeedAsync(Guid guid)
        {
            try
            {
                //投票id获取投票信息
                IVoteRepository voteRepository = new VoteRepository();
                var voteEntity = await voteRepository.GetAsync(guid.ToString());
                //投票小区范围  获取小区业主集合
                IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                IUserRepository userRepository = new UserRepository();
                var ownerCertificationRecordList = await ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto { SmallDistrictId = voteEntity.SmallDistrictArray });
                foreach (var item in ownerCertificationRecordList)
                {
                    var accessToken = AccessTokenContainer.GetAccessToken(WXController.AppId);
                    //更换成你需要的模板消息ID
                    string templateId = "eTflBDVcaZzGtjEbXvHzkQq--Rfnc12-VT4iNMjjlf0";//ConfigurationManager.AppSettings["WXTemplate_EmployeeRegisterRemind"].ToString();
                                                                                      //更换成对应的模板消息格式

                    //UserId
                    var userEntity = await userRepository.GetAsync(new UserDto { Id = item.Id.ToString() });
                    var templateData = new
                    {
                        first = new TemplateDataItem("门店员工注册通知"),
                        //  account = new TemplateDataItem(wxNickName),
                        time = new TemplateDataItem(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss\r\n")),
                        type = new TemplateDataItem("系统通知"),
                        remark = new TemplateDataItem(">>点击完成注册<<", "#FF0000")
                    };

                    var miniProgram = new TempleteModel_MiniProgram()
                    {
                        appid = "wx7f36e41455caec1b",//ZhiShiHuLian_WxOpenAppId,
                                                     //pagepath = "pages/editmyinfo/editmyinfo?id=" + employeeID
                    };

                    TemplateApi.SendTemplateMessage(AppId, userEntity.OpenId, templateId, null, templateData, miniProgram);
                }


            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }
    }
}
