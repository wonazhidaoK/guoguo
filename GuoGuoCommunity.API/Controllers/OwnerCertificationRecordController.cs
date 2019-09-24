using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.TemplateMessage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 业主认证
    /// </summary>
    public class OwnerCertificationRecordController : BaseController
    {
        private readonly IOwnerCertificationRecordRepository _ownerCertificationRecordRepository;
        private readonly IOwnerCertificationAnnexRepository _ownerCertificationAnnexRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IIndustryRepository _industryRepository;
        private readonly IIDCardPhotoRecordRepository _iDCardPhotoRecordRepository;
        private readonly IVipOwnerRepository _vipOwnerRepository;
        private readonly IVipOwnerCertificationRecordRepository _vipOwnerCertificationRecordRepository;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// 
        /// </summary>
        public OwnerCertificationRecordController(
            IOwnerCertificationRecordRepository ownerCertificationRecordRepository,
            IOwnerCertificationAnnexRepository ownerCertificationAnnexRepository,
            IOwnerRepository ownerRepository,
            IIndustryRepository industryRepository,
            IIDCardPhotoRecordRepository iDCardPhotoRecordRepository,
            IVipOwnerCertificationRecordRepository vipOwnerCertificationRecordRepository,
            IVipOwnerRepository vipOwnerRepository,
            ITokenRepository tokenRepository)
        {
            _ownerCertificationRecordRepository = ownerCertificationRecordRepository;
            _ownerCertificationAnnexRepository = ownerCertificationAnnexRepository;
            _ownerRepository = ownerRepository;
            _industryRepository = industryRepository;
            _iDCardPhotoRecordRepository = iDCardPhotoRecordRepository;
            _vipOwnerCertificationRecordRepository = vipOwnerCertificationRecordRepository;
            _vipOwnerRepository = vipOwnerRepository;
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// 添加业主认证记录信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ownerCertificationRecord/add")]
        public async Task<ApiResult<AddOwnerCertificationRecordOutput>> Add([FromBody]AddOwnerCertificationRecordInput input, CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Unknown, new AddOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
            }
            if (string.IsNullOrWhiteSpace(input.StreetOfficeId))
            {
                throw new NotImplementedException("街道办Id信息为空！");
            }

            if (string.IsNullOrWhiteSpace(input.CommunityId))
            {
                throw new NotImplementedException("社区Id信息为空！");
            }

            if (string.IsNullOrWhiteSpace(input.SmallDistrictId))
            {
                throw new NotImplementedException("小区Id信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.BuildingId))
            {
                throw new NotImplementedException("楼宇Id信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.BuildingUnitId))
            {
                throw new NotImplementedException("单元Id信息为空！");
            }
            if (string.IsNullOrWhiteSpace(input.IndustryId))
            {
                throw new NotImplementedException("业户Id信息为空！");
            }
            if (input.Models.Count < 2)
            {
                throw new NotImplementedException("提交附件信息不准确！");
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Unknown, new AddOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
            }

            var entity = await _ownerCertificationRecordRepository.AddAsync(new OwnerCertificationRecordDto
            {
                SmallDistrictId = input.SmallDistrictId,
                IndustryId = input.IndustryId,
                CommunityId = input.CommunityId,
                StreetOfficeId = input.StreetOfficeId,
                BuildingUnitId = input.BuildingUnitId,
                BuildingId = input.BuildingId,
                UserId = user.Id.ToString(),
                OperationTime = DateTimeOffset.Now,
                OperationUserId = user.Id.ToString(),
            }, cancelToken);

            foreach (var item in input.Models)
            {
                if (string.IsNullOrWhiteSpace(item.AnnexContent))
                {
                    continue;
                }
                var itemEntity = await _ownerCertificationAnnexRepository.AddAsync(new OwnerCertificationAnnexDto
                {
                    ApplicationRecordId = entity.Id.ToString(),
                    OwnerCertificationAnnexTypeValue = item.OwnerCertificationAnnexTypeValue,
                    AnnexContent = item.AnnexContent,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                });

                if (itemEntity.OwnerCertificationAnnexTypeValue == OwnerCertificationAnnexType.IDCardFront.Value)
                {
                    await Verification(itemEntity);
                }
            }

            return new ApiResult<AddOwnerCertificationRecordOutput>(APIResultCode.Success, new AddOwnerCertificationRecordOutput { Id = entity.Id.ToString() });
        }

        /// <summary>
        /// 获取用户认证列表
        /// </summary>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ownerCertificationRecord/getList")]
        public async Task<ApiResult<GetListOwnerCertificationRecordOutput>> GetList(CancellationToken cancelToken)
        {
            if (Authorization == null)
            {
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
            if (user == null)
            {
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenError);
            }
            var data = await _ownerCertificationRecordRepository.GetListIncludeAsync(new OwnerCertificationRecordDto
            {
                UserId = user.Id.ToString()
            }, cancelToken);

            List<GetOwnerCertificationRecordOutput> list = new List<GetOwnerCertificationRecordOutput>();
            // var ownerList = await _ownerRepository.GetForIdsAsync(data.Select(x => x.OwnerId.ToString()).ToList(), cancelToken);
            // var industryList = await _industryRepository.GetForIdsAsync(data.Select(x => x.IndustryId.ToString()).ToList(), cancelToken);
            //var smallDistrictList = await _smallDistrictRepository.GetForIdsIncludeAsync(data.Select(x => x.Industry.BuildingUnit.Building.SmallDistrictId.ToString()).ToList(), cancelToken);
            var vipOwnerList = await _vipOwnerRepository.GetForSmallDistrictIdsAsync(data.Select(x => x.Industry.BuildingUnit.Building.SmallDistrictId.ToString()).ToList());
            var vipOwnerCertificationRecordList = await _vipOwnerCertificationRecordRepository.GetForVipOwnerIdsAsync(vipOwnerList.Select(x => x.Id.ToString()).ToList());
            foreach (var item in data)
            {
                //var owner = ownerList.Where(x => x.Id == item.OwnerId).FirstOrDefault();
                //var industry = industryList.Where(x => x.Id.ToString() == item.IndustryId.ToString()).FirstOrDefault();
                //var smallDistrict = smallDistrictList.Where(x => x.Id == item.Industry.BuildingUnit.Building.SmallDistrictId).FirstOrDefault();
                var isVipOwner = false;
                var vipOwner = vipOwnerList.Where(x => x.SmallDistrictId.ToString() == item.Industry.BuildingUnit.Building.SmallDistrictId.ToString()).FirstOrDefault();
                if (vipOwner != null)
                {
                    var vipOwnerCertificationRecord = vipOwnerCertificationRecordList.Where(x => x.VipOwnerId == vipOwner.Id.ToString() && x.OwnerCertificationId == item.Id.ToString()).FirstOrDefault();
                    if (vipOwnerCertificationRecord != null)
                    {
                        isVipOwner = true;
                    }
                }
                list.Add(new GetOwnerCertificationRecordOutput
                {
                    City = item.Industry.BuildingUnit.Building.SmallDistrict.City,
                    Region = item.Industry.BuildingUnit.Building.SmallDistrict.Region,
                    State = item.Industry.BuildingUnit.Building.SmallDistrict.State,
                    BuildingId = item.Industry.BuildingUnit.BuildingId.ToString(),
                    BuildingName = item.Industry.BuildingUnit.Building.Name,
                    BuildingUnitId = item.Industry.BuildingUnitId.ToString(),
                    BuildingUnitName = item.Industry.BuildingUnit.UnitName,
                    CertificationResult = item.CertificationResult,
                    CertificationStatusName = item.CertificationStatusName,
                    CertificationStatusValue = item.CertificationStatusValue,
                    CommunityId = item.Industry.BuildingUnit.Building.SmallDistrict.CommunityId.ToString(),
                    CommunityName = item.Industry.BuildingUnit.Building.SmallDistrict.Community.Name,
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
                    Name = item.Owner?.Name,
                    Birthday = item.Owner?.Birthday,
                    Gender = item.Owner?.Gender,
                    IDCard = item.Owner?.IDCard,
                    PhoneNumber = item.Owner?.PhoneNumber,
                    NumberOfLayers = item.Industry?.NumberOfLayers,
                    Acreage = item.Industry?.Acreage,
                    Oriented = item.Industry?.Oriented,
                    SmallDistrictPhoneNumber = item.Industry.BuildingUnit.Building.SmallDistrict?.PhoneNumber,
                    IsVipOwner = isVipOwner
                });
            }
            return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success, new GetListOwnerCertificationRecordOutput
            {
                List = list
            });
        }

        /// <summary>
        /// 后台执行校验 上传图片认证信息
        /// </summary>
        /// <param name="annex"></param>
        private async Task<ApiResult> Verification(OwnerCertificationAnnex annex)
        {
            var ownerCertificationRecordEntity = await _ownerCertificationRecordRepository.GetIncludeAsync(annex.ApplicationRecordId.ToString());
            try
            {
                OwnerCertificationRecordDto dto = new OwnerCertificationRecordDto
                {
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                    Id = ownerCertificationRecordEntity.Id.ToString()
                };

                try
                {
                    var entity = await PostALiYun(annex);
                    JsonModel json = new JsonModel();
                    try
                    {
                        json = JsonConvert.DeserializeObject<JsonModel>(entity.Message);
                        if (string.IsNullOrEmpty(json.Num))
                        {
                            throw new NotImplementedException("未识别到身份证信息，请提交正规清晰的身份证照片!");
                        }
                    }
                    catch (Exception)
                    {
                        throw new NotImplementedException("未识别到身份证信息，请提交正规清晰的身份证照片!");
                    }
                    var owner = (await _ownerRepository.GetListForLegalizeIncludeAsync(new OwnerDto { IndustryId = ownerCertificationRecordEntity.IndustryId.ToString() })).Where(x => x.IDCard == json.Num).FirstOrDefault();

                    if (owner != null)
                    {
                        dto.CertificationStatusValue = OwnerCertificationStatus.Success.Value;
                        dto.CertificationStatusName = OwnerCertificationStatus.Success.Name;
                        dto.OwnerId = owner.Id.ToString();
                        dto.OwnerName = owner.Name.ToString();
                        dto.CertificationResult = "认证通过";
                    }
                    else
                    {
                        dto.CertificationStatusValue = OwnerCertificationStatus.Failure.Value;
                        dto.CertificationStatusName = OwnerCertificationStatus.Failure.Name;
                        dto.CertificationResult = "未查询到相关业主信息";
                    }

                }
                catch (Exception e)
                {
                    dto.CertificationStatusValue = OwnerCertificationStatus.Failure.Value;
                    dto.CertificationStatusName = OwnerCertificationStatus.Failure.Name;
                    dto.CertificationResult = e.Message;
                    throw new NotImplementedException(e.Message);
                }
                var recordEntity = await _ownerCertificationRecordRepository.UpdateStatusAsync(dto);

                if (string.IsNullOrWhiteSpace(dto.OwnerId))
                {
                    throw new NotImplementedException("未查询到相关业主信息");
                }

                await _ownerRepository.UpdateForLegalizeAsync(new OwnerDto
                {
                    OwnerCertificationRecordId = ownerCertificationRecordEntity.Id.ToString(),
                    Id = recordEntity.OwnerId.ToString(),
                    Name = dto.OwnerName,
                });

                return new ApiResult(APIResultCode.Success);
            }
            catch (Exception e)
            {
                await _ownerCertificationRecordRepository.UpdateInvalidAsync(new OwnerCertificationRecordDto
                {
                    Id = ownerCertificationRecordEntity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                });
                await _ownerCertificationRecordRepository.DeleteAsync(new OwnerCertificationRecordDto
                {
                    Id = ownerCertificationRecordEntity.Id.ToString(),
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                });
                throw new NotImplementedException(e.Message);
            }
        }

        /// <summary>
        /// 调用阿里云接口
        /// </summary>
        /// <param name="annex"></param>
        /// <returns></returns>
        private async Task<IDCardPhotoRecord> PostALiYun(OwnerCertificationAnnex annex)
        {
            string aLiYunApiUrl = ALiYunApiUrl;
            string appcode = ALiYunApiAppCode;

            var url = _ownerCertificationAnnexRepository.GetPath(annex.Id.ToString());
            string img_file = HttpRuntime.AppDomainAppPath.ToString() + url;

            bool is_old_format = false;

            string config = "{\\\"side\\\":\\\"face\\\"}";

            string method = "POST";

            string querys = "";

            using (FileStream fs = new FileStream(img_file, FileMode.Open))
            {
                BinaryReader br = new BinaryReader(fs);
                byte[] contentBytes = br.ReadBytes(Convert.ToInt32(fs.Length));
                string base64 = Convert.ToBase64String(contentBytes);
                string bodys;
                if (is_old_format)
                {
                    bodys = "{\"inputs\" :" +
                                        "[{\"image\" :" +
                                            "{\"dataType\" : 50," +
                                             "\"dataValue\" :\"" + base64 + "\"" +
                                             "}";
                    if (config.Length > 0)
                    {
                        bodys += ",\"configure\" :" +
                                        "{\"dataType\" : 50," +
                                         "\"dataValue\" : \"" + config + "\"}" +
                                         "}";
                    }
                    bodys += "]}";
                }
                else
                {
                    bodys = "{\"image\":\"" + base64 + "\"";
                    if (config.Length > 0)
                    {
                        bodys += ",\"configure\" :\"" + config + "\"";
                    }
                    bodys += "}";
                }
                HttpWebRequest httpRequest = null;
                HttpWebResponse httpResponse = null;

                if (0 < querys.Length)
                {
                    aLiYunApiUrl = aLiYunApiUrl + "?" + querys;
                }

                if (aLiYunApiUrl.Contains("https://"))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(aLiYunApiUrl));
                }
                else
                {
                    httpRequest = (HttpWebRequest)WebRequest.Create(aLiYunApiUrl);
                }
                httpRequest.Method = method;
                httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
                //根据API的要求，定义相对应的Content-Type
                httpRequest.ContentType = "application/json; charset=UTF-8";
                if (0 < bodys.Length)
                {
                    byte[] data = Encoding.UTF8.GetBytes(bodys);
                    using (Stream stream = httpRequest.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }
                }
                try
                {
                    httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                }
                catch (WebException ex)
                {
                    httpResponse = (HttpWebResponse)ex.Response;
                }
                string json;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    Console.WriteLine("http error code: " + httpResponse.StatusCode);
                    Console.WriteLine("error in header: " + httpResponse.GetResponseHeader("X-Ca-Error-Message"));
                    Console.WriteLine("error in body: ");
                    Stream st = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                    json = reader.ReadToEnd();
                    Console.WriteLine(reader.ReadToEnd());
                }
                else
                {
                    Stream st = httpResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                    json = reader.ReadToEnd();
                }
                var entity = await _iDCardPhotoRecordRepository.AddAsync(new IDCardPhotoRecordDto
                {
                    ApplicationRecordId = annex.ApplicationRecordId.ToString(),
                    OwnerCertificationAnnexId = annex.Id.ToString(),
                    Message = json,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = "system",
                    PhotoBase64 = base64,
                });
                return entity;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #region 

        /// <summary>
        /// 认证结果通知
        /// </summary>
        [Obsolete]
        public static void OwnerCertificationRecordPushRemind(OwnerCertificationRecordPushModel model)
        {
            try
            {
                var templateData = new
                {
                    first = new TemplateDataItem("用户认证通知"),
                    keyword1 = new TemplateDataItem(DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss\r\n")),
                    keyword2 = new TemplateDataItem(model.Status),
                    keyword3 = new TemplateDataItem(model.Message),
                    remark = new TemplateDataItem("详情", "#FF0000")
                };

                var miniProgram = new TempleteModel_MiniProgram()
                {
                    appid = GuoGuoCommunity_WxOpenAppId,
                    pagepath = "pages/my/my"
                };

                TemplateApi.SendTemplateMessage(AppId, model.OpenId, OwnerCertificationRecordTemplateId, null, templateData, miniProgram);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Obsolete]
        public string GetPath(string id)
        {
            DirectoryInfo rootDir = Directory.GetParent(Environment.CurrentDirectory);
            string root = rootDir.Parent.Parent.FullName;
            string a = HttpRuntime.AppDomainAppPath.ToString();
            return HttpRuntime.AppDomainAppPath.ToString();
        }

        /// <summary>
        /// 根据小区id获取用户id
        /// </summary>
        /// <param name="SmallDistrictId"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [Obsolete]
        [HttpGet]
        [Route("ownerCertificationRecord/getAllForSmallDistrictId2")]
        public async Task<ApiResult<GetListOwnerCertificationRecordOutput>> GetAllForSmallDistrictId([FromUri]string SmallDistrictId, CancellationToken cancelToken)
        {

            if (Authorization == null)
            {
                return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Unknown, new GetListOwnerCertificationRecordOutput { }, APIResultMessage.TokenNull);
            }

            var user = _tokenRepository.GetUser(Authorization);
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

            foreach (var item in data)
            {
                var owner = await _ownerRepository.GetAsync(item.OwnerId.ToString(), cancelToken);
                var industry = await _industryRepository.GetAsync(item.IndustryId.ToString(), cancelToken);
                list.Add(new GetOwnerCertificationRecordOutput
                {
                    BuildingId = item.Industry.BuildingUnit.BuildingId.ToString(),
                    BuildingName = item.Industry.BuildingUnit.Building.Name,
                    BuildingUnitId = item.Industry.BuildingUnitId.ToString(),
                    BuildingUnitName = item.Industry.BuildingUnit.UnitName,
                    CertificationResult = item.CertificationResult,
                    CertificationStatusName = item.CertificationStatusName,
                    CertificationStatusValue = item.CertificationStatusValue,
                    CertificationTime = item.CreateOperationTime.ToString(),
                    CommunityId = item.Industry.BuildingUnit.Building.SmallDistrict.CommunityId.ToString(),
                    CommunityName = item.Industry.BuildingUnit.Building.SmallDistrict.Community.Name,
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
                    Acreage = industry?.Acreage
                });
            }
            return new ApiResult<GetListOwnerCertificationRecordOutput>(APIResultCode.Success, new GetListOwnerCertificationRecordOutput
            {
                List = list
            });
        }

        #endregion
    }
}
