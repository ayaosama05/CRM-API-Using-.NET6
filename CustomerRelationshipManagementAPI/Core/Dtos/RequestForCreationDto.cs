using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomerRelationshipManagementAPI.Core.Dtos
{
    public class RequestForCreationDto
    {
        [Required, MinLength(50, ErrorMessage = "Description shouldn't be less than 50 characters")]
        public string Description { get; set; } = String.Empty;
        [Required]
        public byte RequestTypeId { get; set; }
    }
}
