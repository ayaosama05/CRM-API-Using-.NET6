namespace CustomerRelationshipManagementAPI.Core.Models
{
    public class RoleModel
    {
        public string RoleName { get; set; }= string.Empty;
        public bool IsSucceeded { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
