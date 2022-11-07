namespace CustomerRelationshipManagementAPI.Core.Models
{
    public class AuthModel
    {
        public string Message { get; set; } = string.Empty; 
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpirationDate { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
