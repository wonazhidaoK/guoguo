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
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 公共接口
    /// </summary>
    public class CommonController : BaseController
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

        /// <summary>
        /// 获取用户小区集合
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("smallDistrict/getListForUserId")]
        public async Task<ApiResult<List<GetListForUserIdOutput>>> GetListForUserId(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<List<GetListForUserIdOutput>>(APIResultCode.Unknown, new List<GetListForUserIdOutput> { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<List<GetListForUserIdOutput>>(APIResultCode.Unknown, new List<GetListForUserIdOutput> { }, APIResultMessage.TokenError);
            }
            var a = (await _ownerCertificationRecordRepository.GetListIncludeAsync(new OwnerCertificationRecordDto
            {
                UserId = user.Id.ToString()
            }, cancelToken));
            var data = (await _ownerCertificationRecordRepository.GetListIncludeAsync(new OwnerCertificationRecordDto
            {
                UserId = user.Id.ToString()
            }, cancelToken)).Select(x => x.Industry.BuildingUnit.Building.SmallDistrictId).Distinct();

            List<GetListForUserIdOutput> list = new List<GetListForUserIdOutput>();
            var smallDistrictList = await _smallDistrictRepository.GetForIdsAsync(data.Select(x => x.ToString()).ToList(), cancelToken);
            foreach (var item in data)
            {
                var entity = smallDistrictList.Where(x => x.Id == item).FirstOrDefault();
                list.Add(new GetListForUserIdOutput
                {
                    Id = entity.Id.ToString(),
                    Name = entity.Name
                });
            }
            return new ApiResult<List<GetListForUserIdOutput>>(APIResultCode.Success, list);
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
            if (Authorization == null)
            {
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
            }
            var user = _tokenManager.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
            }
            var data = await _ownerCertificationRecordRepository.GetAllForSmallDistrictIdIncludeAsync(new OwnerCertificationRecordDto
            {
                UserId = user.Id.ToString(),
                SmallDistrictId = SmallDistrictId
            }, cancelToken);

            List<GetOwnerCertificationRecordOutput> list = new List<GetOwnerCertificationRecordOutput>();
            var ownerList = await _ownerRepository.GetForIdsAsync(data.Select(x => x.OwnerId.ToString()).ToList(), cancelToken);
            var industryList = await _industryRepository.GetForIdsAsync(data.Select(x => x.IndustryId.ToString()).ToList(), cancelToken);

            foreach (var item in data)
            {
                var owner = ownerList.Where(x => x.Id == item.OwnerId).FirstOrDefault();
                var industry = industryList.Where(x => x.Id == item.IndustryId).FirstOrDefault();
                list.Add(new GetOwnerCertificationRecordOutput
                {
                    BuildingId = item. Industry.BuildingUnit.BuildingId.ToString(),
                    BuildingName = item. Industry.BuildingUnit.Building.Name,
                    BuildingUnitId = item. Industry.BuildingUnitId.ToString(),
                    BuildingUnitName = item. Industry.BuildingUnit.UnitName,
                    CertificationResult = item.CertificationResult,
                    CertificationStatusName = item.CertificationStatusName,
                    CertificationStatusValue = item.CertificationStatusValue,
                    CommunityId = item. Industry.BuildingUnit.Building.SmallDistrict.CommunityId.ToString(),
                    CommunityName = item. Industry.BuildingUnit.Building.SmallDistrict.Community.Name,
                    Id = item.Id.ToString(),
                    IndustryId = item.IndustryId.ToString(),
                    IndustryName = item.Industry.Name,
                    OwnerId = item.OwnerId.ToString(),
                    OwnerName = item.Owner.Name,
                    SmallDistrictId = item.Industry.BuildingUnit.Building.SmallDistrictId.ToString(),
                    SmallDistrictName = item.Industry.BuildingUnit.Building.SmallDistrict.Name,
                    StreetOfficeId = item.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOfficeId.ToString(),
                    StreetOfficeName = item.Industry.BuildingUnit.Building.SmallDistrict.Community.StreetOffice.Name,
                    UserId = item.UserId,
                    Name = owner?.Name,
                    Birthday = owner?.Birthday,
                    Gender = owner?.Gender,
                    IDCard = owner?.IDCard,
                    PhoneNumber = owner?.PhoneNumber,
                    NumberOfLayers = industry?.NumberOfLayers,
                    Acreage = industry?.Acreage,
                    Oriented = industry?.Oriented
                });
            }
            return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success, new GetListOwnerCertificationRecordOutput
            {
                List = list
            });
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
            new { Name="处理中", ComplaintStatus.Processing.Value },
            new { Name="已完结", ComplaintStatus.Finished.Value },
            new { Name="已完成", ComplaintStatus.Completed.Value }
        });

        /// <summary>
        /// 后台创建账户获取部门集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("department/getAll")]
        public ApiResult<List<Department>> GetAll() => new ApiResult<List<Department>>(APIResultCode.Success, new List<Department>
        {
            Department.JieDaoBan,
            Department .WuYe
        }.ToList());

        /// <summary>
        /// 创建投诉类型获取部门集合
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("department/getForComplaintAll")]
        public ApiResult<List<Department>> GetForComplaintAll() => new ApiResult<List<Department>>(APIResultCode.Success, new List<Department>
        {
            Department.YeZhu,
            Department.YeZhuWeiYuanHui
        }.ToList());

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
