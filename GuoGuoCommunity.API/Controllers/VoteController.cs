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
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 投票相关
    /// </summary>
    public class VoteController : BaseController
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IVoteQuestionRepository _voteQuestionRepository;
        private readonly IVoteQuestionOptionRepository _voteQuestionOptionRepository;
        private readonly IVoteRecordRepository _voteRecordRepository;
        private readonly IVoteAnnexRepository _voteAnnexRepository;
        private readonly IVoteRecordDetailRepository _voteRecordDetailRepository;
        private readonly IVoteResultRecordRepository _voteResultRecordRepository;
        private readonly IVoteAssociationVipOwnerRepository _voteAssociationVipOwnerRepository;
        private readonly IVipOwnerRepository _vipOwnerRepository;
        private readonly IVipOwnerApplicationRecordRepository _vipOwnerApplicationRecordRepository;
        private readonly IVipOwnerCertificationRecordRepository _vipOwnerCertificationRecordRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly ISmallDistrictRepository _smallDistrictRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IVipOwnerCertificationAnnexRepository _vipOwnerCertificationAnnexRepository;
        private readonly IVipOwnerCertificationConditionRepository _vipOwnerCertificationConditionRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IStreetOfficeRepository _streetOfficeRepository;
        private readonly IWeiXinUserRepository _weiXinUserRepository;
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
        /// <param name="announcementRepository"></param>
        /// <param name="vipOwnerCertificationAnnexRepository"></param>
        /// <param name="vipOwnerCertificationConditionRepository"></param>
        /// <param name="streetOfficeRepository"></param>
        /// <param name="ownerRepository"></param>
        /// <param name="weiXinUserRepository"></param>
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
            IVoteRecordRepository voteRecordRepository,
            IAnnouncementRepository announcementRepository,
            IVipOwnerCertificationAnnexRepository vipOwnerCertificationAnnexRepository,
            IVipOwnerCertificationConditionRepository vipOwnerCertificationConditionRepository,
            IStreetOfficeRepository streetOfficeRepository,
            IOwnerRepository ownerRepository,
            IWeiXinUserRepository weiXinUserRepository)
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
            _announcementRepository = announcementRepository;
            _vipOwnerCertificationAnnexRepository = vipOwnerCertificationAnnexRepository;
            _vipOwnerCertificationConditionRepository = vipOwnerCertificationConditionRepository;
            _ownerRepository = ownerRepository;
            _streetOfficeRepository = streetOfficeRepository;
            _weiXinUserRepository = weiXinUserRepository;
            _tokenManager = new TokenManager();
        }

        #region 发起投票

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
            if (Authorization == null)
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

            var user = _tokenManager.GetUser(Authorization);
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
            var ownerCertificationRecordList = await _ownerCertificationRecordRepository.GetListForSmallDistrictIdAsync(new OwnerCertificationRecordDto
            {
                SmallDistrictId = input.SmallDistrict
            });
            if (!ownerCertificationRecordList.Any())
            {
                throw new NotImplementedException("所选小区人数为0不能发起投票！");
            }

            var entity = await _voteRepository.AddForStreetOfficeAsync(new VoteDto
            {
                Deadline = dateTime,
                Title = input.Title,
                Summary = input.Summary,
                SmallDistrictArray = input.SmallDistrict,
                StreetOfficeId = user.StreetOfficeId,
                OperationTime = nowTime,
                OperationUserId = user.Id.ToString(),
                DepartmentValue = Department.JieDaoBan.Value,
                DepartmentName = Department.JieDaoBan.Name,
                VoteTypeName = voteType.Name,
                VoteTypeValue = voteType.Value
            }, cancelToken);

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
            BackgroundJob.Enqueue(() => SeedVoteAsync(entity.Id));
            return new ApiResult<AddVoteForStreetOfficeOutput>(APIResultCode.Success, new AddVoteForStreetOfficeOutput { Id = entity.Id.ToString() });
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
            if (Authorization == null)
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

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddVoteForPropertyOutput>(APIResultCode.Unknown, new AddVoteForPropertyOutput { }, APIResultMessage.TokenError);
            }
            var ownerCertificationRecordList = await _ownerCertificationRecordRepository.GetListForSmallDistrictIdAsync(new OwnerCertificationRecordDto
            {
                SmallDistrictId = user.SmallDistrictId
            });
            if (!ownerCertificationRecordList.Any())
            {
                throw new NotImplementedException("当前小区人数为0不能发起投票！");
            }
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

            BackgroundJob.Enqueue(() => SeedVoteAsync(entity.Id));
            return new ApiResult<AddVoteForPropertyOutput>(APIResultCode.Success, new AddVoteForPropertyOutput { Id = entity.Id.ToString() });
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
            if (Authorization == null)
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

            var user = _tokenManager.GetUser(Authorization);
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
            BackgroundJob.Enqueue(() => SeedVoteAsync(entity.Id));
            return new ApiResult<AddVoteForVipOwnerOutput>(APIResultCode.Success, new AddVoteForVipOwnerOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 街道办发起业委会选举投票
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("vote/addVoteForVipOwnerElection")]
        public async Task<ApiResult<AddVoteForVipOwnerElectionOutput>> AddVoteForVipOwnerElection([FromBody]AddVoteForVipOwnerElectionInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
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
            if (input.ElectionNumber < 1)
            {
                throw new NotImplementedException("投票数信息不准确！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddVoteForVipOwnerElectionOutput>(APIResultCode.Unknown, new AddVoteForVipOwnerElectionOutput { }, APIResultMessage.TokenError);
            }
            var vipOwnerEntity = await _vipOwnerRepository.GetAsync(input.VipOwnerId, cancelToken);
            if (vipOwnerEntity == null)
            {
                throw new NotImplementedException("业委会信息不正确！");
            }
            else if (vipOwnerEntity.IsElection)
            {
                throw new NotImplementedException("该小区存在竞选中的投票！不能发起业委会选举投票！");
            }
            var vipOwnerApplicationRecordList = (await _vipOwnerApplicationRecordRepository.GetAllInvalidAsync(new VipOwnerApplicationRecordDto
            {
                SmallDistrictId = input.SmallDistrictId
            }, cancelToken)).Where(x => x.IsAdopt == true).ToList();

            if (vipOwnerApplicationRecordList.Count < input.ElectionNumber)
            {
                throw new NotImplementedException("当前通过申请人数为" + vipOwnerApplicationRecordList.Count + "！投票数大于竞选人数！");
            }
            var ownerCertificationRecordList = await _ownerCertificationRecordRepository.GetListForSmallDistrictIdAsync(new OwnerCertificationRecordDto
            {
                SmallDistrictId = input.SmallDistrictId
            });
            if (!ownerCertificationRecordList.Any())
            {
                throw new NotImplementedException("所选小区人数为0不能发起投票！");
            }
            if (!DateTimeOffset.TryParse(input.Deadline, out DateTimeOffset dateTime))
            {
                throw new NotImplementedException("投票结束时间转换失败！");
            }
            var nowTime = DateTimeOffset.Now;
            if (dateTime < nowTime)
            {
                throw new NotImplementedException("投票结束时间小于投票创建时间！");
            }

            var entity = await _voteRepository.AddForStreetOfficeAsync(new VoteDto
            {
                Deadline = dateTime,
                Title = vipOwnerEntity.Name + "选举投票",
                Summary = input.Summary,
                SmallDistrictArray = input.SmallDistrictId,
                StreetOfficeId = user.StreetOfficeId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                DepartmentValue = Department.JieDaoBan.Value,
                DepartmentName = Department.JieDaoBan.Name,
                VoteTypeName = VoteTypes.VipOwnerElection.Name,
                VoteTypeValue = VoteTypes.VipOwnerElection.Value
            }, cancelToken);

            var voteAssociationVipOwner = await _voteAssociationVipOwnerRepository.AddAsync(new VoteAssociationVipOwnerDto
            {
                VipOwnerId = input.VipOwnerId,
                VoteId = entity.Id.ToString(),
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                ElectionNumber = input.ElectionNumber
            });

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

            var entityQuestion = await _voteQuestionRepository.AddAsync(new VoteQuestionDto
            {
                Title = vipOwnerEntity.Name + "竞选",
                OptionMode = "1",
                VoteId = entity.Id.ToString(),
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);

            foreach (var item in vipOwnerApplicationRecordList)
            {
                var vipOwnerApplicationRecord = await _vipOwnerApplicationRecordRepository.GetAsync(item.Id.ToString(), cancelToken);

                var entityOption = await _voteQuestionOptionRepository.AddAsync(new VoteQuestionOptionDto
                {
                    Describe = item.Name,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    VoteId = entity.Id.ToString(),
                    VoteQuestionId = entityQuestion.Id.ToString(),

                });
                await _vipOwnerApplicationRecordRepository.UpdateVoteAsync(new VipOwnerApplicationRecordDto
                {
                    VoteId = entity.Id.ToString(),
                    VoteQuestionId = entityQuestion.Id.ToString(),
                    VoteQuestionOptionId = entityOption.Id.ToString(),
                    Id = vipOwnerApplicationRecord.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    VipOwnerId = vipOwnerEntity.Id.ToString(),
                    VipOwnerName = vipOwnerEntity.Name
                }, cancelToken);
            }

            await _vipOwnerRepository.UpdateIsElectionAsync(new VipOwnerDto
            {
                Id = input.VipOwnerId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),

            }, cancelToken);

            BackgroundJob.Enqueue(() => SeedVoteAsync(entity.Id));
            return new ApiResult<AddVoteForVipOwnerElectionOutput>(APIResultCode.Success, new AddVoteForVipOwnerElectionOutput { Id = entity.Id.ToString() });
        }

        #endregion

        #region 投票展示

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
            if (Authorization == null)
            {
                return new ApiResult<GetAllForStreetOfficeOutput>(APIResultCode.Unknown, new GetAllForStreetOfficeOutput { }, APIResultMessage.TokenNull);
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
            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }
            int startRow = (input.PageIndex - 1) * input.PageSize;

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllForStreetOfficeOutput>(APIResultCode.Unknown, new GetAllForStreetOfficeOutput { }, APIResultMessage.TokenError);
            }
            var data = await _voteRepository.GetAllForStreetOfficeAsync(new VoteDto
            {
                SmallDistrictArray = input.SmallDistrictArray,
                Title = input.Title,
                StreetOfficeId = user.StreetOfficeId,
                DepartmentValue = Department.JieDaoBan.Value,
                StartTime = startTime,
                EndTime = endTime,
                StatusValue = input.StatusValue
            }, cancelToken);

            var listCount = data.Count();
            var list = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllForStreetOfficeOutput>(APIResultCode.Success, new GetAllForStreetOfficeOutput
            {
                List = list.Select(x => new GetForStreetOfficeOutput
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
                    VoteTypeValue = x.VoteTypeValue
                }).ToList(),
                TotalCount = listCount
            });
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
            if (Authorization == null)
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
            int startRow = (input.PageIndex - 1) * input.PageSize;

            var user = _tokenManager.GetUser(Authorization);
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
                StartTime = startTime,
                EndTime = endTime,
                StatusValue = input.StatusValue
            }, cancelToken);

            var listCount = data.Count();
            var list = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllForPropertyOutput>(APIResultCode.Success, new GetAllForPropertyOutput
            {
                List = list.Select(x => new GetForStreetOfficeOutput
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
                }).ToList(),
                TotalCount = listCount
            });
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
            if (Authorization == null)
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

            var user = _tokenManager.GetUser(Authorization);
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
            }, cancelToken);

            var listCount = data.Count();
            var list = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize);

            return new ApiResult<GetAllForOwnerOutput>(APIResultCode.Success, new GetAllForOwnerOutput
            {
                List = list.Select(x => new GetForStreetOfficeOutput
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
                    IsProcessing = DateTimeOffset.Now < x.Deadline,
                    VoteTypeName = x.VoteTypeName,
                    VoteTypeValue = x.VoteTypeValue
                }).ToList(),
                TotalCount = listCount
            });
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
            if (string.IsNullOrWhiteSpace(input.Id))
            {
                throw new NotImplementedException("投票Id信息为空！");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetVoteOutput>(APIResultCode.Unknown, new GetVoteOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
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
            var smallDistrictEntity = await _smallDistrictRepository.GetAsync(vote.SmallDistrictArray, cancelToken);
            var userEntity = await _userRepository.GetForIdAsync(vote.CreateOperationUserId, cancelToken);
            var OperationName = userEntity?.Name;
            if (vote.DepartmentValue == Department.YeZhuWeiYuanHui.Value)
            {
                OperationName = (await _ownerCertificationRecordRepository.GetIncludeAsync(vote.OwnerCertificationId, cancelToken))?.Owner.Name;
            }
            var voteQuestionList = await _voteQuestionRepository.GetListAsync(new VoteQuestionDto { VoteId = vote?.Id.ToString() }, cancelToken);
            List<GetVoteQuestionModel> list = new List<GetVoteQuestionModel>();
            foreach (var item in voteQuestionList)
            {
                GetVoteQuestionModel questionModel = new GetVoteQuestionModel();
                List<GetVoteQuestionOptionModel> voteQuestionOptionModels = new List<GetVoteQuestionOptionModel>();
                var voteQuestionOptionList = await _voteQuestionOptionRepository.GetListAsync(new VoteQuestionOptionDto { VoteId = vote?.Id.ToString(), VoteQuestionId = item.Id.ToString() }, cancelToken);
                var voteResult = (await _voteResultRecordRepository.GetListForVoteIdAsync(vote.Id.ToString())).FirstOrDefault();
                foreach (var voteQuestionOptionItem in voteQuestionOptionList)
                {
                    GetVoteQuestionOptionModel model = new GetVoteQuestionOptionModel
                    {
                        Id = voteQuestionOptionItem.Id.ToString(),
                        Describe = voteQuestionOptionItem.Describe,
                        Votes = voteQuestionOptionItem.Votes,
                        Headimgurl = "https://www.guoguoshequ.com/icon-mrtx.png"
                    };
                    if (vote.VoteTypeValue != VoteTypes.VipOwnerElection.Value)
                    {
                        if (voteResult != null)
                        {
                            if (voteQuestionOptionItem.Describe == "同意")
                            {
                                if (voteResult.CalculationMethodValue == CalculationMethod.Opposition.Value)
                                {
                                    model.Votes = model.Votes + (voteResult.ShouldParticipateCount.Value - voteResult.ActualParticipateCount);
                                }
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            var vipOwnerApplicationRecord = await _vipOwnerApplicationRecordRepository.GetForVoteQuestionOptionIdAsync(new VipOwnerApplicationRecordDto
                            {
                                VoteId = voteQuestionOptionItem.VoteId,
                                VoteQuestionId = voteQuestionOptionItem.VoteQuestionId,
                                VoteQuestionOptionId = voteQuestionOptionItem.Id.ToString()
                            });
                            var ownerCertificationRecord = await _ownerCertificationRecordRepository.GetAsync(vipOwnerApplicationRecord.OwnerCertificationId.ToString(), cancelToken);
                            var ownerCertificationRecordUser = await _userRepository.GetForIdAsync(ownerCertificationRecord.UserId);
                            var weiXinUser = await _weiXinUserRepository.GetAsync(ownerCertificationRecordUser.UnionId);
                            model.Headimgurl = weiXinUser.Headimgurl;
                        }
                        catch (Exception)
                        {

                        }

                    }
                    voteQuestionOptionModels.Add(model);
                }
                if (vote.VoteTypeValue == VoteTypes.VipOwnerElection.Value)
                {
                    var voteAssociationVipOwner = await _voteAssociationVipOwnerRepository.GetForVoteIdAsync(vote.Id.ToString(), cancelToken);
                    questionModel.ElectionNumber = voteAssociationVipOwner.ElectionNumber;
                }
                else
                {
                    questionModel.ElectionNumber = 1;
                }
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
                CreateUserName = OperationName,
                SmallDistrictArrayName = smallDistrictEntity.Name,
                Feedback = voteRecord?.Feedback
            });
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
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("投票Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
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

        /// <summary>
        /// 投票自动任务触发
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/timingVote")]
        public async Task Timing_Vote()
        {
            var list = await _voteRepository.GetDeadListAsync(new VoteDto
            {
                OperationTime = DateTimeOffset.Now
            });
            if (list.Any())
            {
                foreach (var item in list)
                {
                    if (item.VoteTypeValue == VoteTypes.VipOwnerElection.Value)
                    {
                        await AddVoteResultRecordIndefiniteAsync(item.Id);
                    }
                    else
                    {
                        await AddVoteResultRecordAsync(item.Id);
                    }
                }
            }
        }

        /// <summary>
        /// 根据投票问题选项id获取用户信息(业委会该选投票展示用户信息用)
        /// </summary>
        /// <param name="voteQuestionOptionId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("vote/getUserInfo")]
        public async Task<ApiResult<GetUserInfoOutput>> GetUserInfo([FromUri]string voteQuestionOptionId, CancellationToken cancelToken)
        {
            if (string.IsNullOrWhiteSpace(voteQuestionOptionId))
            {
                throw new NotImplementedException("投票问题选项Id信息为空！");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetUserInfoOutput>(APIResultCode.Unknown, new GetUserInfoOutput(), APIResultMessage.TokenNull);
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetUserInfoOutput>(APIResultCode.Unknown, new GetUserInfoOutput(), APIResultMessage.TokenError);
            }
            var voteQuestionOption = await _voteQuestionOptionRepository.GetAsync(voteQuestionOptionId, cancelToken);
            if (voteQuestionOption == null)
            {
                throw new NotImplementedException("投票问题选项信息为空！");
            }
            var vote = await _voteRepository.GetAsync(voteQuestionOption.VoteId, cancelToken);
            if (vote.VoteTypeValue != VoteTypes.VipOwnerElection.Value)
            {
                throw new NotImplementedException("投票类型不为业委会重组！");
            }

            var vipOwnerApplicationRecord = await _vipOwnerApplicationRecordRepository.GetForVoteQuestionOptionIdAsync(new VipOwnerApplicationRecordDto
            {
                VoteId = voteQuestionOption?.VoteId,
                VoteQuestionId = voteQuestionOption?.VoteQuestionId,
                VoteQuestionOptionId = voteQuestionOption?.Id.ToString()
            });
            var annexList = await _vipOwnerCertificationAnnexRepository.GetListAsync(new VipOwnerCertificationAnnexDto
            {
                ApplicationRecordId = vipOwnerApplicationRecord.Id.ToString()
            }, cancelToken);
            var certificationConditionList = await _vipOwnerCertificationConditionRepository.GetAllAsync(new VipOwnerCertificationConditionDto { }, cancelToken);
            List<AnnexModel> list = new List<AnnexModel>();
            foreach (var item in annexList)
            {
                if (Guid.TryParse(item.CertificationConditionId, out var uid))
                {
                }
                var certificationCondition = certificationConditionList.Where(x => x.Id == uid).FirstOrDefault();
                list.Add(new AnnexModel
                {
                    CertificationConditionName = certificationCondition.Title,
                    CertificationConditionId = item.ApplicationRecordId,
                    ID = item.Id.ToString(),
                    TypeValue = certificationCondition.TypeValue,
                    Url = item.AnnexContent
                });
            }
            var ownerCertificationRecord = await _ownerCertificationRecordRepository.GetIncludeAsync(vipOwnerApplicationRecord?.OwnerCertificationId.ToString(), cancelToken);
            var streetOffice = await _streetOfficeRepository.GetAsync(ownerCertificationRecord?.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOfficeId.ToString(), cancelToken);
            var owner = await _ownerRepository.GetAsync(ownerCertificationRecord?.OwnerId.ToString(), cancelToken);
            var userEntity = await _userRepository.GetForIdAsync(ownerCertificationRecord?.UserId);
            var weiXinUser = await _weiXinUserRepository.GetAsync(userEntity?.UnionId);
            var age = 18;
            if (DateTimeOffset.TryParse(owner?.Birthday, out DateTimeOffset birthdaytimeOffset))
            {
                age = DateTimeOffset.Now.Year - birthdaytimeOffset.Year;
            }
            return new ApiResult<GetUserInfoOutput>(APIResultCode.Success, new GetUserInfoOutput
            {
                Address = streetOffice?.State + "省" + streetOffice?.City + "市" + streetOffice?.Region + ownerCertificationRecord?.Industry.BuildingUnit.Building.SmallDistrict.Name + " " + ownerCertificationRecord?.Industry.BuildingUnit.Building.Name + ownerCertificationRecord?.Industry.BuildingUnit.UnitName + ownerCertificationRecord?.Industry.Name,
                Birthday = owner?.Birthday,
                Gender = owner?.Gender,
                Headimgurl = weiXinUser != null ? weiXinUser?.Headimgurl : "https://www.guoguoshequ.com/icon-mrtx.png",
                List = list,
                Name = owner?.Name,
                Reason = vipOwnerApplicationRecord?.Reason,
                StructureName = vipOwnerApplicationRecord?.StructureName,
                Age = age
            });
        }
        #endregion

        #region 推送

        /// <summary>
        /// (发起投票)根据投票Id发送推送
        /// </summary>
        /// <param name="guid"></param>
        public async static Task SeedVoteAsync(Guid guid)
        {
            try
            {
                IVoteRepository voteRepository = new VoteRepository();
                var voteEntity = await voteRepository.GetAsync(guid.ToString());
                IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                IUserRepository userRepository = new UserRepository();
                var ownerCertificationRecordList = await ownerCertificationRecordRepository.GetListForSmallDistrictIdIncludeAsync(new OwnerCertificationRecordDto { SmallDistrictId = voteEntity.SmallDistrictArray });

                foreach (var item in ownerCertificationRecordList)
                {
                    try
                    {
                        var accessToken = AccessTokenContainer.GetAccessToken(WXController.AppId);

                        var userEntity = await userRepository.GetForIdAsync(item.UserId);
                        IWeiXinUserRepository weiXinUserRepository = new WeiXinUserRepository();
                        var weiXinUser = await weiXinUserRepository.GetAsync(userEntity.UnionId);
                        var templateData = new
                        {
                            first = new TemplateDataItem("请参与投票"),
                            keyword1 = new TemplateDataItem(voteEntity.Title),
                            keyword2 = new TemplateDataItem(item.Industry.BuildingUnit.Building.SmallDistrict.Name),
                            keyword3 = new TemplateDataItem(voteEntity.CreateOperationTime.Value.ToString("yyyy年MM月dd日 HH:mm:ss\r\n")),
                            keyword4 = new TemplateDataItem(voteEntity.Deadline.ToString("yyyy年MM月dd日")),
                            remark = new TemplateDataItem(">>请点击参与投票，感谢你的参与<<", "#FF0000")
                        };

                        var miniProgram = new TempleteModel_MiniProgram()
                        {
                            appid = GuoGuoCommunity_WxOpenAppId, //ZhiShiHuLian_WxOpenAppId,
                            pagepath = "pages/voteDetail/voteDetail?id=" + voteEntity.Id.ToString()
                        };

                        TemplateApi.SendTemplateMessage(AppId, weiXinUser.OpenId, VoteCreateTemplateId, null, templateData, miniProgram);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// (投票结果产生)根据投票Id发送推送
        /// </summary>
        /// <param name="guid"></param>
        public static async Task SeedResultAsync(Guid guid)
        {
            try
            {
                IVoteRepository voteRepository = new VoteRepository();
                var voteEntity = await voteRepository.GetAsync(guid.ToString());

                IVoteResultRecordRepository voteResultRecordRepository = new VoteResultRecordRepository();
                var voteResultRecord = await voteResultRecordRepository.GetForVoteIdAsync(guid.ToString());
                IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                var ownerCertificationRecordList = (await ownerCertificationRecordRepository.GetListForSmallDistrictIdIncludeAsync(new OwnerCertificationRecordDto { SmallDistrictId = voteEntity.SmallDistrictArray })).ToList().Distinct();

                IUserRepository userRepository = new UserRepository();
                foreach (var item in ownerCertificationRecordList)
                {
                    var accessToken = AccessTokenContainer.GetAccessToken(WXController.AppId);

                    var userEntity = await userRepository.GetForIdAsync(item.UserId);
                    IWeiXinUserRepository weiXinUserRepository = new WeiXinUserRepository();
                    var weiXinUser = await weiXinUserRepository.GetAsync(userEntity.UnionId);
                    var templateData = new
                    {
                        first = new TemplateDataItem("您小区内的投票结果已产生"),
                        keyword1 = new TemplateDataItem(voteEntity.Title),
                        keyword2 = new TemplateDataItem(item.Industry.BuildingUnit.Building.SmallDistrict.Name),
                        keyword3 = new TemplateDataItem(DateTimeOffset.Now.ToString("yyyy年MM月dd日 HH:mm:ss\r\n")),
                        remark = new TemplateDataItem(">>点击查看投票结果<<", "#FF0000")
                    };

                    var miniProgram = new TempleteModel_MiniProgram()
                    {
                        appid = GuoGuoCommunity_WxOpenAppId,
                        pagepath = "pages/voteDetail/voteDetail?id=" + voteEntity.Id.ToString()
                    };

                    TemplateApi.SendTemplateMessage(AppId, weiXinUser.OpenId, VoteResultTemplateId, null, templateData, miniProgram);
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 高级认证通过推送
        /// </summary>
        /// <returns></returns>
        public static async Task SeedVipOwnerCertificationRecordAsync(string ownerCertificationId)
        {
            try
            {
                IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                IUserRepository userRepository = new UserRepository();
                var ownerCertificationRecord = await ownerCertificationRecordRepository.GetAsync(ownerCertificationId);
                IOwnerRepository ownerRepository = new OwnerRepository();

                var owner = await ownerRepository.GetForOwnerCertificationRecordIdAsync(new OwnerDto { OwnerCertificationRecordId = ownerCertificationRecord.Id.ToString() });
                var userEntity = await userRepository.GetForIdAsync(ownerCertificationRecord.UserId);
                IWeiXinUserRepository weiXinUserRepository = new WeiXinUserRepository();
                var weiXinUser = await weiXinUserRepository.GetAsync(userEntity.UnionId);

                var templateData = new
                {
                    first = new TemplateDataItem("您的高级认证申请已通过"),
                    keyword1 = new TemplateDataItem(owner.Name),
                    keyword2 = new TemplateDataItem(owner.IDCard),
                    keyword3 = new TemplateDataItem(owner.PhoneNumber),
                    keyword4 = new TemplateDataItem(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss\r\n")),
                    remark = new TemplateDataItem(">>点击查看详情<<", "#FF0000")
                };

                var miniProgram = new TempleteModel_MiniProgram()
                {
                    appid = GuoGuoCommunity_WxOpenAppId,
                    pagepath = "pages/my/my"
                };

                TemplateApi.SendTemplateMessage(AppId, weiXinUser.OpenId, VipOwnerCertificationRecordTemplateId, null, templateData, miniProgram);
            }
            catch (Exception)
            {

            }
        }

        #endregion

        #region 投票结果计算

        /// <summary>
        /// 一个问题两个选项计算投票结果
        /// </summary>
        /// <param name="guid"></param>
        public static async Task AddVoteResultRecordAsync(Guid guid)
        {
            try
            {
                IVoteRepository voteRepository = new VoteRepository();
                var voteEntity = await voteRepository.GetAsync(guid.ToString());

                IVoteQuestionRepository voteQuestionRepository = new VoteQuestionRepository();
                var voteQuestionList = await voteQuestionRepository.GetListAsync(new VoteQuestionDto() { VoteId = voteEntity.Id.ToString() });
                var voteQuestion = voteQuestionList[0];

                IVoteQuestionOptionRepository voteQuestionOptionRepository = new VoteQuestionOptionRepository();
                var voteQuestionOptionList = await voteQuestionOptionRepository.GetListAsync(new VoteQuestionOptionDto() { VoteId = voteEntity.Id.ToString(), VoteQuestionId = voteQuestion.Id.ToString() });
                var voteQuestionOption1 = voteQuestionOptionList.Where(x => x.Describe == "同意").FirstOrDefault();
                var voteQuestionOption2 = voteQuestionOptionList.Where(x => x.Describe == "不同意").FirstOrDefault();

                IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                var ownerCertificationRecordList = await ownerCertificationRecordRepository.GetListForSmallDistrictIdAsync(new OwnerCertificationRecordDto { SmallDistrictId = voteEntity.SmallDistrictArray });

                VoteResult result = VoteResult.Overrule;
                if (voteEntity.CalculationMethodValue == CalculationMethod.EndorsedNumber.Value)
                {
                    if ((double)voteQuestionOption1.Votes >= ((double)ownerCertificationRecordList.Count / (double)3) * 2)
                    {
                        result = VoteResult.Adopt;
                    }
                    else
                    {
                        result = VoteResult.Overrule;
                    }
                }
                if (voteEntity.CalculationMethodValue == CalculationMethod.Opposition.Value)
                {
                    if ((double)voteQuestionOption2.Votes <= ((double)ownerCertificationRecordList.Count / (double)3))
                    {
                        result = VoteResult.Adopt;
                    }
                    else
                    {
                        result = VoteResult.Overrule;
                    }
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
                    VoteQuestionId = voteQuestion.Id.ToString(),
                    ShouldParticipateCount = ownerCertificationRecordList.Count,
                    ActualParticipateCount = voteQuestionOption1.Votes + voteQuestionOption2.Votes
                });
                await voteRepository.UpdateForClosedAsync(new VoteDto
                {
                    Id = voteEntity.Id.ToString()
                });

                await SeedResultAsync(voteEntity.Id);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 一个问题多个选项计算投票结果(业委会竞选用)
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static async Task AddVoteResultRecordIndefiniteAsync(Guid guid)
        {
            try
            {
                IVoteRepository voteRepository = new VoteRepository();
                IVoteQuestionRepository voteQuestionRepository = new VoteQuestionRepository();
                IVoteQuestionOptionRepository voteQuestionOptionRepository = new VoteQuestionOptionRepository();
                IVoteResultRecordRepository voteResultRecordRepository = new VoteResultRecordRepository();
                IVoteRecordRepository recordRepository = new VoteRecordRepository();
                IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
                IVoteAssociationVipOwnerRepository voteAssociationVipOwnerRepository = new VoteAssociationVipOwnerRepository();
                IVipOwnerApplicationRecordRepository vipOwnerApplicationRecordRepository = new VipOwnerApplicationRecordRepository();
                IVipOwnerRepository vipOwnerRepository = new VipOwnerRepository();
                IVipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository = new VipOwnerCertificationRecordRepository();
                IUserRepository userRepository = new UserRepository();
                var voteEntity = await voteRepository.GetAsync(guid.ToString());
                var voteQuestionList = await voteQuestionRepository.GetListAsync(new VoteQuestionDto() { VoteId = voteEntity.Id.ToString() });
                var voteQuestion = voteQuestionList[0];
                var ownerCertificationRecordList = await ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto { SmallDistrictId = voteEntity.SmallDistrictArray });
                var voteResultRecordList = await recordRepository.GetListAsync(new VoteRecordDto
                {
                    VoteId = voteEntity.Id.ToString()
                });
                var entity = await voteResultRecordRepository.AddAsync(new VoteResultRecordDto
                {
                    CalculationMethodValue = voteEntity.CalculationMethodValue,
                    CalculationMethodName = voteEntity.CalculationMethodName,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                    VoteId = voteEntity.Id.ToString(),
                    ResultValue = VoteResult.Adopt.Value,
                    ResultName = VoteResult.Adopt.Name,
                    ShouldParticipateCount = ownerCertificationRecordList.Count,
                    ActualParticipateCount = voteResultRecordList.Count
                });
                var voteAssociationVipOwner = await voteAssociationVipOwnerRepository.GetForVoteIdAsync(voteEntity.Id.ToString());
                var vipOwner = await vipOwnerRepository.GetAsync(voteAssociationVipOwner.VipOwnerId);
                var vipOwnerOld = await vipOwnerRepository.GetForSmallDistrictIdAsync(new VipOwnerDto
                {
                    SmallDistrictId = vipOwner.SmallDistrictId.ToString()
                });
                if (vipOwnerOld != null)
                {
                    await vipOwnerRepository.UpdateInvalidAsync(new VipOwnerDto
                    {
                        Id = vipOwnerOld.Id.ToString(),
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = "system"
                    });
                }

                await vipOwnerRepository.UpdateValidAsync(new VipOwnerDto
                {
                    Id = vipOwner.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system"
                });
                var voteQuestionOptionList = (await voteQuestionOptionRepository.GetListAsync(new VoteQuestionOptionDto() { VoteId = voteEntity.Id.ToString(), VoteQuestionId = voteQuestion.Id.ToString() })).OrderByDescending(x => x.Votes).ToList();
                string content = "";
                for (int i = 0; i < voteAssociationVipOwner.ElectionNumber; i++)
                {
                    var vipOwnerApplicationRecord = await vipOwnerApplicationRecordRepository.GetForVoteQuestionOptionIdAsync(new VipOwnerApplicationRecordDto
                    {
                        VoteId = voteQuestionOptionList[i].VoteId,
                        VoteQuestionId = voteQuestionOptionList[i].VoteQuestionId.ToString(),
                        VoteQuestionOptionId = voteQuestionOptionList[i].Id.ToString()
                    });

                    await vipOwnerCertificationRecordRepository.AddAsync(new VipOwnerCertificationRecordDto
                    {
                        OperationTime = DateTimeOffset.Now,
                        OperationUserId = "system",
                        VipOwnerId = voteAssociationVipOwner.VipOwnerId,
                        OwnerCertificationId = vipOwnerApplicationRecord.OwnerCertificationId,
                        UserId = vipOwnerApplicationRecord.UserId,
                        VipOwnerName = vipOwner.Name,
                        VipOwnerStructureId = vipOwnerApplicationRecord.StructureId,
                        VipOwnerStructureName = vipOwnerApplicationRecord.StructureName,
                        VoteId = voteQuestionOptionList[i].VoteId
                    });

                    BackgroundJob.Enqueue(() => SeedVipOwnerCertificationRecordAsync(vipOwnerApplicationRecord.OwnerCertificationId));

                    content = content + vipOwnerApplicationRecord.Name + ":票数为" + voteQuestionOptionList[i].Votes + "任命为" + vipOwnerApplicationRecord.StructureName + "\r\n ";
                }
                await voteRepository.UpdateForClosedAsync(new VoteDto
                {
                    Id = voteEntity.Id.ToString()
                });
                await SeedResultAsync(voteEntity.Id);

            }
            catch (Exception)
            {

            }
        }

        #endregion

        /// <summary>
        /// 业委会查询投票列表(小程序用)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("vote/getAllForVipOwner")]
        public async Task<ApiResult<GetAllForVipOwnerOutput>> GetAllForVipOwner([FromUri]GetAllForVipOwnerInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
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
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetAllForVipOwnerOutput>(APIResultCode.Unknown, new GetAllForVipOwnerOutput { }, APIResultMessage.TokenError);
            }
            var data = await _voteRepository.GetAllForVipOwnerAsync(new VoteDto
            {
                Title = input.Title,
                StreetOfficeId = user.StreetOfficeId,
                CommunityId = user.CommunityId,
                SmallDistrictId = user.SmallDistrictId
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
    }
}
