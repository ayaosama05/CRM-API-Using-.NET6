namespace CustomerRelationshipManagement.API.Services
{
    public class ServiceResponse
    {
        public bool IsValid { get; set; }
        public IList<string> Errors = new List<string>();
    }
}
