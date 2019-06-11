using Senparc.NeuChar.Context;
using Senparc.NeuChar.Entities;

namespace GuoGuoCommunity.API
{
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

            WXCustomMessageContext messageContext = e.MessageContext as WXCustomMessageContext;
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