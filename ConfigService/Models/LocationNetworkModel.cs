using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class LocationNetworkModel
    {
        public long Id { get; set; }
        public string LocationNetworkUid { get; set; }
        [Required]
        public string ParentLocationUid { get; set; }
        [Required]
        public string LocationUid { get; set; }
        [Required]
        public string OrganisationUid { get; set; }
        public string Distance { get; set; }
        public string TotalTime { get; set; }
        public string Fare { get; set; }
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
        public string Name { get; set; }
        public LocationNetworkModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }

    }
    public class LocationNetworkSearchModel: SearchModel
    {
        public string LocationNetworkUid { get; set; }
        public string TypeCode { get; set; }
        public string OrganizationUid { get; set; }
        public string ParentLocationUid { get; set; }
    }

    public class UnMapNetwork
    {
        public List<string> LocNetworkUIds { get; set; }
    }
    public class LocNetworkDistanceParameterModel
    {
        public string LocationNetworkUid { get; set; }
        public string Distance { get; set; }
        public string TotalTime { get; set; }
        public string Fare { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }       
        public LocNetworkDistanceParameterModel()
        {
            ModifiedBy = "1";
            ModifiedDate = DateTime.Now;
        }
    }
    public class LocNetworkDistanceResponseModel
    {
        [JsonPropertyName("Distance")]
        public string Distance { get; set; }
        [JsonPropertyName("TotalTime")]
        public string TotalTime { get; set; }
        [JsonPropertyName("Fare")]
        public string Fare { get; set; }
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("LocationNetworkUid")]
        public string LocationNetworkUid { get; set; }
        [JsonPropertyName("ParentLocationUid")]
        public string ParentLocationUid { get; set; }
        [JsonPropertyName("LocationUid")]
        public string LocationUid { get; set; }
        [JsonPropertyName("OrganisationUid")]
        public string OrganisationUid { get; set; }
        [JsonPropertyName("TypeCode")]
        public string TypeCode { get; set; }
        [JsonPropertyName("TypeDisplay")]
        public string TypeDisplay { get; set; }
        [JsonPropertyName("TotalRecords")]
        public int TotalRecords { get; set; } 
    }
}