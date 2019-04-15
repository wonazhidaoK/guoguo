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
        private readonly IVipOwnerRepository _vipOwnerRepository;
        private readonly IVoteAssociationVipOwnerRepository _voteAssociationVipOwnerRepository;
        private readonly IVipOwnerApplicationRecordRepository _vipOwnerApplicationRecordRepository;
        private readonly IVipOwnerCertificationRecordRepository _vipOwnerCertificationRecordRepository;
        private readonly IVoteRecordDetailRepository _voteRecordDetailRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IVoteResultRecordRepository _voteResultRecordRepository;
        private readonly ISmallDistrictRepository _smallDistrictRepository;
        private readonly IVoteRecordRepository _voteRecordRepository;
        private readonly IUserRepository _userRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="voteRepository"></param>
        /// <param name="voteQuestionRepository"></param>
        /// <param name="voteQuestionOptionRepository"></param>
        /// <param name="voteAnnexRepository"></param>
        /// <param name="vipOwnerRepository"></param>
        /// <param name="voteAssociationVipOwnerRepository"></param>
        /// <param name="vipOwnerApplicationRecordRepository"></param>
        /// <param name="vipOwnerCertificationRecordRepository"></param>
        /// <param name="voteRecordDetailRepository"></param>
        /// <param name="ownerCertificationRecordRepository"></param>
        /// <param name="voteResultRecordRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="smallDistrictRepository"></param>
        /// <param name="voteRecordRepository"></param>
        public VoteController(IVoteRepository voteRepository,
            IVoteQuestionRepository voteQuestionRepository,
            IVoteQuestionOptionRepository voteQuestionOptionRepository,
            IVoteAnnexRepository voteAnnexRepository,
            IVipOwnerRepository vipOwnerRepository,
            IVoteAssociationVipOwnerRepository voteAssociationVipOwnerRepository,
            IVipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository,
            IVipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository,
            IVoteRecordDetailRepository voteRecordDetailRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IVoteResultRecordRepository voteResultRecordRepository,
            IUserRepository userRepository,
            ISmallDistrictRepository smallDistrictRepository,
            IVoteRecordRepository voteRecordRepository)
        {
            _voteRepository = voteRepository;
            _voteQuestionRepository = voteQuestionRepository;
            _voteQuestionOptionRepository = voteQuestionOptionRepository;
            _voteAnnexRepository = voteAnnexRepository;
            _vipOwnerRepository = vipOwnerRepository;
            _voteAssociationVipOwnerRepository = voteAssociationVipOwnerRepository;
            _vipOwnerApplicationRecordRepository = vipOwnerApplicationRecordRepository;
            _vipOwnerCertificationRecordRepository = vipOwnerCertificationRecordRepository;
            _voteRecordDetailRepository = voteRecordDetailRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _voteResultRecordRepository = voteResultRecordRepository;
            _userRepository = userRepository;
            _smallDistrictRepository = smallDistrictRepository;
            _voteRecordRepository = voteRecordRepository;
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
         * 12.发起业委会改选投票
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
                if (input.List.Count < 1)
                {
                    throw new NotImplementedException("投票问题数量信息不正确！");
                }
                if (input.List[0].List.Count < 2)
                {
                    throw new NotImplementedException("投票问题选项数量信息不正确！");
                }
                if (!DateTimeOffset.TryParse(input.Deadline, out DateTimeOffset dateTime))
                {
                    throw new NotImplementedException("投票结束时间转换失败！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Unknown, new AddVoteForStreetOfficeOutput { }, APIResultMessage.TokenError);
                }
                var voteType = VoteTypes.GetAllForStreetOffice().Where(x => x.Value == input.VoteTypeValue).FirstOrDefault();
                if (voteType == null)
                {
                    throw new NotImplementedException("投票类型信息不准确！");
                }

                var nowTime = DateTimeOffset.Now;
                if (dateTime < nowTime)
                {
                    throw new NotImplementedException("投票结束时间小于投票创建时间！");
                }
                //增加投票主体
                var entity = await _voteRepository.AddForStreetOfficeAsync(new VoteDto
                {
                    Deadline = dateTime,
                    Title = input.Title,
                    Summary = input.Summary,
                    SmallDistrictArray = input.SmallDistrict,
                    //SmallDistrictId = user.SmallDistrictId,
                    //CommunityId = user.CommunityId,
                    StreetOfficeId = user.StreetOfficeId,
                    OperationTime = nowTime,
                    OperationUserId = user.Id.ToString(),
                    DepartmentValue = Department.JieDaoBan.Value,
                    DepartmentName = Department.JieDaoBan.Name,
                    VoteTypeName = voteType.Name,
                    VoteTypeValue = voteType.Value
                }, cancelToken);

                //增加投票附件
                if (!string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    await _voteAnnexRepository.AddAsync(new VoteAnnexDto
                    {
                        AnnexContent = input.AnnexId,
                        VoteId = entity.Id.ToString(),
                        OperationTime = nowTime,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);
                }

                //增加投票问题
                foreach (var item in input.List)
                {
                    var entityQuestion = await _voteQuestionRepository.AddAsync(new VoteQuestionDto
                    {
                        Title = item.Title,
                        OptionMode = "0",
                        VoteId = entity.Id.ToString(),
                        OperationTime = nowTime,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);

                    //增加投票问题选项
                    foreach (var questionOptions in item.List)
                    {
                        var questionOption = await _voteQuestionOptionRepository.AddAsync(new VoteQuestionOptionDto
                        {
                            Describe = questionOptions.Describe,
                            OperationTime = nowTime,
                            OperationUserId = user.Id.ToString(),
                            VoteId = entity.Id.ToString(),
                            VoteQuestionId = entityQuestion.Id.ToString()
                        });
                    }
                }

                //TODO发布投票同步推送
                //TODO发布投票添加定时任务计算投票结果
                //BackgroundJob.Schedule(() => AddVoteResultRecordAsync(entity.Id), entity.Deadline);
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
                if (!DateTimeOffset.TryParse(input.Deadline, out DateTimeOffset dateTime))
                {
                    throw new NotImplementedException("投票结束时间转换失败！");
                }
                if (input.List.Count != 1)
                {
                    throw new NotImplementedException("投票问题数量信息不正确！");
                }
                if (input.List[0].List.Count != 2)
                {
                    throw new NotImplementedException("投票问题选项数量信息不正确！");
                }
                var nowTime = DateTimeOffset.Now;
                if (dateTime < nowTime)
                {
                    throw new NotImplementedException("投票结束时间小于投票创建时间！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForPropertyOutput>(APIResultCode.Unknown, new AddVoteForPropertyOutput { }, APIResultMessage.TokenError);
                }

                //增加投票主体
                var entity = await _voteRepository.AddAsync(new VoteDto
                {
                    Deadline = dateTime,
                    Title = input.Title,
                    Summary = input.Summary,
                    SmallDistrictArray = user.SmallDistrictId,
                    SmallDistrictId = user.SmallDistrictId,
                    CommunityId = user.CommunityId,
                    StreetOfficeId = user.StreetOfficeId,
                    OperationTime = nowTime,
                    OperationUserId = user.Id.ToString(),
                    DepartmentValue = Department.WuYe.Value,
                    DepartmentName = Department.WuYe.Name,
                    VoteTypeValue = VoteTypes.Ordinary.Value,
                    VoteTypeName = VoteTypes.Ordinary.Name
                }, cancelToken);

                //增加投票附件
                if (!string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    await _voteAnnexRepository.AddAsync(new VoteAnnexDto
                    {
                        AnnexContent = input.AnnexId,
                        VoteId = entity.Id.ToString(),
                        OperationTime = nowTime,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);
                }

                //增加投票问题
                foreach (var item in input.List)
                {
                    var entityQuestion = await _voteQuestionRepository.AddAsync(new VoteQuestionDto
                    {
                        Title = item.Title,
                        OptionMode = "0",
                        VoteId = entity.Id.ToString(),
                        OperationTime = nowTime,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);

                    //增加投票问题选项
                    foreach (var questionOptions in item.List)
                    {
                        var questionOption = await _voteQuestionOptionRepository.AddAsync(new VoteQuestionOptionDto
                        {
                            Describe = questionOptions.Describe,
                            OperationTime = nowTime,
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
                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("投票业主认证信息为空！");
                }
                if (input.List.Count != 1)
                {
                    throw new NotImplementedException("投票问题数量信息不正确！");
                }
                if (input.List[0].List.Count != 2)
                {
                    throw new NotImplementedException("投票问题选项数量信息不正确！");
                }
                if (!DateTimeOffset.TryParse(input.Deadline, out DateTimeOffset dateTime))
                {
                    throw new NotImplementedException("投票结束时间转换失败！");
                }
                if (string.IsNullOrWhiteSpace(input.VoteTypeValue))
                {
                    throw new NotImplementedException("投票类型信息为空！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForVipOwnerOutput>(APIResultCode.Unknown, new AddVoteForVipOwnerOutput { }, APIResultMessage.TokenError);
                }

                var voteType = VoteTypes.GetAllForVipOwner().Where(x => x.Value == input.VoteTypeValue).FirstOrDefault();
                if (voteType == null)
                {
                    throw new NotImplementedException("投票类型信息不准确！");
                }
                var nowTime = DateTimeOffset.Now;
                if (dateTime < nowTime)
                {
                    throw new NotImplementedException("投票结束时间小于投票创建时间！");
                }
                //增加投票主体
                var entity = await _voteRepository.AddForVipOwnerAsync(new VoteDto
                {
                    Deadline = dateTime,
                    Title = input.Title,
                    Summary = input.Summary,
                    OwnerCertificationId = input.OwnerCertificationId,
                    OperationTime = nowTime,
                    OperationUserId = user.Id.ToString(),
                    DepartmentValue = Department.YeZhuWeiYuanHui.Value,
                    DepartmentName = Department.YeZhuWeiYuanHui.Name,
                    VoteTypeValue = voteType.Value,
                    VoteTypeName = voteType.Name,

                }, cancelToken);

                //增加投票附件
                if (!string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    await _voteAnnexRepository.AddAsync(new VoteAnnexDto
                    {
                        AnnexContent = input.AnnexId,
                        VoteId = entity.Id.ToString(),
                        OperationTime = nowTime,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);
                }

                //增加投票问题
                foreach (var item in input.List)
                {
                    var entityQuestion = await _voteQuestionRepository.AddAsync(new VoteQuestionDto
                    {
                        Title = item.Title,
                        OptionMode = "0",
                        VoteId = entity.Id.ToString(),
                        OperationTime = nowTime,
                        OperationUserId = user.Id.ToString()
                    }, cancelToken);

                    //增加投票问题选项
                    foreach (var questionOptions in item.List)
                    {
                        var questionOption = await _voteQuestionOptionRepository.AddAsync(new VoteQuestionOptionDto
                        {
                            Describe = questionOptions.Describe,
                            OperationTime = nowTime,
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
                    List = data.OrderByDescending(a => a.CreateOperationTime).Select(x => new GetForStreetOfficeOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Deadline = x.Deadline,
                        SmallDistrictArray = x.SmallDistrictArray,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        Summary = x.Summary,
                        StatusValue = x.StatusValue,
                        StatusName = x.StatusName,
                        CreateTime = x.CreateOperationTime.Value,
                        IsCreateUser = x.CreateOperationUserId == user.Id.ToString(),
                        IsProcessing = DateTimeOffset.Now < x.CreateOperationTime.Value
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
                    List = data.OrderByDescending(a => a.CreateOperationTime).Select(x => new GetForStreetOfficeOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Deadline = x.Deadline,
                        SmallDistrictArray = x.SmallDistrictArray,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        Summary = x.Summary,
                        StatusValue = x.StatusValue,
                        StatusName = x.StatusName,
                        CreateTime = x.CreateOperationTime.Value,
                        IsCreateUser = x.CreateOperationUserId == user.Id.ToString(),
                        IsProcessing = DateTimeOffset.Now < x.CreateOperationTime.Value
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllForPropertyOutput>(APIResultCode.Success_NoB, new GetAllForPropertyOutput { }, e.Message);
            }
        }

        ///// <summary>
        ///// 业委会查询投票列表(小程序用)
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="cancelToken"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("vote/getAllForVipOwner")]
        //public async Task<ApiResult<GetAllForVipOwnerOutput>> GetAllForVipOwner([FromUri]GetAllForVipOwnerInput input, CancellationToken cancelToken)
        //{
        //    try
        //    {
        //        var token = HttpContext.Current.Request.Headers["Authorization"];
        //        if (token == null)
        //        {
        //            return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Unknown, new GetAllForVipOwnerOutput { }, APIResultMessage.TokenNull);
        //        }

        //        if (input.PageIndex < 1)
        //        {
        //            input.PageIndex = 1;
        //        }
        //        if (input.PageSize < 1)
        //        {
        //            input.PageSize = 10;
        //        }
        //        int startRow = (input.PageIndex - 1) * input.PageSize;
        //        var user = _tokenManager.GetUser(token);
        //        if (user == null)
        //        {
        //            return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Unknown, new GetAllForVipOwnerOutput { }, APIResultMessage.TokenError);
        //        }
        //        var data = await _voteRepository.GetAllForVipOwnerAsync(new VoteDto
        //        {
        //            Title = input.Title,
        //            StreetOfficeId = user.StreetOfficeId,
        //            CommunityId = user.CommunityId,
        //            SmallDistrictId = user.SmallDistrictId,
        //            // DepartmentValue = Department.YeZhuWeiYuanHui.Value
        //        }, cancelToken);

        //        return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Success, new GetAllForVipOwnerOutput
        //        {
        //            List = data.Select(x => new GetForStreetOfficeOutput
        //            {
        //                Id = x.Id.ToString(),
        //                Title = x.Title,
        //                Deadline = x.Deadline,
        //                SmallDistrictArray = x.SmallDistrictArray,
        //                DepartmentName = x.DepartmentName,
        //                DepartmentValue = x.DepartmentValue,
        //                Summary = x.Summary
        //            }).Skip(startRow).Take(input.PageSize).ToList(),
        //            TotalCount = data.Count()
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Success_NoB, new GetAllForVipOwnerOutput { }, e.Message);
        //    }
        //}

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
                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.DepartmentValue))
                {
                    throw new NotImplementedException("部门值信息为空！");
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
                    DepartmentValue = input.DepartmentValue,
                    OwnerCertificationId = input.OwnerCertificationId
                    // DepartmentValue = Department.YeZhuWeiYuanHui.Value
                }, cancelToken);

                return new ApiResult<GetAllForOwnerOutput>(APIResultCode.Success, new GetAllForOwnerOutput
                {
                    List = data.OrderByDescending(a=>a.CreateOperationTime).Select(x => new GetForStreetOfficeOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Deadline = x.Deadline,
                        SmallDistrictArray = x.SmallDistrictArray,
                        DepartmentName = x.DepartmentName,
                        DepartmentValue = x.DepartmentValue,
                        Summary = x.Summary,
                        StatusValue = x.StatusValue,
                        StatusName = x.StatusName,
                        CreateTime = x.CreateOperationTime.Value,
                        IsCreateUser = x.CreateOperationUserId == user.Id.ToString(),
                        IsProcessing = DateTimeOffset.Now < x.CreateOperationTime.Value,
                        VoteTypeName = x.VoteTypeName,
                        VoteTypeValue = x.VoteTypeValue
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
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/get")]
        public async Task<ApiResult<GetVoteOutput>> Get([FromUri]GetVoteInput input, CancellationToken cancelToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input.Id))
                {
                    throw new NotImplementedException("投票Id信息为空！");
                }

                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetVoteOutput>(APIResultCode.Unknown, new GetVoteOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetVoteOutput>(APIResultCode.Unknown, new GetVoteOutput { }, APIResultMessage.TokenError);
                }
                var vote = await _voteRepository.GetAsync(input.Id, cancelToken);
                var voteRecordDetails = await _voteRecordDetailRepository.GetListAsync(new VoteRecordDetailDto
                {
                    VoteId = vote.Id.ToString(),
                    OperationUserId = user.Id.ToString()
                });
                var ownerCertificationRecordList = await _ownerCertificationRecordRepository.GetListForSmallDistrictIdAsync(new OwnerCertificationRecordDto
                {
                    SmallDistrictId = vote.SmallDistrictArray
                });

                var voteRecord = await _voteRecordRepository.GetForOwnerCertificationIdAsync(new VoteRecordDto
                {
                    OwnerCertificationId = input.OwnerCertificationId,
                    VoteId = input.Id
                }, cancelToken);

                //获取小区名称
                var smallDistrictEntity = await _smallDistrictRepository.GetAsync(vote.SmallDistrictArray, cancelToken);
                var userEntity = await _userRepository.GetForIdAsync(vote.CreateOperationUserId, cancelToken);
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
                            Id = voteQuestionOptionItem.Id.ToString(),
                            Describe = voteQuestionOptionItem.Describe,
                            Votes = voteQuestionOptionItem.Votes,
                        };
                        voteQuestionOptionModels.Add(model);
                    }
                    var voteResult = await _voteResultRecordRepository.GetForVoteQuestionIdAsync(new VoteResultRecordDto
                    {
                        VoteId = vote.Id.ToString(),
                        VoteQuestionId = item.Id.ToString()
                    });

                    questionModel.Id = item.Id.ToString();
                    questionModel.List = voteQuestionOptionModels;
                    questionModel.Title = item.Title;
                    questionModel.OptionMode = item.OptionMode;
                    questionModel.VoteResultName = voteResult != null ? voteResult.ResultName : "";
                    questionModel.VoteResultValue = voteResult != null ? voteResult.ResultValue : "";
                    list.Add(questionModel);
                }

                return new ApiResult<GetVoteOutput>(APIResultCode.Success, new GetVoteOutput
                {
                    Title = vote.Title,
                    List = list,
                    Deadline = vote.Deadline.ToString("yyyy-MM-dd"),
                    SmallDistrictArray = vote.SmallDistrictArray,
                    Summary = vote.Summary,
                    Url = _voteAnnexRepository.GetUrl(vote.Id.ToString()),
                    DepartmentName = vote.DepartmentName,
                    DepartmentValue = vote.DepartmentValue,
                    Id = vote.Id.ToString(),
                    IsVoted = voteRecordDetails.Count > 0,
                    StatusValue = vote.StatusValue,
                    ShouldParticipateCount = ownerCertificationRecordList.Count.ToString(),
                    VoteTypeName = vote.VoteTypeName,
                    VoteTypeValue = vote.VoteTypeValue,
                    CreateUserName = userEntity?.Name,
                    SmallDistrictArrayName = smallDistrictEntity.Name,
                    Feedback = voteRecord?.Feedback
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

        /*
         * 
         */

        /// <summary>
        /// 投票自动任务触发
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/timingVote")]
        public async Task Timing_Vote()
        {
            var list = await _voteRepository.GetListAsync(new VoteDto
            {
                OperationTime = DateTimeOffset.Now
            });
            if (list.Any())
            {
                foreach (var item in list)
                {
                    if (item.VoteTypeValue != VoteTypes.VipOwnerElection.Value)
                    {
                        BackgroundJob.Enqueue(() => AddVoteResultRecordIndefiniteAsync(item.Id));
                    }
                    else
                    {
                        BackgroundJob.Enqueue(() => AddVoteResultRecordAsync(item.Id));
                    }
                }
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
                    ResultName = result.Name,
                    VoteQuestionId = voteQuestion.Id.ToString()
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

        #region 

        // 业委会选举投票0.选择业委会1.多个问题（1个申请）2.两个选项 3.时间结束 4.增加高级认证记录 5.有高级认证
        /*
         * 发起业委会选举投票
         * 0.选择小区
         * 1.选择业委会
         * 2.添加参加竞选的人
         * 3.设置截至日期
         * 4.摘要
         * 
         */

        /// <summary>
        /// 街道办发起业委会选举投票
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vote/addVoteForVipOwnerElection")]
        public async Task<ApiResult<AddVoteForVipOwnerElectionOutput>> AddVoteForVipOwnerElection([FromBody]AddVoteForVipOwnerElectionInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVoteForVipOwnerElectionOutput>(APIResultCode.Unknown, new AddVoteForVipOwnerElectionOutput { }, APIResultMessage.TokenNull);
                }

                if (string.IsNullOrWhiteSpace(input.VipOwnerId))
                {
                    throw new NotImplementedException("投票业委会信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
                {
                    throw new NotImplementedException("投票范围小区信息为空！");
                }

                //if (input.List.Count < 1)
                //{
                //    throw new NotImplementedException("参选人员数量信息不正确！");
                //}

                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddVoteForVipOwnerElectionOutput>(APIResultCode.Unknown, new AddVoteForVipOwnerElectionOutput { }, APIResultMessage.TokenError);
                }
                var vipOwnerEntity = await _vipOwnerRepository.GetAsync(input.VipOwnerId, cancelToken);
                if (vipOwnerEntity == null)
                {
                    throw new NotImplementedException("业委会信息不正确！");
                }
                //增加投票主体
                var entity = await _voteRepository.AddForStreetOfficeAsync(new VoteDto
                {
                    Deadline = input.Deadline,
                    Title = vipOwnerEntity.Name + "选举投票",
                    Summary = input.Summary,
                    SmallDistrictArray = input.SmallDistrictId,
                    //SmallDistrictId = user.SmallDistrictId,
                    //CommunityId = user.CommunityId,
                    StreetOfficeId = user.StreetOfficeId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    DepartmentValue = Department.JieDaoBan.Value,
                    DepartmentName = Department.JieDaoBan.Name,
                    VoteTypeName = VoteTypes.VipOwnerElection.Name,
                    VoteTypeValue = VoteTypes.VipOwnerElection.Value
                }, cancelToken);

                //投票关联业委会
                var voteAssociationVipOwner = await _voteAssociationVipOwnerRepository.AddAsync(new VoteAssociationVipOwnerDto
                {
                    VipOwnerId = input.VipOwnerId,
                    VoteId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                });


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
                //查询小区下参与投票的申请
                var data = (await _vipOwnerApplicationRecordRepository.GetAllAsync(new VipOwnerApplicationRecordDto
                {
                    SmallDistrictId = input.SmallDistrictId
                }, cancelToken)).Where(x => x.IsAdopt == true).ToList();

                //var vipOwnerApplicationRecord = await _vipOwnerApplicationRecordRepository.GetAsync(item, cancelToken);
                var entityQuestion = await _voteQuestionRepository.AddAsync(new VoteQuestionDto
                {
                    Title = vipOwnerEntity.Name + "竞选",
                    OptionMode = "1",
                    VoteId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                //增加投票问题
                foreach (var item in data)
                {
                    var vipOwnerApplicationRecord = await _vipOwnerApplicationRecordRepository.GetAsync(item.Id.ToString(), cancelToken);
                    //var entityQuestion = await _voteQuestionRepository.AddAsync(new VoteQuestionDto
                    //{
                    //    Title = vipOwnerApplicationRecord.Name + "竞选" + vipOwnerApplicationRecord.StructureName,
                    //    OptionMode = "0",
                    //    VoteId = entity.Id.ToString(),
                    //    OperationTime = DateTimeOffset.Now,
                    //    OperationUserId = user.Id.ToString()
                    //}, cancelToken);

                    //增加投票问题选项
                    await _voteQuestionOptionRepository.AddAsync(new VoteQuestionOptionDto
                    {
                        Describe = item.Name, /*+ "竞选" + item.StructureName,*/
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString(),
                        VoteId = entity.Id.ToString(),
                        VoteQuestionId = entityQuestion.Id.ToString()
                    });

                    //await _voteQuestionOptionRepository.AddAsync(new VoteQuestionOptionDto
                    //{
                    //    Describe = "不同意",
                    //    OperationTime = DateTimeOffset.Now,
                    //    OperationUserId = user.Id.ToString(),
                    //    VoteId = entity.Id.ToString(),
                    //    VoteQuestionId = entityQuestion.Id.ToString()
                    //});

                    //更改高级认证申请数据
                    await _vipOwnerApplicationRecordRepository.UpdateVoteAsync(new VipOwnerApplicationRecordDto
                    {
                        // OwnerCertificationId = item,
                        VoteId = entity.Id.ToString(),
                        VoteQuestionId = entityQuestion.Id.ToString(),
                        Id = vipOwnerApplicationRecord.Id.ToString(),
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = user.Id.ToString(),
                    }, cancelToken);
                }

                //更改业委会竞选状态
                await _vipOwnerRepository.UpdateIsElectionAsync(new VipOwnerDto
                {
                    Id = input.VipOwnerId,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                }, cancelToken);

                //TODO发布投票同步推送

                //发布投票添加定时任务计算投票结果
                //BackgroundJob.Schedule(() => AddVoteResultRecordsAsync(entity.Id), entity.Deadline);
                return new ApiResult<AddVoteForVipOwnerElectionOutput>(APIResultCode.Success, new AddVoteForVipOwnerElectionOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddVoteForVipOwnerElectionOutput>(APIResultCode.Success_NoB, new AddVoteForVipOwnerElectionOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 一个投票多个选项
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static async Task AddVoteResultRecordIndefiniteAsync(Guid guid)
        {
            try
            {
                /*
                 * 查询投票主体
                 * 查询投票问题
                 * 比较两个投票选项结果
                 * 查询当前小区人数
                 * 记录投票结果和此投票期间有效人数
                 * 记录每个选项的投票结果
                 */
                IVoteRepository voteRepository = new VoteRepository();
                IVoteQuestionRepository voteQuestionRepository = new VoteQuestionRepository();
                IVoteQuestionOptionRepository voteQuestionOptionRepository = new VoteQuestionOptionRepository();
                IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                IVoteAssociationVipOwnerRepository voteAssociationVipOwnerRepository = new VoteAssociationVipOwnerRepository();
                IVipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository = new VipOwnerApplicationRecordRepository();
                IVipOwnerRepository vipOwnerRepository = new VipOwnerRepository();

                //查询投票主题
                var voteEntity = await voteRepository.GetAsync(guid.ToString());
                //查询投票(竞选人)问题
                var voteQuestionList = await voteQuestionRepository.GetListAsync(new VoteQuestionDto() { VoteId = voteEntity.Id.ToString() });
                //查询当前小区人数
                var ownerCertificationRecordList = await ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto { SmallDistrictId = voteEntity.SmallDistrictArray });
                var voteAssociationVipOwnerEntity = await voteAssociationVipOwnerRepository.GetForVoteIdAsync(voteEntity.Id.ToString());
                var vipOwner = await vipOwnerRepository.GetAsync(voteAssociationVipOwnerEntity.VipOwnerId);
                foreach (var item in voteQuestionList)
                {
                    //TODO 业主认证关联
                    var vipOwnerApplicationRecord = await vipOwnerApplicationRecordRepository.GetForVoteQuestionIdAsync(new VipOwnerApplicationRecordDto
                    {
                        VoteId = item.VoteId,
                        VoteQuestionId = item.Id.ToString()
                    });

                    //根据投票选项(竞选人)查找投票记录
                    var voteQuestionOptionList = await voteQuestionOptionRepository.GetListAsync(new VoteQuestionOptionDto() { VoteId = voteEntity.Id.ToString(), VoteQuestionId = item.Id.ToString() });
                    var voteQuestionOption1 = voteQuestionOptionList[0];
                    var voteQuestionOption2 = voteQuestionOptionList[1];
                    IVoteResultRecordRepository voteResultRecordRepository = new VoteResultRecordRepository();
                    IVipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository = new VipOwnerCertificationRecordRepository();
                    if (voteEntity.DepartmentValue == CalculationMethod.EndorsedNumber.Value)
                    {
                        VoteResult result = VoteResult.Overrule;

                        if (voteQuestionOption1.Votes > (ownerCertificationRecordList.Count / 3) * 2)
                        {
                            result = VoteResult.Adopt;
                            await vipOwnerCertificationRecordRepository.AddAsync(new VipOwnerCertificationRecordDto
                            {
                                OperationTime = DateTimeOffset.Now,
                                OperationUserId = "system",
                                VipOwnerId = voteAssociationVipOwnerEntity.VipOwnerId,
                                OwnerCertificationId = vipOwnerApplicationRecord.OwnerCertificationId,
                                UserId = vipOwnerApplicationRecord.UserId,
                                VipOwnerName = vipOwner.Name,
                                VipOwnerStructureId = vipOwnerApplicationRecord.StructureId,
                                VipOwnerStructureName = vipOwnerApplicationRecord.StructureName
                            });
                        }
                        else
                        {
                            result = VoteResult.Overrule;
                        }

                        var entity = await voteResultRecordRepository.AddAsync(new VoteResultRecordDto
                        {
                            CalculationMethodValue = voteEntity.CalculationMethodValue,
                            CalculationMethodName = voteEntity.CalculationMethodName,
                            OperationTime = DateTimeOffset.Now,
                            OperationUserId = "system",
                            VoteId = voteEntity.Id.ToString(),
                            ResultValue = result.Value,
                            ResultName = result.Name,
                            VoteQuestionId = item.Id.ToString()
                        });
                    }
                    if (voteEntity.DepartmentValue == CalculationMethod.Opposition.Value)
                    {
                        VoteResult result = VoteResult.Overrule;
                        if (voteQuestionOption2.Votes < (ownerCertificationRecordList.Count / 3))
                        {
                            result = VoteResult.Adopt;
                            await vipOwnerCertificationRecordRepository.AddAsync(new VipOwnerCertificationRecordDto
                            {
                                OperationTime = DateTimeOffset.Now,
                                OperationUserId = "system",
                                VipOwnerId = voteAssociationVipOwnerEntity.VipOwnerId,
                                OwnerCertificationId = vipOwnerApplicationRecord.OwnerCertificationId,
                                UserId = vipOwnerApplicationRecord.UserId,
                                VipOwnerName = vipOwner.Name,
                                VipOwnerStructureId = vipOwnerApplicationRecord.StructureId,
                                VipOwnerStructureName = vipOwnerApplicationRecord.StructureName
                            });
                        }
                        else
                        {
                            result = VoteResult.Overrule;
                        }
                        var entity = await voteResultRecordRepository.AddAsync(new VoteResultRecordDto
                        {
                            CalculationMethodValue = voteEntity.CalculationMethodValue,
                            CalculationMethodName = voteEntity.CalculationMethodName,
                            OperationTime = DateTimeOffset.Now,
                            OperationUserId = "system",
                            VoteId = voteEntity.Id.ToString(),
                            ResultValue = result.Value,
                            ResultName = result.Name,
                            VoteQuestionId = item.Id.ToString()
                        });
                    }
                }


                //var voteQuestion = voteQuestionList[0];

                //IVoteQuestionOptionRepository voteQuestionOptionRepository = new VoteQuestionOptionRepository();
                //var voteQuestionOptionList = await voteQuestionOptionRepository.GetListAsync(new VoteQuestionOptionDto() { VoteId = voteEntity.Id.ToString(), VoteQuestionId = voteQuestion.Id.ToString() });
                //var voteQuestionOption1 = voteQuestionOptionList[0];
                //var voteQuestionOption2 = voteQuestionOptionList[1];

                //IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                //var ownerCertificationRecordList = await ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto { SmallDistrictId = voteEntity.SmallDistrictArray });

                //VoteResult result = VoteResult.Overrule;
                //if (voteEntity.DepartmentValue == CalculationMethod.EndorsedNumber.Value)
                //{
                //    if (voteQuestionOption1.Votes > (ownerCertificationRecordList.Count / 3) * 2)
                //    {
                //        result = VoteResult.Adopt;
                //    }
                //    result = VoteResult.Overrule;
                //}
                //if (voteEntity.DepartmentValue == CalculationMethod.Opposition.Value)
                //{
                //    if (voteQuestionOption2.Votes < (ownerCertificationRecordList.Count / 3))
                //    {
                //        result = VoteResult.Adopt;
                //    }
                //    result = VoteResult.Overrule;
                //}
                //TODO 更改历史业委会有效状态

            }
            catch (Exception)
            {

            }


        }

        #endregion
    }
}
