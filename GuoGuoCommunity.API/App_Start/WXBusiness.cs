using GuoGuoCommunity.API.Controllers;
using Senparc.NeuChar.Context;
using Senparc.NeuChar.Entities;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.IO;

namespace GuoGuoCommunity.API
{
    /// <summary>
    /// 自定义消息处理
    /// </summary>
    public class WXCustomMessageHandler : MessageHandler<WXCustomMessageContext>
    {
        

      

        /// <summary>
        /// 构造子
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="postModel"></param>
        /// <param name="maxRecordCount"></param>
        public WXCustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount)
            : base(inputStream, postModel, maxRecordCount)
        {

        }

        //{"action_name": "QR_LIMIT_STR_SCENE", "action_info": {"scene": {"scene_str": "AnZhi_Pub&dimdicien2dddvbnd23rfdcjkw8129&1"}}}

        #region 模板消息

        /// <summary>
        /// 默认响应
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            return CreateResponseMessage<ResponseMessageNoResponse>();
        }

        /// <summary>
        /// 事件之发送模板消息返回结果
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_TemplateSendJobFinishRequest(RequestMessageEvent_TemplateSendJobFinish requestMessage)
        {
            switch (requestMessage.Status)
            {
                case "success":
                    //发送成功

                    break;
                case "failed:user block":
                    //送达由于用户拒收（用户设置拒绝接收公众号消息）而失败
                    break;
                case "failed: system failed":
                    //送达由于其他原因失败
                    break;
                default:
                    throw new WeixinException("未知模板消息状态：" + requestMessage.Status);
            }

            //注意：此方法内不能再发送模板消息，否则会造成无限循环！

            try
            {
                //                var msg = @"已向您发送模板消息
                //状态：{0}
                //MsgId：{1}
                //（这是一条来自MessageHandler的客服消息）".FormatWith(requestMessage.Status, requestMessage.MsgID);
                //                CustomApi.SendText(_appID, WeixinOpenId, msg);//发送客服消息
            }
            catch (Exception e)
            {
                Senparc.Weixin.WeixinTrace.SendCustomLog("模板消息发送失败", e.ToString());
            }


            //无需回复文字内容 return null;
            return null;
        }

        #endregion

        #region 扫码进入公众号

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            if (!string.IsNullOrEmpty(requestMessage.EventKey))
            {
                
                string[] qr_para = requestMessage.EventKey.Split('&');
                string shopCode = string.Empty;

                if (qr_para != null && qr_para.Length >= 2)
                {
                    //4S店的识别号
                    shopCode = qr_para[1];

                    #region 消费者扫码
                    if (qr_para[0].Contains("AnZhi_Pub"))
                    {
                        registerCustomer(qr_para, shopCode, true);
                    }
                    #endregion

                    #region 员工扫码

                    if (qr_para[0].Contains("AnZhi_EmployeeRegQR"))
                    {
                        registerRemind(qr_para);
                    }

                    #endregion
                }
                
            }
            return null;
        }

        /// <summary>
        /// 通过二维码扫描进入公众号事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            if (!string.IsNullOrEmpty(requestMessage.EventKey))
            {                
                string[] qr_para = requestMessage.EventKey.Split('&');
                string shopCode = string.Empty;

                if (qr_para != null && qr_para.Length >= 2)
                {
                    //4S店的识别号
                    shopCode = qr_para[1];

                    #region 消费者扫码
                    if (qr_para[0].Contains("AnZhi_Pub"))
                    {
                        registerCustomer(qr_para, shopCode, false);
                    }
                    
                    #endregion

                    #region 员工扫码

                    if (qr_para[0].Contains("AnZhi_EmployeeRegQR"))
                    {
                        registerRemind(qr_para);
                    }

                    #endregion
                }

                return null;
            }
            else
            {
                #region 模拟码，平台店
                string[] qr_para = "AnZhi_Pub&SHOPCODE20181016185200001&1&1".Split('&');
                registerCustomer(qr_para, "SHOPCODE20181016185200001", true);
                return null;
                #endregion
            }
        }

        #endregion

        #region 取关公众号
        /// <summary>
        /// 退订事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            

            return null;
        }
        #endregion

        #region 事件处理

        /// <summary>
        /// 文字请求的处理
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            return null;
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            //菜单点击，需要跟创建菜单时的Key匹配

            var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();

            switch (requestMessage.EventKey)
            {
                case "ContactMe":
                    strongResponseMessage.Content = "测试数据";
                    break;
                default:
                    break;
            }

            reponseMessage = strongResponseMessage;
            return reponseMessage;
        }

        /// <summary>
        /// 弹出地理位置选择器后的处理（location_select）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "发送您的地址后，请留下您的手机号以方便我们的工作人员与您取得联系~";
            return responseMessage;
        }

        #endregion

        /// <summary>
        /// 消费者扫码注册
        /// </summary>
        /// <param name="qr_para"></param>
        /// <param name="shopCode"></param>
        /// <param name="isSubscribe">是否是关注事件</param>
        private void registerCustomer(string[] qr_para, string shopCode, bool isSubscribe)
        {
            int employeeID = Convert.ToInt32(qr_para[2]);
            int departmentID = Convert.ToInt32(qr_para[3]);

            //TODO 业务逻辑

            //模板消息
            WXController.SendEmployeeRegisterRemind(1, OpenId,"sss");
            
        }

        

        /// <summary>
        /// 员工扫码加入公众号
        /// </summary>
        /// <param name="qr_para"></param>
        /// <param name="shopCode"></param>
        private void registerRemind(string[] qr_para)
        {
            //获取用户信息
            var userInfo = UserApi.Info(WXController.AppId, OpenId);

            int shopID = Convert.ToInt32(qr_para[1]);

            //TODO 开发者业务逻辑


        }
    }

    #region 自定义上下文
    /// <summary>
    /// 自定义上下文
    /// </summary>
    public class WXCustomMessageContext : MessageContext<IRequestMessageBase, IResponseMessageBase>
    {
        /// <summary>
        /// 构造子
        /// </summary>
        public WXCustomMessageContext()
        {
            base.MessageContextRemoved += CustomMessageContext_MessageContextRemoved;
        }

        /// <summary>
        /// 当上下文过期，被移除时触发的时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CustomMessageContext_MessageContextRemoved(object sender, WeixinContextRemovedEventArgs<IRequestMessageBase, IResponseMessageBase> e)
        {
            /* 注意，这个事件不是实时触发的（当然你也可以专门写一个线程监控）
             * 为了提高效率，根据WeixinContext中的算法，这里的过期消息会在过期后下一条请求执行之前被清除
             */

            var messageContext = e.MessageContext as WXCustomMessageContext;
            if (messageContext == null)
            {
                return;//如果是正常的调用，messageContext不会为null
            }

            //TODO:这里根据需要执行消息过期时候的逻辑，下面的代码仅供参考

            //Log.InfoFormat("{0}的消息上下文已过期",e.OpenId);
            //api.SendMessage(e.OpenId, "由于长时间未搭理客服，您的客服状态已退出！");
        }
    }
    #endregion
}