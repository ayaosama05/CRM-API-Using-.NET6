using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRelationshipManagementAPI.Core.Models
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [Required, MinLength(50,ErrorMessage ="Description shouldn't be less than 50 characters")]
        public string Description { get; set; } = String.Empty;
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = new Customer();

        public byte RequestTypeId { get; set; }
        [ForeignKey("RequestTypeId")]
        public RequestType RequestType { get; set; } = new RequestType();
    }
}
