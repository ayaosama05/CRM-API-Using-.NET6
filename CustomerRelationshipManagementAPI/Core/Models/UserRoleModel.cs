namespace CustomerRelationshipManagementAPI.Core.Models
{
    public class UserRoleModel
    {
        public string UserName { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public bool IsSucceeded { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
