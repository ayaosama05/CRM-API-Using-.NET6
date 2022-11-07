using System.ComponentModel.DataAnnotations;

namespace CustomerRelationshipManagementAPI.Core.Dtos
{
    public class RequestForUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [MinLength(50, ErrorMessage = "Description shouldn't be less than 50 characters")]
        public string Description { get; set; } = String.Empty;
        public byte RequestTypeId { get; set; }
    }
}
