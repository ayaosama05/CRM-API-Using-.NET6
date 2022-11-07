namespace CustomerRelationshipManagement.API.Services
{
    public abstract class MessageContract
    {
        public virtual bool IsValid()
        {

            return true;
        }
        public IList<string> Errors = new List<string>();
    }
}
