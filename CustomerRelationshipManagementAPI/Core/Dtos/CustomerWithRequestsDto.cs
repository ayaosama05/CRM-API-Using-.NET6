using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerRelationshipManagementAPI.Core.Dtos
{
    public class CustomerWithRequestsDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime? Birthdate { get; set; }
        [JsonPropertyName("Customer Requests")]
        [JsonProperty("Customer Requests")]
        public ICollection<CustomerRequestDto> CustomerRequestDtos { get; set; } = new List<CustomerRequestDto>();
        public string FullName
        {
            get
            {
                return $"{this.FirstName} {this.LastName}";
            }
        }

        public int CstRequestsCount
        {
            get
            {
                return this.CustomerRequestDtos.Count();
            }
        }
    }
}
