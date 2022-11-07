namespace CustomerRelationshipManagementAPI.Core.Dtos
{
    public class CustomerRequestDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = String.Empty;
        public string RequestType { get; set; } = String.Empty;
    }
}
