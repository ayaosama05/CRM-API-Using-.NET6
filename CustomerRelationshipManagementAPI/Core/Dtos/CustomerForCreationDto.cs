using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace CustomerRelationshipManagementAPI.Core.Dtos
{
    public class CustomerForCreationDto
    {
        [Required(ErrorMessage = "First Name is required !"), MaxLength(100, ErrorMessage = "It shouldn't exceed 100 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Phone Number is Required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]{11})$", ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email Address is Required")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public DateTime? Birthdate { get; set; }
    }
}
