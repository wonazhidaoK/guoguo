namespace GuoGuoCommunity.Domain.Abstractions.Event
{
    public interface IEventHandler<TEvent>
         where TEvent : IEvent
    {
        /// <summary>
        /// 处理程序
        /// </summary>
        /// <param name="evt"></param>
        void Handle(TEvent evt);

    }
}
