using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CustomerRelationshipManagementAPI.Core.Dtos
{
    public class RequestDto
    {
        public int Id { get; set; }

        public string Description { get; set; } = String.Empty;
        public string CustomerName { get; set; } = String.Empty;
        public string CustomerPhone { get; set; } = String.Empty;
        public string RequestType { get; set; } = String.Empty;
    }
}
