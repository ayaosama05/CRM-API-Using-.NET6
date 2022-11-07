using System.ComponentModel.DataAnnotations;

namespace CustomerRelationshipManagementAPI.Core.Models
{
    public class RegisterModel
    {
        [Required,MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required, MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password")]
        [Required(ErrorMessage = "Confirm Password required")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
