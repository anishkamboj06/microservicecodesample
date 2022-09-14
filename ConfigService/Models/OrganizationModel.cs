using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class OrganizationModel
    {
        [JsonPropertyName("Id")]
        public long Id { get; set; }
        //[Required]
        [JsonPropertyName("OrganizationUid")]
        public string OrganizationUid { get; set; }
        [JsonPropertyName("OrganizationDisplay")]
        [Required(ErrorMessage = "The Name field is required.")]
        public string OrganizationDisplay { get; set; }
        [JsonPropertyName("OrganizationType")]
        [Required]
        public string OrganizationType { get; set; }
        [JsonPropertyName("OrganizationDefinition")]
        public string OrganizationDefinition { get; set; }
        [JsonPropertyName("GovernedBy")]
        public string GovernedBy { get; set; }
        [JsonPropertyName("LstOrganizationCodeMappingModel")]
        public List<OrganizationCodeMappingModel> LstOrganizationCodeMappingModel { get; set; } //TBD with Sumandeep comment this as of now
        [Required]
        [JsonPropertyName("CreatedByType")]
        public string CreatedByType { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }
        [JsonPropertyName("Source")]
        public int Source { get; set; }
        [JsonPropertyName("Active")]
        public bool Active { get; set; }
        [JsonIgnore]
        public string lstOrganizationCode { get; set; }
        public OrganizationModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
    }

    public class OrganizationSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        //public string Type { get; set; }
        public string OrganizationType { get; set; }
        //public string OrganizationDisplay { get; set; }
        public bool? Active { get; set; }
        //[MinLength(3)]
        public string SearchValue { get; set; }
    }

    public class OrganizationCodeMappingModel
    {
        [JsonPropertyName("CodeId")]
        public int CodeId { get; set; }
        [JsonPropertyName("Code")]
        public string Code { get; set; }
        [JsonPropertyName("CodeDisplay")]
        public string CodeDisplay { get; set; }
        [JsonPropertyName("CodeValue")]
        public string CodeValue { get; set; }
    }
}
