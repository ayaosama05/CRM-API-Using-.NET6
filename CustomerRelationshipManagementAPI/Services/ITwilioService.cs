namespace CustomerRelationshipManagement.API.Services
{
    public interface ITwilioService
    {
        ServiceResponse Send(Message message);
    }
}
