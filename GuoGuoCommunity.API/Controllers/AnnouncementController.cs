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
using System.Web;
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
        private TokenManager _tokenManager;

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

        /*
         * TODO
         * 1.业主委员会添加公告
         * 2.物业添加公告
         * 3.街道办添加公告
         * 4.删除公告
         * 5.街道办公告列表，物业公告列表,业委会公告列表
         * （街道办查询街道办下所有公告，物业查询当前物业所有公告)
         * 6.街道办公告列表，物业公告列表,业委会公告列表(小程序？)
         * 7.公告详情
        */

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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddVipOwnerAnnouncementOutput>(APIResultCode.Unknown, new AddVipOwnerAnnouncementOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Content))
                {
                    throw new NotImplementedException("公告内容信息为空！");
                }
                //if (string.IsNullOrWhiteSpace(input.AnnexId))
                //{
                //    throw new NotImplementedException("附件Id信息为空！");
                //}
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("公告标题信息为空！");
                }
                //if (string.IsNullOrWhiteSpace(input.Summary))
                //{
                //    throw new NotImplementedException("公告摘要信息为空！");
                //}
                //if (string.IsNullOrWhiteSpace(input.SmallDistrict))
                //{
                //    throw new NotImplementedException("公告小区范围信息为空！");
                //}
                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息为空！");
                }
                var user = _tokenManager.GetUser(token);
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
                    //SmallDistrictArray = input.SmallDistrict,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    // CommunityId = user.CommunityId,
                    // CommunityName = user.CommunityName,
                    // SmallDistrictId = user.SmallDistrictId,
                    // SmallDistrictName = user.SmallDistrictName,
                    // StreetOfficeId = user.StreetOfficeId,
                    // StreetOfficeName = user.StreetOfficeName,
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

                /*
                 * 微信消息推送
                 */
                var url = (await _announcementAnnexRepository.GetAsync(entity.Id.ToString()))?.AnnexContent;
                var OperationName = (await _ownerCertificationRecordRepository.GetAsync(entity.OwnerCertificationId, cancelToken))?.OwnerName;
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
            catch (Exception e)
            {
                return new ApiResult<AddVipOwnerAnnouncementOutput>(APIResultCode.Success_NoB, new AddVipOwnerAnnouncementOutput { }, e.Message);
            }
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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddPropertyAnnouncementOutput>(APIResultCode.Unknown, new AddPropertyAnnouncementOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Content))
                {
                    throw new NotImplementedException("公告内容信息为空！");
                }
                //if (string.IsNullOrWhiteSpace(input.AnnexId))
                //{
                //    throw new NotImplementedException("附件Id信息为空！");
                //}
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("公告标题信息为空！");
                }

                var user = _tokenManager.GetUser(token);
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

                /*
                 * 微信消息推送
                 */
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
            catch (Exception e)
            {
                return new ApiResult<AddPropertyAnnouncementOutput>(APIResultCode.Success_NoB, new AddPropertyAnnouncementOutput { }, e.Message);
            }
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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddStreetOfficeAnnouncementOutput>(APIResultCode.Unknown, new AddStreetOfficeAnnouncementOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Content))
                {
                    throw new NotImplementedException("公告内容信息为空！");
                }
                //if (string.IsNullOrWhiteSpace(input.AnnexId))
                //{
                //    throw new NotImplementedException("附件Id信息为空！");
                //}
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("公告标题信息为空！");
                }

                //if (string.IsNullOrWhiteSpace(input.Summary))
                //{
                //    throw new NotImplementedException("公告摘要信息为空！");
                //}
                if (input.SmallDistricts.Count < 1)
                {
                    throw new NotImplementedException("公告小区信息为空！");
                }
                var user = _tokenManager.GetUser(token);
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


                /*
                 * 微信消息推送
                 */
                foreach (var item in input.SmallDistricts)
                {
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
                    }, item);
                }
                return new ApiResult<AddStreetOfficeAnnouncementOutput>(APIResultCode.Success, new AddStreetOfficeAnnouncementOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddStreetOfficeAnnouncementOutput>(APIResultCode.Success_NoB, new AddStreetOfficeAnnouncementOutput { }, e.Message);
            }
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
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult(APIResultCode.Unknown, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new NotImplementedException("楼宇Id信息为空！");
                }

                var user = _tokenManager.GetUser(token);
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
            catch (Exception e)
            {
                return new ApiResult(APIResultCode.Success_NoB, e.Message);
            }
        }

        #region 小程序 当前认证身份的公告

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
            try
            {
                if (string.IsNullOrWhiteSpace(input.OwnerCertificationId))
                {
                    throw new NotImplementedException("业主认证Id信息为空！");
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
                var token = HttpContext.Current.Request.Headers["Authorization"];

                if (token == null)
                {
                    return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);

                if (user == null)
                {
                    return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenError);
                }

                var data = await _announcementRepository.GetAllForVipOwnerAsync(new AnnouncementDto
                {
                    Title = input.Title,
                    // SmallDistrictArray = user.SmallDistrictId,
                    DepartmentValue = Department.YeZhuWeiYuanHui.Value,
                    OwnerCertificationId = input.OwnerCertificationId
                }, cancelToken);
                List<GetVipOwnerAnnouncementOutput> list = new List<GetVipOwnerAnnouncementOutput>();
                foreach (var item in data)
                {
                    var url = (await _announcementAnnexRepository.GetAsync(item.Id.ToString()))?.AnnexContent;
                    var OperationName = (await _ownerCertificationRecordRepository.GetAsync(item.OwnerCertificationId, cancelToken))?.OwnerName;
                    //OwnerCertificationId
                    //var userEntity = await _userRepository.GetForIdAsync(item.CreateOperationUserId);
                    list.Add(new GetVipOwnerAnnouncementOutput
                    {
                        Id = item.Id.ToString(),
                        Title = item.Title,
                        Content = item.Content,
                        ReleaseTime = item.CreateOperationTime.Value,
                        Summary = item.Summary,
                        Url = url,
                        CreateUserName = OperationName
                    });
                }

                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = list.OrderByDescending(a => a.ReleaseTime).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, e.Message);
            }
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
            try
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
                var token = HttpContext.Current.Request.Headers["Authorization"];

                if (token == null)
                {
                    return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);

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
                List<GetVipOwnerAnnouncementOutput> list = new List<GetVipOwnerAnnouncementOutput>();
                foreach (var item in dataList)
                {
                    var url = (await _announcementAnnexRepository.GetAsync(item.Id.ToString()))?.AnnexContent;
                    var userEntity = await _userRepository.GetForIdAsync(item.CreateOperationUserId);
                    list.Add(new GetVipOwnerAnnouncementOutput
                    {
                        Id = item.Id.ToString(),
                        Title = item.Title,
                        Content = item.Content,
                        ReleaseTime = item.CreateOperationTime.Value,
                        Summary = item.Summary,
                        Url = url,
                        CreateUserName = userEntity.Name
                    });
                }

                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = list,
                    TotalCount = listCount
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, e.Message);
            }
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
            try
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
                var token = HttpContext.Current.Request.Headers["Authorization"];

                if (token == null)
                {
                    return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);

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
                List<GetVipOwnerAnnouncementOutput> list = new List<GetVipOwnerAnnouncementOutput>();
                foreach (var item in dataList)
                {
                    var url = (await _announcementAnnexRepository.GetAsync(item.Id.ToString()))?.AnnexContent;
                    var userEntity = await _userRepository.GetForIdAsync(item.CreateOperationUserId);
                    list.Add(new GetVipOwnerAnnouncementOutput
                    {
                        Id = item.Id.ToString(),
                        Title = item.Title,
                        Content = item.Content,
                        ReleaseTime = item.CreateOperationTime.Value,
                        Summary = item.Summary,
                        Url = url,
                        CreateUserName = userEntity.Name
                    });
                }
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = list,
                    TotalCount = listCount
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, e.Message);
            }
        }

        #endregion

        #region 街道办后台

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
            try
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
                var token = HttpContext.Current.Request.Headers["Authorization"];

                if (token == null)
                {
                    return new ApiResult<GetListStreetOfficeAnnouncementOutput>(APIResultCode.Unknown, new GetListStreetOfficeAnnouncementOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);

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
                    var userEntity = await _userRepository.GetForIdAsync(item.CreateOperationUserId);
                    list.Add(new GetListStreetOfficeAnnouncementModelOutput
                    {
                        Id = item.Id.ToString(),
                        Title = item.Title,
                        Content = item.Content,
                        ReleaseTime = item.CreateOperationTime.Value,
                        Summary = item.Summary,
                        Url = (await _announcementAnnexRepository.GetAsync(item.Id.ToString(), cancelToken))?.AnnexContent,
                        SmallDistrict = smallDistrictList,
                        CreateUserName = userEntity.Name
                    });
                }
                return new ApiResult<GetListStreetOfficeAnnouncementOutput>(APIResultCode.Success, new GetListStreetOfficeAnnouncementOutput
                {
                    List = list,
                    TotalCount = listCount
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetListStreetOfficeAnnouncementOutput>(APIResultCode.Success_NoB, new GetListStreetOfficeAnnouncementOutput { }, e.Message);
            }
        }

        #endregion

        #region 物业后台

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
            try
            {
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
                var token = HttpContext.Current.Request.Headers["Authorization"];

                if (token == null)
                {
                    return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Unknown, new GetAllAnnouncementOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);

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
                List<GetVipOwnerAnnouncementOutput> list = new List<GetVipOwnerAnnouncementOutput>();


                foreach (var item in dataList)
                {
                    var userEntity = await _userRepository.GetForIdAsync(item.CreateOperationUserId);
                    list.Add(
                       new GetVipOwnerAnnouncementOutput
                       {
                           Id = item.Id.ToString(),
                           Title = item.Title,
                           Content = item.Content,
                           ReleaseTime = item.CreateOperationTime.Value,
                           Summary = item.Summary,
                           Url = (await _announcementAnnexRepository.GetAsync(item.Id.ToString()))?.AnnexContent,
                           CreateUserName = userEntity.Name
                       }
                        );
                }
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = list,
                    TotalCount = listCount
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, e.Message);
            }
        }

        #endregion

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

            var userList = (await ownerCertificationRecordRepository.GetListForSmallDistrictIdAsync(new OwnerCertificationRecordDto
            {
                SmallDistrictId = smallDistrict
            })).Select(x => x.UserId).Distinct().ToList();

            foreach (var item in userList)
            {
                try
                {
                    var user = await userRepository.GetForIdAsync(item);
                    var weiXinUser = await weiXinUserRepository.GetAsync(user.UnionId);
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
                        appid = GuoGuoCommunity_WxOpenAppId,//ZhiShiHuLian_WxOpenAppId,
                        pagepath = "pages/noticeDetail/noticeDetail?con=" + JsonConvert.SerializeObject(model)
                    };

                    TemplateApi.SendTemplateMessage(AppId, weiXinUser.OpenId, AnnouncementTemplateId, null, templateData, miniProgram);
                }
                catch (Exception e)
                {
                    // throw new NotImplementedException(e.Message + weiXinUser.OpenId);
                }
            }
        }

    }
}
