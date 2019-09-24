using GuoGuoCommunity.API.Common;
using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 上传
    /// </summary>
    public class UploadController : BaseController
    {
        private readonly IUploadRepository _uploadRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IBuildingUnitRepository _buildingUnitRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly IOwnerRepository _ownerRepository;

        /// <summary>
        /// 
        /// </summary>
        public UploadController(
            IUploadRepository uploadRepository,
            ITokenRepository tokenRepository,
            IBuildingRepository buildingRepository,
            IBuildingUnitRepository buildingUnitRepository,
            IIndustryRepository industryRepository,
            IOwnerRepository ownerRepository)
        {
            _uploadRepository = uploadRepository;
            _tokenRepository = tokenRepository;
            _buildingRepository = buildingRepository;
            _buildingUnitRepository = buildingUnitRepository;
            _industryRepository = industryRepository;
            _ownerRepository = ownerRepository;
        }

        /// <summary>
        /// 业主认证上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadOwnerCertification")]
        public async Task<ApiResult<UploadOutput>> UploadOwnerCertification(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "OwnerCertification";
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }
                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 高级认证上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadVipOwnerCertificationRecord")]
        public async Task<ApiResult<UploadOutput>> UploadVipOwnerCertificationRecord(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "VipOwnerCertification";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 公告上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadAnnouncement")]
        public async Task<ApiResult<UploadOutput>> UploadAnnouncement(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "Announcement";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 投诉上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadComplaint")]
        public async Task<ApiResult<UploadOutput>> UploadComplaint(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "Complaint";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 投票上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadVote")]
        public async Task<ApiResult<UploadOutput>> UploadVote(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "Vote";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 站内信上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadStationLetter")]
        public async Task<ApiResult<UploadOutput>> UploadStationLetter(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "StationLetter";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await AddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }



        /// <summary>
        /// 上传物业公司Logo图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadPropertyCompany")]
        public async Task<ApiResult<UploadOutput>> UploadPropertyCompany(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "PropertyCompany";
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }
                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 商户账户创建上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadShop")]
        public async Task<ApiResult<UploadOutput>> UploadShop(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "Shop";

                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 上传商户商品图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadShopCommodity")]
        public async Task<ApiResult<UploadOutput>> UploadShopCommodity(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "ShopCommodity";
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);
                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }
                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 上传平台商品图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/uploadPlatformCommodityCertification")]
        public async Task<ApiResult<UploadOutput>> UploadPlatformCommodityCertification(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<UploadOutput>(APIResultCode.Unknown, new UploadOutput { }, APIResultMessage.TokenError);
                }

                #endregion

                string typeName = "PlatformCommodityCertification";
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);

                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }
                return new ApiResult<UploadOutput>(APIResultCode.Success, files[0], APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<UploadOutput>(APIResultCode.Error, new UploadOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 上传业主信息表格文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/ownerInfoFileUpload")]
        public async Task<ApiResult<dynamic>> UploadOwnerInfoFile(CancellationToken cancelToken)
        {
            try
            {
                #region Token

                if (Authorization == null)
                {
                    return new ApiResult<dynamic>(APIResultCode.Unknown, new { }, APIResultMessage.TokenNull);
                }
                var user = _tokenRepository.GetUser(Authorization);
                if (user == null)
                {
                    return new ApiResult<dynamic>(APIResultCode.Unknown, new { }, APIResultMessage.TokenError);
                }
                if (user.DepartmentValue != Department.WuYe.Value)
                {
                    return new ApiResult<dynamic>(APIResultCode.Success_NoB, new { }, "只能通过物业导入业主信息！");
                }

                #endregion

                string typeName = "OwnerInfoFile";
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }

                string fileSaveLocation = HttpContext.Current.Server.MapPath("~/Upload/" + typeName);

                CustomMultipartFormDataStreamProvider provider = new CustomMultipartFormDataStreamProvider(fileSaveLocation);
                List<UploadOutput> files = new List<UploadOutput>();

                await Request.Content.ReadAsMultipartAsync(provider, cancelToken);

                foreach (MultipartFileData file in provider.FileData)
                {
                    string orfilename = file.Headers.ContentDisposition.FileName.TrimStart('"').TrimEnd('"');
                    string fileExt = orfilename.Substring(orfilename.LastIndexOf('.'));
                    FileInfo fileinfo = new FileInfo(file.LocalFileName);
                    var newPath = DateTime.Now.ToString("yyyyMMddhhmmssffffff") + fileExt;
                    fileinfo.CopyTo(Path.Combine(fileSaveLocation, newPath), true);
                    fileinfo.Delete();
                    files.Add(await NewAddUpload(typeName, newPath, user.Id.ToString(), cancelToken));
                }

                var data = AsposeCellsUtility.ImportExcel(HttpContext.Current.Server.MapPath("~/Upload/") + files[0].Url);

                //不是本小区数据跳过数
                var notSmallDistrictCount = 0;
                //业户下包含业主信息跳过数
                var containOwnerCount = 0;
                //身份证号为空跳过数
                var iDCardCount = 0;
                //成功添加数
                var successCount = 0;

                if (data.Rows.Count > 0)
                {
                    foreach (DataRow item in data.Rows)
                    {
                        if (string.IsNullOrEmpty(item["省"].ToString()))
                        {
                            continue;
                        }
                        var state = item["省"].ToString();
                        var city = item["市"].ToString();
                        var region = item["区"].ToString();
                        var streetOfficeName = item["街道办"].ToString();
                        var communityName = item["社区"].ToString();
                        var smallDistrictName = item["小区"].ToString();
                        var NumberOfLayers = Convert.ToInt32(item["层"].ToString());

                        if (user.SmallDistrictName != smallDistrictName || user.CommunityName != communityName || user.StreetOfficeName != streetOfficeName || user.Region != region || user.City != city || user.State != state)
                        {
                            notSmallDistrictCount += 1;
                            continue;
                        }

                        //根据当前的小区id查询 小区下是否存在 楼宇信息 不存在添加
                        var build = (await _buildingRepository.GetListAsync(new BuildingDto { SmallDistrictId = user.SmallDistrictId })).Where(x => x.Name == item["楼宇"].ToString()).FirstOrDefault();
                        if (build == null)
                        {
                            build = await _buildingRepository.AddAsync(
                                new BuildingDto
                                {
                                    SmallDistrictId = user.SmallDistrictId,
                                    SmallDistrictName = user.SmallDistrictName,
                                    OperationTime = DateTime.Now,
                                    OperationUserId = user.Id.ToString(),
                                    Name = item["楼宇"].ToString()
                                });
                        }

                        //根据楼宇信息查询单元信息
                        var buildingUnit = (await _buildingUnitRepository.GetListAsync(new BuildingUnitDto { BuildingId = build.Id.ToString() })).Where(x => x.UnitName == item["单元"].ToString()).FirstOrDefault();
                        if (buildingUnit == null)
                        {
                            buildingUnit = await _buildingUnitRepository.AddAsync(
                                new BuildingUnitDto
                                {
                                    UnitName = item["单元"].ToString(),
                                    BuildingId = build.Id.ToString(),
                                    NumberOfLayers = NumberOfLayers,
                                    OperationTime = DateTime.Now,
                                    OperationUserId = user.Id.ToString()
                                });
                        }
                        else if (buildingUnit.NumberOfLayers < NumberOfLayers)
                        {
                            buildingUnit = await _buildingUnitRepository.UpdateNumberOfLayersAsync(
                                new BuildingUnitDto
                                {
                                    Id = buildingUnit.Id.ToString(),
                                    NumberOfLayers = NumberOfLayers,
                                    OperationTime = DateTime.Now,
                                    OperationUserId = user.Id.ToString()
                                });
                        }

                        //查找业户 业户不存在添加业户
                        var industry = (await _industryRepository.GetListAsync(
                            new IndustryDto
                            {
                                NumberOfLayers = NumberOfLayers,
                                BuildingUnitId = buildingUnit.Id.ToString()
                            })).Where(x => x.Name == item["门牌号"].ToString()).FirstOrDefault();
                        if (industry == null)
                        {
                            industry = await _industryRepository.AddAsync(
                                new IndustryDto
                                {
                                    Name = item["门牌号"].ToString(),
                                    Acreage = item["面积"].ToString(),
                                    BuildingId = build.Id.ToString(),
                                    BuildingName = build.Name,
                                    BuildingUnitId = buildingUnit.Id.ToString(),
                                    BuildingUnitName = buildingUnit.UnitName,
                                    NumberOfLayers = NumberOfLayers,
                                    Oriented = item["朝向"].ToString(),
                                    OperationTime = DateTime.Now,
                                    OperationUserId = user.Id.ToString(),
                                    OperationUserSmallDistrictId = user.SmallDistrictId
                                });
                        }

                        //查询当前业户下业主
                        var owner = (await _ownerRepository.GetListAsync(
                            new OwnerDto
                            {
                                IndustryId = industry.Id.ToString()
                            })).FirstOrDefault();
                        if (owner != null)
                        {
                            containOwnerCount += 1;
                            continue;
                        }
                        if (string.IsNullOrEmpty(item["身份证号"].ToString()))
                        {
                            iDCardCount += 1;
                            continue;
                        }
                        var birthday = "";
                        if (DateTime.TryParse(item["生日"].ToString(), out var format))
                        {
                            birthday = format.ToString("yyyyMMdd");
                        }
                        owner = await _ownerRepository.AddAsync(
                            new OwnerDto
                            {
                                IDCard = item["身份证号"].ToString(),
                                Birthday = birthday,
                                Gender = item["性别"].ToString(),
                                IndustryId = industry.Id.ToString(),
                                //IndustryName = industry.Name,
                                Name = item["姓名"].ToString(),
                                PhoneNumber = item["手机号"].ToString(),
                                OperationUserId = user.Id.ToString(),
                                OperationTime = DateTime.Now
                            });
                        successCount += 1;
                    }
                }
                var mgs = "成功导入:" + successCount + ",身份证号为空跳过数:" + iDCardCount + ",业户下包含业主信息跳过数:" + containOwnerCount + ",不是本小区数据跳过数:" + notSmallDistrictCount;
                return new ApiResult<dynamic>(APIResultCode.Success, new { mgs }, APIResultMessage.Success);
            }
            catch (Exception e)
            {
                return new ApiResult<dynamic>(APIResultCode.Error, new { }, "导入失败:" + e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <param name="cancelToken"></param>
        private async Task<UploadOutput> AddUpload(string directory, string file, string userId, CancellationToken cancelToken = default)
        {
            var upload = await _uploadRepository.AddAsync(
                  new UploadDto
                  {
                      Agreement = Agreement + "://",
                      Host = Host + "/",
                      Domain = "/Upload",
                      Directory = "/" + directory,
                      File = "/" + file,
                      OperationTime = DateTimeOffset.Now,
                      OperationUserId = userId
                  }, cancelToken);
            if (upload != null)
            {
                return new UploadOutput { Id = upload.Id.ToString(), Url = upload.Agreement + upload.Host + upload.Domain + upload.Directory + upload.File };
            }
            return new UploadOutput { };
        }

        /// <summary>
        /// 服务器地址和业务存储地址分开返回
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <param name="cancelToken"></param>
        private async Task<UploadOutput> NewAddUpload(string directory, string file, string userId, CancellationToken cancelToken = default)
        {
            var upload = await _uploadRepository.AddAsync(
                  new UploadDto
                  {
                      Agreement = Agreement + "://",
                      Host = Host + "/",
                      Domain = "/Upload",
                      Directory = "/" + directory,
                      File = "/" + file,
                      OperationTime = DateTimeOffset.Now,
                      OperationUserId = userId
                  }, cancelToken);
            if (upload != null)
            {
                return new UploadOutput { Id = upload.Id.ToString(), Url = upload.Directory + upload.File };
            }
            return new UploadOutput { };
        }

        /// <summary>
        /// 
        /// </summary>
        public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="path"></param>
            public CustomMultipartFormDataStreamProvider(string path) : base(path) { }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="headers"></param>
            /// <returns></returns>
            public override string GetLocalFileName(HttpContentHeaders headers)
            {
                return headers.ContentDisposition.FileName.Replace("\"", string.Empty);
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        [Route("upload/image")]
        public string UploadImage()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            HttpPostedFile filess = HttpContext.Current.Request.Files["card"];//接收
            string sFileName = AppDomain.CurrentDomain.BaseDirectory + filess.FileName;
            filess.SaveAs(sFileName);
            return "";
        }
    }
}
