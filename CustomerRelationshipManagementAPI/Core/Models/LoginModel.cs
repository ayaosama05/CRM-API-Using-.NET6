using System.ComponentModel.DataAnnotations;

namespace CustomerRelationshipManagementAPI.Core.Models
{
    public class LoginModel
    {
        [Required, MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
