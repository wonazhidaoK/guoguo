﻿using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.API.Models.Vote;
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
         * 7.干预投票结果计算方式
         * 8.业主投票
         * 9.业主查看投票详情
         * 10.业主查看投票列表
         * 11.投票详情
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

                if (input.List.Count < 1)
                {
                    throw new NotImplementedException("投票问题信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForPropertyOutput>(APIResultCode.Unknown, new AddVoteForPropertyOutput { }, APIResultMessage.TokenError);
                }

                //增加投票主题
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

                if (input.List.Count < 1)
                {
                    throw new NotImplementedException("投票问题信息为空！");
                }

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForVipOwnerOutput>(APIResultCode.Unknown, new AddVoteForVipOwnerOutput { }, APIResultMessage.TokenError);
                }

                //增加投票主题
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

    }
}