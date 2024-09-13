namespace TasteTrailOwnerExperience.Core.Common.MessageBroker;

public interface IMessageBrokerService
{
    public Task PushAsync<T>(string destination, T obj);
}
