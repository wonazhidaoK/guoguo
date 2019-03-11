namespace GuoGuoCommunity.API
{
    /// <summary>
    /// WebAPI接口返回类型
    /// </summary>
    public class ApiResult<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public ApiResult(string code = APIResultCode.Success, T data = default, string message = APIResultMessage.Success)
        {
            this.code = code;
            this.data = data;
            this.message = message;
        }

        /// <summary>
        /// 返回的结果代码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 返回的结果数据
        /// </summary>
        public T data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }

        
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ApiResult(string code = APIResultCode.Success, string message = APIResultMessage.Success)
        {
            this.code = code;
            this.message = message;
        }

        /// <summary>
        /// 返回的结果代码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }

    }

    /// <summary>
    /// API结果码
    /// </summary>
    public struct APIResultCode
    {
        /// <summary>
        /// 接口调用成功，业务成功
        /// </summary>
        public const string Success = "200";
        /// <summary>
        /// 接口调用成功，业务失败
        /// </summary>
        public const string Success_NoB = "500";
        /// <summary>
        /// 异常错误
        /// </summary>
        public const string Error = "404";
        /// <summary>
        /// 未知异常
        /// </summary>
        public const string Unknown = "501";
        /// <summary>
        /// 网络异常
        /// </summary>
        public const string NetworkError = "402";
    }

    /// <summary>
    /// API结果数据包
    /// </summary>
    public struct APIResultMessage
    {
        /// <summary>
        /// 成功
        /// </summary>
        public const string Success = "成功";
        /// <summary>
        /// 无记录
        /// </summary>
        public const string NoRecord = "无记录";
        /// <summary>
        /// 更新成功
        /// </summary>
        public const string UpdateSuccess = "更新成功";
        /// <summary>
        /// 创建成功
        /// </summary>
        public const string CreateSuccess = "创建成功";
        /// <summary>
        /// 删除成功
        /// </summary>
        public const string RemoveSuccess = "删除成功";
        /// <summary>
        /// 未知错误
        /// </summary>
        public const string Unknown = "未知原因";
        /// <summary>
        /// 出现重复记录
        /// </summary>
        public const string AlwaysExist = "出现重复记录";
        /// <summary>
        /// 有关联数据，不能删除
        /// </summary>
        public const string HaveSubData = "有关联数据，不能删除";
        /// <summary>
        /// 主外键关联错误
        /// </summary>
        public const string PKFKError = "主外键关联错误";
        /// <summary>
        /// 日期类型错误
        /// </summary>
        public const string DateFormatError = "日期类型错误";
    }
}