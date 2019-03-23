using GuoGuoCommunity.API.Models;
using GuoGuoCommunity.Domain;
using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GuoGuoCommunity.API.Controllers
{
    /// <summary>
    /// 站内信相关
    /// </summary>
    public class StationLetterController : ApiController
    {
        private readonly IStationLetterRepository _stationLetterRepository;
        private TokenManager _tokenManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationLetterRepository"></param>
        /// <param name="tokenManager"></param>
        public StationLetterController(IStationLetterRepository stationLetterRepository, TokenManager tokenManager)
        {
            _stationLetterRepository = stationLetterRepository;
            _tokenManager = tokenManager;
        }
        /*
         * 1.街道办后台添加站内信
         * 2.街道办后台展示站内信列表
         * 3.物业后台展示站内信列表
         * 4.物业后台查看站内信详情标记已读
         */

        /// <summary>
        /// 添加站内信(街道办后台)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("stationLetter/add")]
        public async Task<ApiResult<AddStationLetterOutput>> Add([FromBody]AddStationLetterInput input, CancellationToken cancelToken)
        {
            try
            {
                var token = HttpContext.Current.Request.Headers["Authorization"];
                if (token == null)
                {
                    return new ApiResult<AddStationLetterOutput>(APIResultCode.Unknown, new AddStationLetterOutput { }, APIResultMessage.TokenNull);
                }
                if (string.IsNullOrWhiteSpace(input.Content))
                {
                    throw new NotImplementedException("站内信内容信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.SmallDistrictArray))
                {
                    throw new NotImplementedException("站内信小区范围信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Summary))
                {
                    throw new NotImplementedException("站内信摘要信息为空！");
                }
                if (string.IsNullOrWhiteSpace(input.Title))
                {
                    throw new NotImplementedException("站内信标题信息为空！");
                }
                var user = _tokenManager.GetUser(token);
                if (user == null)
                {
                    return new ApiResult<AddStationLetterOutput>(APIResultCode.Unknown, new AddStationLetterOutput { }, APIResultMessage.TokenError);
                }
                var entity = await _stationLetterRepository.AddAsync(new StationLetterDto
                {
                    Title = input.Title,
                    Summary = input.Summary,
                    SmallDistrictArray = input.SmallDistrictArray,
                    Content = input.Content,
                    StreetOfficeId = user.StreetOfficeId,
                    StreetOfficeName = user.StreetOfficeName,
                    OperationTime = DateTimeOffset.Now,
                    OperationUserId = user.Id.ToString()
                }, cancelToken);

                return new ApiResult<AddStationLetterOutput>(APIResultCode.Success, new AddStationLetterOutput { Id = entity.Id.ToString() });
            }
            catch (Exception e)
            {
                return new ApiResult<AddStationLetterOutput>(APIResultCode.Success_NoB, new AddStationLetterOutput { }, e.Message);
            }
        }

        /// <summary>
        /// 街道办获取站内信列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancelToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("stationLetter/getAllStreetOfficeStationLetter")]
        public async Task<ApiResult<GetAllStreetOfficeStationLetterOutput>> GetAllStreetOfficeStationLetter([FromUri]GetAllStreetOfficeStationLetterInput input, CancellationToken cancelToken)
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
                    return new ApiResult<GetAllStreetOfficeStationLetterOutput>(APIResultCode.Unknown, new GetAllStreetOfficeStationLetterOutput { }, APIResultMessage.TokenNull);
                }

                var user = _tokenManager.GetUser(token);

                if (user == null)
                {
                    return new ApiResult<GetAllStreetOfficeStationLetterOutput>(APIResultCode.Unknown, new GetAllStreetOfficeStationLetterOutput { }, APIResultMessage.TokenError);
                }

                var data = await _stationLetterRepository.GetAllAsync(new StationLetterDto
                {
                    //ReleaseTimeEnd = input.ReleaseTimeEnd,
                    //ReleaseTimeStart = input.ReleaseTimeStart,
                    SmallDistrictArray = input.SmallDistrict
                }, cancelToken);

                return new ApiResult<GetAllStreetOfficeStationLetterOutput>(APIResultCode.Success, new GetAllStreetOfficeStationLetterOutput
                {
                    List = data.Select(x => new GetStreetOfficeStationLetterOutput
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Content = x.Content,
                        StreetOfficeId = x.StreetOfficeId,
                        Summary = x.Summary,
                        StreetOfficeName = x.StreetOfficeName,
                        //Url = _announcementAnnexRepository.GetUrl(x.Id.ToString())
                    }).Skip(startRow).Take(input.PageSize).ToList(),
                    TotalCount = data.Count()
                });
            }
            catch (Exception e)
            {
                return new ApiResult<GetAllStreetOfficeStationLetterOutput>(APIResultCode.Success_NoB, new GetAllStreetOfficeStationLetterOutput { }, e.Message);
            }
        }


    }
}
