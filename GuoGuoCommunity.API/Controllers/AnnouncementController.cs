using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using GuoGuoCommunity.Domain.Service;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 公告信息管理
    /// </summary>
    public class AnnouncementController : BaseController
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementAnnexRepository _announcementAnnexRepository;
        private readonly IUploadRepository _uploadRepository;
        private readonly ISmallDistrictRepository _smallDistrictRepository;
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IUserRepository _userRepository;
        private readonly TokenManager _tokenManager;

        /// <summary>
        /// 公告信息管理
        /// </summary>
        /// <param name="announcementRepository"></param>
        /// <param name="announcementAnnexRepository"></param>
        /// <param name="uploadRepository"></param>
        /// <param name="ownerCertificationRecordRepository"></param>
        /// <param name="smallDistrictRepository"></param>
        /// <param name="userRepository"></param>
        public AnnouncementController(IAnnouncementRepository announcementRepository,
            IAnnouncementAnnexRepository announcementAnnexRepository,
            IUploadRepository uploadRepository,
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            ISmallDistrictRepository smallDistrictRepository,
            IUserRepository userRepository)
        {
            _announcementRepository = announcementRepository;
            _announcementAnnexRepository = announcementAnnexRepository;
            _uploadRepository = uploadRepository;
            _smallDistrictRepository = smallDistrictRepository;
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _userRepository = userRepository;
            _tokenManager = new TokenManager();
        }

        /// <summary>
        /// 业委会添加公告信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("announcement/addVipOwnerAnnouncement")]
        public async Task<ApiResult<AddVipOwnerAnnouncementOutput>> AddVipOwnerAnnouncement([FromBody]AddVipOwnerAnnouncementInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddVipOwnerAnnouncementOutput>(APIResultCode.Unknown, new AddVipOwnerAnnouncementOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Content))
            {
                throw new NotImplementedException("公告内容信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Title))
            {
                throw new NotImplementedException("公告标题信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
            {
                throw new NotImplementedException("业主认证Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddVipOwnerAnnouncementOutput>(APIResultCode.Unknown, new AddVipOwnerAnnouncementOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _announcementRepository.AddVipOwnerAsync(new AnnouncementDto
            {
                Content = input.Content,
                Summary = input.Summary,
                Title = input.Title,
                DepartmentValue = Department.YeZhuWeiYuanHui.Value,
                DepartmentName = Department.YeZhuWeiYuanHui.Name,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                OwnerCertificationId = input.OwnerCertificationId
            }, cancelToken);

            if (!string.IsNullOrWhiteSpace(input.AnnexId))
            {
                await _announcementAnnexRepository.AddAsync(new AnnouncementAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    AnnouncementId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
            }

            var url = (await _announcementAnnexRepository.GetAsync(entity.Id.ToString()))?.AnnexContent;
            var OperationName = (await _ownerCertificationRecordRepository.GetIncludeAsync(entity.OwnerCertificationId, cancelToken))?.Owner.Name;
            await AnnouncementPushRemind(new AnnouncementPushModel
            {
                Content = entity.Content,
                Id = entity.Id.ToString(),
                ReleaseTime = entity.CreateOperationTime.Value.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"),
                Summary = entity.Summary,
                Title = entity.Title,
                Url = url,
                CreateUserName = OperationName
            }, entity.SmallDistrictArray);
            return new ApiResult<AddVipOwnerAnnouncementOutput>(APIResultCode.Success, new AddVipOwnerAnnouncementOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 物业添加公告信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("announcement/addPropertyAnnouncement")]
        public async Task<ApiResult<AddPropertyAnnouncementOutput>> AddPropertyAnnouncement([FromBody]AddPropertyAnnouncementInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddPropertyAnnouncementOutput>(APIResultCode.Unknown, new AddPropertyAnnouncementOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Content))
            {
                throw new NotImplementedException("公告内容信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Title))
            {
                throw new NotImplementedException("公告标题信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddPropertyAnnouncementOutput>(APIResultCode.Unknown, new AddPropertyAnnouncementOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _announcementRepository.AddAsync(new AnnouncementDto
            {
                Content = input.Content,
                Summary = input.Summary,
                Title = input.Title,
                DepartmentValue = Department.WuYe.Value,
                DepartmentName = Department.WuYe.Name,
                SmallDistrictArray = user.SmallDistrictId,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                CommunityId = user.CommunityId,
                CommunityName = user.CommunityName,
                SmallDistrictId = user.SmallDistrictId,
                SmallDistrictName = user.SmallDistrictName,
                StreetOfficeId = user.StreetOfficeId,
                StreetOfficeName = user.StreetOfficeName
            }, cancelToken);

            if (!string.IsNullOrWhiteSpace(input.AnnexId))
            {
                await _announcementAnnexRepository.AddAsync(new AnnouncementAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    AnnouncementId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
            }

            var url = (await _announcementAnnexRepository.GetAsync(entity.Id.ToString()))?.AnnexContent;
            var userEntity = await _userRepository.GetForIdAsync(entity.CreateOperationUserId);
            await AnnouncementPushRemind(new AnnouncementPushModel
            {
                Content = entity.Content,
                Id = entity.Id.ToString(),
                ReleaseTime = entity.CreateOperationTime.Value.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"),
                Summary = entity.Summary,
                Title = entity.Title,
                Url = url,
                CreateUserName = userEntity?.Name
            }, entity.SmallDistrictArray);
            return new ApiResult<AddPropertyAnnouncementOutput>(APIResultCode.Success, new AddPropertyAnnouncementOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 街道办添加公告信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("announcement/addStreetOfficeAnnouncement")]
        public async Task<ApiResult<AddStreetOfficeAnnouncementOutput>> AddStreetOfficeAnnouncement([FromBody]AddStreetOfficeAnnouncementInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddStreetOfficeAnnouncementOutput>(APIResultCode.Unknown, new AddStreetOfficeAnnouncementOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.Content))
            {
                throw new NotImplementedException("公告内容信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.Title))
            {
                throw new NotImplementedException("公告标题信息为空！");
            }
            if (input.SmallDistricts.Count < 1)
            {
                throw new NotImplementedException("公告小区信息为空！");
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddStreetOfficeAnnouncementOutput>(APIResultCode.Unknown, new AddStreetOfficeAnnouncementOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _announcementRepository.AddAsync(new AnnouncementDto
            {
                Content = input.Content,
                Summary = input.Summary,
                Title = input.Title,
                DepartmentValue = Department.JieDaoBan.Value,
                DepartmentName = Department.JieDaoBan.Name,
                SmallDistrictArray = string.Join(",", input.SmallDistricts.ToArray()),
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
                CommunityId = user.CommunityId,
                CommunityName = user.CommunityName,
                SmallDistrictId = user.SmallDistrictId,
                SmallDistrictName = user.SmallDistrictName,
                StreetOfficeId = user.StreetOfficeId,
                StreetOfficeName = user.StreetOfficeName
            }, cancelToken);

            if (!string.IsNullOrWhiteSpace(input.AnnexId))
            {
                await _announcementAnnexRepository.AddAsync(new AnnouncementAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    AnnouncementId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
            }

            var url = (await _announcementAnnexRepository.GetAsync(entity.Id.ToString()))?.AnnexContent;
            var userEntity = await _userRepository.GetForIdAsync(entity.CreateOperationUserId);

            foreach (var item in input.SmallDistricts)
            {
                await AnnouncementPushRemind(new AnnouncementPushModel
                {
                    Content = entity.Content,
                    Id = entity.Id.ToString(),
                    ReleaseTime = entity.CreateOperationTime.Value.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss"),
                    Summary = entity.Summary,
                    Title = entity.Title,
                    Url = url,
                    CreateUserName = userEntity?.Name
                }, item);
            }

            return new ApiResult<AddStreetOfficeAnnouncementOutput>(APIResultCode.Success, new AddStreetOfficeAnnouncementOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 删除公告信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("announcement/delete")]
        public async Task<ApiResult> Delete([FromUri]string id, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }

            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenError);
            }
            await _announcementRepository.DeleteAsync(new AnnouncementDto
            {
                Id = id,
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString()
            }, cancelToken);
            return new ApiResult();
        }

        /// <summary>
        /// 小程序用业委会公告列表(查询查看范围为当前登陆人小区业委会公告列表)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("announcement/getAllVipOwnerAnnouncement")]
        public async Task<ApiResult<GetAllAnnouncementOutput>> GetAllVipOwnerAnnouncement([FromUri]GetAllAnnouncementInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, "分页参数必传");
            }
            //if (Authorization == null)
            //{
            //    return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenNull);
            //}
            //if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
            //{
            //    throw new NotImplementedException("业主认证Id信息为空！");
            //}
            //var user = _tokenManager.GetUser(Authorization);
            //if (user == null)
            //{
            //    return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenError);
            //}

            if (input.PageIndex < 1)
            {
                input.PageIndex = 1;
            }
            if (input.PageSize < 1)
            {
                input.PageSize = 10;
            }

            int startRow = (input.PageIndex - 1) * input.PageSize;
            var data = await _announcementRepository.GetAllForVipOwnerAsync(new AnnouncementDto
            {
                Title = input.Title,
                DepartmentValue = Department.YeZhuWeiYuanHui.Value,
                OwnerCertificationId = input.OwnerCertificationId
            }, cancelToken);

            var listCount = data.Count();
            var dataList = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize).ToList();
            var userList = await _ownerCertificationRecordRepository.GetListForIdArrayIncludeAsync(dataList.Select(x => x.OwnerCertificationId).ToList());
            var urlList = await _announcementAnnexRepository.GetForAnnouncementIdsAsync(dataList.Select(x => x.Id.ToString()).ToList(), cancelToken);
            List<GetVipOwnerAnnouncementOutput> list = new List<GetVipOwnerAnnouncementOutput>();
            foreach (var item in dataList)
            {
                list.Add(new GetVipOwnerAnnouncementOutput
                {
                    Id = item.Id.ToString(),
                    Title = item.Title,
                    Content = item.Content,
                    ReleaseTime = item.CreateOperationTime.Value,
                    Summary = item.Summary,
                    Url = urlList.Where(x => x.AnnouncementId == item.Id.ToString()).Any() ? urlList.Where(x => x.AnnouncementId == item.Id.ToString()).FirstOrDefault().AnnexContent : "",
                    CreateUserName = userList.Where(x => x.Id.ToString() == item.OwnerCertificationId).Any() ? userList.Where(x => x.Id.ToString() == item.OwnerCertificationId).FirstOrDefault().Owner.Name : ""
                });
            }

            return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
            {
                List = list,
                TotalCount = data.Count()
            });
        }

        /// <summary>
        /// 小程序用物业公告列表(查询查看范围为当前登陆人小区物业公告列表)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("announcement/getAllPropertyAnnouncement")]
        public async Task<ApiResult<GetAllAnnouncementOutput>> GetAllPropertyAnnouncement([FromUri]GetAllAnnouncementInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, "分页参数必传");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenNull);
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
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenError);
            }

            var data = await _announcementRepository.GetAllAsync(new AnnouncementDto
            {
                Title = input.Title,
                OwnerCertificationId = input.OwnerCertificationId,
                DepartmentValue = Department.WuYe.Value
            }, cancelToken);

            var listCount = data.Count();
            var dataList = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize).ToList();
            var userList = await _userRepository.GetByIdsAsync(dataList.Select(x => x.CreateOperationUserId).ToList());
            var urlList = await _announcementAnnexRepository.GetForAnnouncementIdsAsync(dataList.Select(x => x.Id.ToString()).ToList(), cancelToken);

            List<GetVipOwnerAnnouncementOutput> list = new List<GetVipOwnerAnnouncementOutput>();
            foreach (var item in dataList)
            {
                list.Add(new GetVipOwnerAnnouncementOutput
                {
                    Id = item.Id.ToString(),
                    Title = item.Title,
                    Content = item.Content,
                    ReleaseTime = item.CreateOperationTime.Value,
                    Summary = item.Summary,
                    Url = urlList.Where(x => x.AnnouncementId == item.Id.ToString()).FirstOrDefault()?.AnnexContent,
                    CreateUserName = userList.Where(x => x.Id.ToString() == item.CreateOperationUserId)?.FirstOrDefault().Name
                });
            }

            return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
            {
                List = list,
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 小程序用街道办公告列表(查询查看范围为当前登陆人小区街道办公告列表)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("announcement/getAllStreetOfficeAnnouncement")]
        public async Task<ApiResult<GetAllAnnouncementOutput>> GetAllStreetOfficeAnnouncement([FromUri]GetAllAnnouncementInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, "分页参数必传");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenNull);
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
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenError);
            }

            var data = await _announcementRepository.GetAllAsync(new AnnouncementDto
            {
                Title = input.Title,
                OwnerCertificationId = input.OwnerCertificationId,
                DepartmentValue = Department.JieDaoBan.Value
            }, cancelToken);

            var listCount = data.Count();
            var dataList = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize).ToList();
            var userList = await _userRepository.GetByIdsAsync(dataList.Select(x => x.CreateOperationUserId).ToList());
            var urlList = await _announcementAnnexRepository.GetForAnnouncementIdsAsync(dataList.Select(x => x.Id.ToString()).ToList(), cancelToken);

            List<GetVipOwnerAnnouncementOutput> list = new List<GetVipOwnerAnnouncementOutput>();
            foreach (var item in dataList)
            {
                list.Add(new GetVipOwnerAnnouncementOutput
                {
                    Id = item.Id.ToString(),
                    Title = item.Title,
                    Content = item.Content,
                    ReleaseTime = item.CreateOperationTime.Value,
                    Summary = item.Summary,
                    Url = urlList.Where(x => x.AnnouncementId == item.Id.ToString()).FirstOrDefault()?.AnnexContent,
                    CreateUserName = userList.Where(x => x.Id.ToString() == item.CreateOperationUserId)?.FirstOrDefault().Name
                });
            }
            return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
            {
                List = list,
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 街道办公告列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("announcement/getListStreetOfficeAnnouncement")]
        public async Task<ApiResult<GetListStreetOfficeAnnouncementOutput>> GetListStreetOfficeAnnouncement([FromUri]GetListStreetOfficeAnnouncementInput input, CancellationToken cancelToken)
        {
            if (input == null)
            {
                return new ApiResult<GetListStreetOfficeAnnouncementOutput>(APIResultCode.Success_NoB, new GetListStreetOfficeAnnouncementOutput { }, "分页参数必传");
            }
            if (Authorization == null)
            {
                return new ApiResult<GetListStreetOfficeAnnouncementOutput>(APIResultCode.Unknown, new GetListStreetOfficeAnnouncementOutput { }, APIResultMessage.TokenNull);
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
                return new ApiResult<GetListStreetOfficeAnnouncementOutput>(APIResultCode.Unknown, new GetListStreetOfficeAnnouncementOutput { }, APIResultMessage.TokenError);
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

            var data = await _announcementRepository.GetListForStreetOfficeAsync(new AnnouncementDto
            {
                StartTime = startTime,
                EndTime = endTime,
                Title = input.Title,
                StreetOfficeId = user.StreetOfficeId,
                DepartmentValue = Department.JieDaoBan.Value
            }, cancelToken);

            var listCount = data.Count();
            var dataList = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize).ToList();
            var userList = await _userRepository.GetByIdsAsync(dataList.Select(x => x.CreateOperationUserId).ToList());
            var urlList = await _announcementAnnexRepository.GetForAnnouncementIdsAsync(dataList.Select(x => x.Id.ToString()).ToList(), cancelToken);

            List<GetListStreetOfficeAnnouncementModelOutput> list = new List<GetListStreetOfficeAnnouncementModelOutput>();
            foreach (var item in dataList)
            {
                List<string> smallDistrictIdList = new List<string>(item.SmallDistrictArray.Split(','));
                List<SmallDistrictModel> smallDistrictList = new List<SmallDistrictModel>();
                foreach (var smallDistrictId in smallDistrictIdList)
                {
                    var smallDistrictEntity = await _smallDistrictRepository.GetAsync(smallDistrictId, cancelToken);
                    smallDistrictList.Add(new SmallDistrictModel
                    {
                        Id = smallDistrictId,
                        Name = smallDistrictEntity.Name
                    });
                };
                var userEntity = userList.Where(x => x.Id.ToString() == item.CreateOperationUserId).FirstOrDefault();
                list.Add(new GetListStreetOfficeAnnouncementModelOutput
                {
                    Id = item.Id.ToString(),
                    Title = item.Title,
                    Content = item.Content,
                    ReleaseTime = item.CreateOperationTime.Value,
                    Summary = item.Summary,
                    Url = urlList.Where(x => x.AnnouncementId == item.Id.ToString()).FirstOrDefault()?.AnnexContent,
                    SmallDistrict = smallDistrictList,
                    CreateUserName = userEntity?.Name
                });
            }
            return new ApiResult<GetListStreetOfficeAnnouncementOutput>(APIResultCode.Success, new GetListStreetOfficeAnnouncementOutput
            {
                List = list,
                TotalCount = listCount
            });
        }

        ///<summary>
        /// 物业公告列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("announcement/getListPropertyAnnouncement")]
        public async Task<ApiResult<GetAllAnnouncementOutput>> GetListPropertyAnnouncement([FromUri]GetListPropertyAnnouncementInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenNull);
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
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenError);
            }

            var data = await _announcementRepository.GetListPropertyAsync(new AnnouncementDto
            {
                StartTime = startTime,
                EndTime = endTime,
                Title = input.Title,
                SmallDistrictId = user.SmallDistrictId,
                DepartmentValue = Department.WuYe.Value
            }, cancelToken);
            var listCount = data.Count();

            var dataList = data.OrderByDescending(a => a.CreateOperationTime).Skip(startRow).Take(input.PageSize).ToList();
            var userList = await _userRepository.GetByIdsAsync(dataList.Select(x => x.CreateOperationUserId).ToList());
            var urlList = await _announcementAnnexRepository.GetForAnnouncementIdsAsync(dataList.Select(x => x.Id.ToString()).ToList(), cancelToken);
            List<GetVipOwnerAnnouncementOutput> list = new List<GetVipOwnerAnnouncementOutput>();
            foreach (var item in dataList)
            {
                list.Add(
                    new GetVipOwnerAnnouncementOutput
                    {
                        Id = item.Id.ToString(),
                        Title = item.Title,
                        Content = item.Content,
                        ReleaseTime = item.CreateOperationTime.Value,
                        Summary = item.Summary,
                        Url = urlList.Where(x => x.AnnouncementId == item.Id.ToString()).FirstOrDefault()?.AnnexContent,
                        CreateUserName = userList.Where(x => x.Id.ToString() == item.CreateOperationUserId).FirstOrDefault()?.Name
                    });
            }
            return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
            {
                List = list,
                TotalCount = listCount
            });
        }

        /// <summary>
        /// 发送推送消息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="smallDistrict"></param>
        public static async Task AnnouncementPushRemind(AnnouncementPushModel model, string smallDistrict)
        {
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository = new OwnerCertificationRecordRepository();
            IUserRepository userRepository = new UserRepository();
            IWeiXinUserRepository weiXinUserRepository = new WeiXinUserRepository();

            var userIdList = (await ownerCertificationRecordRepository.GetListForSmallDistrictIdAsync(new OwnerCertificationRecordDto
            {
                SmallDistrictId = smallDistrict
            })).Select(x => x.UserId).Distinct().ToList();
            var userList = await userRepository.GetByIdsAsync(userIdList.ToList());
            var weiXinUserList = await weiXinUserRepository.GetForUnionIdsAsync(userList.Select(x => x.UnionId).ToList());

            foreach (var item in userIdList)
            {
                try
                {
                    var user = userList.Where(x => x.Id.ToString() == item).FirstOrDefault();
                    var weiXinUser = weiXinUserList.Where(x => x.Unionid == user?.UnionId).FirstOrDefault();
                    var templateData = new
                    {
                        first = new TemplateDataItem("公告通知"),
                        keyword1 = new TemplateDataItem(model.Title),
                        keyword2 = new TemplateDataItem("发送成功"),
                        keyword3 = new TemplateDataItem(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss\r\n")),
                        remark = new TemplateDataItem(">>点击查看详情<<", "#FF0000")
                    };
                    var miniProgram = new TempleteModel_MiniProgram()
                    {
                        appid = GuoGuoCommunity_WxOpenAppId,
                        pagepath = "pages/noticeDetail/noticeDetail?con=" + JsonConvert.SerializeObject(model)
                    };
                    TemplateApi.SendTemplateMessage(AppId, weiXinUser?.OpenId, AnnouncementTemplateId, null, templateData, miniProgram);
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
