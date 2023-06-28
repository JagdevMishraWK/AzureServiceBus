namespace ASBQueueSender.Service
{
    public interface IQueueSenderService<T> where T : class
    {
        Task<string> Send(T command, string correlationId, Action<IDictionary<string, object>>? setAppProperties = null);
    }
}
