using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class OrganizationStateModel
    {
        public long Id { get; set; }
        //[Required]
        public string OrganizationStateUid { get; set; }
        public string OrganizationUid { get; set; }       
        public string OrganizationDisplay { get; set; }      
        public int StateCode { get; set; }
        public string StateDisplay { get; set; }
        public string CreatedByType { get; set; }
        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }
        public int Source { get; set; }
        public bool Active { get; set; }
        public OrganizationStateModel()
        {
            Active = true;
            CreatedDate = ModifiedDate =  DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
    }


    public class OrganizationStateInsertDeleteModel
    {
        public List<OrganizationStateModel> ListOrganizationStateModel { get; set; }
        public List<long> OrganizationStateId { get; set; }
    }

    public class OrganizationStateSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Type { get; set; }
        public string OrganizationDisplay { get; set; }
        public string OrganizationStateUid { get; set; }
    }
}
