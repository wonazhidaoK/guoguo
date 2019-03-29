using GuoGuoCommunity.API.Models;
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
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 公告信息管理
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AnnouncementController : ApiController
    {
        private readonly IAnnouncementRepository _announcementRepository;
        private readonly IAnnouncementAnnexRepository _announcementAnnexRepository;
        private readonly IUploadRepository _uploadRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 公告信息管理
        /// </summary>
        /// <param name="announcementRepository"></param>
        /// <param name="announcementAnnexRepository"></param>
        /// <param name="uploadRepository"></param>
        public AnnouncementController(IAnnouncementRepository announcementRepository,
            IAnnouncementAnnexRepository announcementAnnexRepository,
            IUploadRepository uploadRepository)
        {
            _announcementRepository = announcementRepository;
            _announcementAnnexRepository = announcementAnnexRepository;
            _uploadRepository = uploadRepository;
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
                if (string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    throw new NotImplementedException("附件Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("公告标题信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Summary))
                {
                    throw new NotImplementedException("公告摘要信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.SmallDistrict))
                {
                    throw new NotImplementedException("公告小区范围信息为空！");
                }
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
                    SmallDistrictArray = input.SmallDistrict,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString(),
                    CommunityId = user.CommunityId,
                    CommunityName = user.CommunityName,
                    SmallDistrictId = user.SmallDistrictId,
                    SmallDistrictName = user.SmallDistrictName,
                    StreetOfficeId = user.StreetOfficeId,
                    StreetOfficeName = user.StreetOfficeName,
                    OwnerCertificationId = input.OwnerCertificationId
                }, cancelToken);

                await _announcementAnnexRepository.AddAsync(new AnnouncementAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    AnnouncementId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);
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
                if (string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    throw new NotImplementedException("附件Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("公告标题信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.Summary))
                {
                    throw new NotImplementedException("公告摘要信息为空！");
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

                await _announcementAnnexRepository.AddAsync(new AnnouncementAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    AnnouncementId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

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
                if (string.IsNullOrWhiteSpace(input.AnnexId))
                {
                    throw new NotImplementedException("附件Id信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("公告标题信息为空！");
                }

                if (string.IsNullOrWhiteSpace(input.Summary))
                {
                    throw new NotImplementedException("公告摘要信息为空！");
                }
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

                await _announcementAnnexRepository.AddAsync(new AnnouncementAnnexDto
                {
                    AnnexContent = input.AnnexId,
                    AnnouncementId = entity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

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
                    SmallDistrictArray = user.SmallDistrictId,
                    DepartmentValue = Department.YeZhuWeiYuanHui.Value
                }, cancelToken);


                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = data.Select(x => new GetVipOwnerAnnouncementOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Content = x.Content,
                        ReleaseTime = x.CreateOperationTime.Value,
                        Summary = x.Summary,
                        Url = _announcementAnnexRepository.GetUrl(x.Id.ToString())
                    }).Skip(startRow).Take(input.PageSize).ToList(),
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
                    SmallDistrictArray = user.SmallDistrictId,
                    DepartmentValue = Department.WuYe.Value
                }, cancelToken);

                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = data.Select(x => new GetVipOwnerAnnouncementOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Content = x.Content,
                        ReleaseTime = x.CreateOperationTime.Value,
                        Summary = x.Summary,
                        Url = _announcementAnnexRepository.GetUrl(x.Id.ToString())
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
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
                    SmallDistrictArray = user.SmallDistrictId,
                    DepartmentValue = Department.JieDaoBan.Value
                }, cancelToken);

                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = data.Select(x => new GetVipOwnerAnnouncementOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Content = x.Content,
                        ReleaseTime = x.CreateOperationTime.Value,
                        Summary = x.Summary,
                        Url = _announcementAnnexRepository.GetUrl(x.Id.ToString())
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
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
        public async Task<ApiResult<GetAllAnnouncementOutput>> GetListStreetOfficeAnnouncement([FromUri]GetAllAnnouncementInput input, CancellationToken cancelToken)
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

                var data = await _announcementRepository.GetListForStreetOfficeAsync(new AnnouncementDto
                {
                    Title = input.Title,
                    StreetOfficeId = user.StreetOfficeId,
                    DepartmentValue = Department.JieDaoBan.Value
                }, cancelToken);

                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = data.Select(x => new GetVipOwnerAnnouncementOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Content = x.Content,
                        ReleaseTime = x.CreateOperationTime.Value,
                        Summary = x.Summary,
                        Url = _announcementAnnexRepository.GetUrl(x.Id.ToString())
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, e.Message);
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
        public async Task<ApiResult<GetAllAnnouncementOutput>> GetListPropertyAnnouncement([FromUri]GetAllAnnouncementInput input, CancellationToken cancelToken)
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

                var data = await _announcementRepository.GetListPropertyAsync(new AnnouncementDto
                {
                    Title = input.Title,
                    SmallDistrictId = user.SmallDistrictId,
                    DepartmentValue = Department.WuYe.Value
                }, cancelToken);

                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success, new GetAllAnnouncementOutput
                {
                    List = data.Select(x => new GetVipOwnerAnnouncementOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Content = x.Content,
                        ReleaseTime = x.CreateOperationTime.Value,
                        Summary = x.Summary,
                        Url = _announcementAnnexRepository.GetUrl(x.Id.ToString())
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllAnnouncementOutput>(APIResultCode.Success_NoB, new GetAllAnnouncementOutput { }, e.Message);
            }
        }

        #endregion
    }
}
