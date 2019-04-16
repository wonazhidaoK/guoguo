using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 公共接口
    /// </summary>
    public class CommonController : ApiController
    {
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly ISmallDistrictRepository _smallDistrictRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IIndustryRepository _industryRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerCertificationRecordRepository"></param>
        /// <param name="smallDistrictRepository"></param>
        /// <param name="ownerRepository"></param>
        /// <param name="industryRepository"></param>
        public CommonController(IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            ISmallDistrictRepository smallDistrictRepository,
            IOwnerRepository ownerRepository,
            IIndustryRepository industryRepository)
        {
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _smallDistrictRepository = smallDistrictRepository;
            _ownerRepository = ownerRepository;
            _industryRepository = industryRepository;
            _tokenManager = new TokenManager();
        }

        // 当前用户有效业主认证的小区记录

        // 当前用户有效认证业主认证记录

        //TODO 当前用户有效高级认证记录

        /// <summary>
        /// 获取用户小区集合
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrict/getListForUserId")]
        public async Task<ApiResult<List<GetListForUserIdOutput>>> GetListForUserId(CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<List<GetListForUserIdOutput>>(APIResultCode.Unknown, new List<GetListForUserIdOutput> { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<List<GetListForUserIdOutput>>(APIResultCode.Unknown, new List<GetListForUserIdOutput> { }, APIResultMessage.TokenError);
                }

                var data = (await _ownerCertificationRecordRepository.GetListAsync(new OwnerCertificationRecordDto
                {
                    UserId = user.Id.ToString()
                }, cancelToken)).Select(x => x.SmallDistrictId).Distinct();

                List<GetListForUserIdOutput> list = new List<GetListForUserIdOutput>();
                foreach (var item in data)
                {
                    var entity = await _smallDistrictRepository.GetAsync(item, cancelToken);
                    list.Add(new GetListForUserIdOutput
                    {
                        Id = entity.Id.ToString(),
                        Name = entity.Name,
                        //City = entity.City,
                        //CommunityId = entity.CommunityId,
                        //CommunityName = entity.CommunityName,
                        //Region = entity.Region,
                        //State = entity.State,
                        //StreetOfficeId = entity.StreetOfficeId,
                        //StreetOfficeName = entity.StreetOfficeName
                    });
                }
                return new ApiResult<List<GetListForUserIdOutput>>(APIResultCode.Success, list);
            }
            catch (Exception e)
            {
                return new ApiResult<List<GetListForUserIdOutput>>(APIResultCode.Success_NoB, new List<GetListForUserIdOutput> { }, e.Message);
            }
        }

        /// <summary>
        /// 根据小区id获取用户认证记录
        /// </summary>
        /// <param name="SmallDistrictId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ownerCertificationRecord/getAllForSmallDistrictId")]
        public async Task<ApiResult<GetListOwnerCertificationRecordOutput>> GetAllForSmallDistrictId([FromUri]string SmallDistrictId, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
                }
                var data = await _ownerCertificationRecordRepository.GetAllForSmallDistrictIdAsync(new OwnerCertificationRecordDto
                {
                    UserId = user.Id.ToString(),
                    SmallDistrictId = SmallDistrictId
                }, cancelToken);
                List<GetOwnerCertificationRecordOutput> list = new List<GetOwnerCertificationRecordOutput>();

                foreach (var item in data)
                {
                    var owner = await _ownerRepository.GetAsync(item.OwnerId, cancelToken);
                    var industry = await _industryRepository.GetAsync(item.IndustryId, cancelToken);
                    list.Add(new GetOwnerCertificationRecordOutput
                    {
                        BuildingId = item.BuildingId,
                        BuildingName = item.BuildingName,
                        BuildingUnitId = item.BuildingUnitId,
                        BuildingUnitName = item.BuildingUnitName,
                        CertificationResult = item.CertificationResult,
                        CertificationStatusName = item.CertificationStatusName,
                        CertificationStatusValue = item.CertificationStatusValue,
                        //CertificationTime = item.CertificationTime,
                        CommunityId = item.CommunityId,
                        CommunityName = item.CommunityName,
                        Id = item.Id.ToString(),
                        IndustryId = item.IndustryId,
                        IndustryName = item.IndustryName,
                        OwnerId = item.OwnerId,
                        OwnerName = item.OwnerName,
                        SmallDistrictId = item.SmallDistrictId,
                        SmallDistrictName = item.SmallDistrictName,
                        StreetOfficeId = item.StreetOfficeId,
                        StreetOfficeName = item.StreetOfficeName,
                        UserId = item.UserId,
                        Name = owner?.Name,
                        Birthday = owner?.Birthday,
                        Gender = owner?.Gender,
                        IDCard = owner?.IDCard,
                        PhoneNumber = owner?.PhoneNumber,
                        NumberOfLayers = industry?.NumberOfLayers,
                        Acreage = industry?.Acreage
                    });
                }
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success, new GetListOwnerCertificationRecordOutput
                {
                    List = list
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success_NoB, new GetListOwnerCertificationRecordOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 获取后台用投诉状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("complaintStatus/getAll")]
        public ApiResult<List<dynamic>> ComplaintStatusGetAll() => new ApiResult<List<dynamic>>(APIResultCode.Success, new List<dynamic>
        {
            new { Name="未处理", ComplaintStatus.NotAccepted.Value },
            new { Name="待反馈", ComplaintStatus.Processing.Value },
            new { Name="已处理", ComplaintStatus.Finished.Value },
            new { Name="已完结", ComplaintStatus.Completed.Value }
        });

        /// <summary>
        /// 获取部门集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("department/getAll")]
        public ApiResult<List<Department>> GetAll() => new ApiResult<List<Department>>(APIResultCode.Success, Department.GetAll().ToList());

        /// <summary>
        /// 业主投诉上级部门集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("department/getAllForOwner")]
        public ApiResult<List<Department>> GetAllForOwner() => new ApiResult<List<Department>>(APIResultCode.Success, new List<Department>
        {
            Department.YeZhuWeiYuanHui,
            Department .WuYe
        });

        /// <summary>
        /// 业委会投诉上级部门集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("department/getAllForVipOwner")]
        public ApiResult<List<Department>> GetAllForVipOwner() => new ApiResult<List<Department>>(APIResultCode.Success, new List<Department>
        {
            Department.JieDaoBan,
            Department .WuYe
        }.ToList());

        /// <summary>
        /// 获取服务器Id
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("common/getIp")]
        public ApiResult<string> GetIp() => new ApiResult<string>(APIResultCode.Success, IpUtility.GetLocalIP());

        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("fileType/getAll")]
        public ApiResult<List<FileType>> GetFileTypeAll() => new ApiResult<List<FileType>>(APIResultCode.Success, FileType.GetAll().ToList());

        /// <summary>
        /// 获取业主认证上传附件类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ownerCertificationAnnexType/getAll")]
        public ApiResult<List<OwnerCertificationAnnexType>> GetOwnerCertificationAnnexTypeAll() => new ApiResult<List<OwnerCertificationAnnexType>>(APIResultCode.Success, OwnerCertificationAnnexType.GetAll().ToList());

    }
}
