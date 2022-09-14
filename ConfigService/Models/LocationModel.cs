using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class LocationModel
    {
        [JsonPropertyName("Id")]
        public long Id { get; set; }

        //[Required]
        [JsonPropertyName("LocationUid")]
        public string LocationUid { get; set; }

        [Required]
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        [JsonPropertyName("locationCodeMappingModel")]
        public LocationCodeMappingModel locationCodeMappingModel { get; set; } // TBD with Sumandeep this is not use as of now
        [Required]
        [JsonPropertyName("OrganizationUid")]
        public string OrganizationUid { get; set; }
        [Required]
        [JsonPropertyName("TypeCode")]
        public int TypeCode { get; set; }
        [Required]
        [JsonPropertyName("TypeDisplay")]
        public string TypeDisplay { get; set; }

        [JsonPropertyName("Priority")]
        public long Priority { get; set; }
        [JsonPropertyName("ImagePath")]
        public string ImagePath { get; set; }

        [JsonIgnore]
        public string LocationContactDetail { get; set; }

        [JsonPropertyName("LocationContactDetailMappingModel")]
        public List<LocationContactDetailMappingModel> locationContactDetailMappingModel { get; set; }
        [Required]
        [JsonPropertyName("LocationSubTypeCode")]
        public int LocationSubTypeCode { get; set; } //HFi_Type 
        [Required]
        [JsonPropertyName("LocationSubTypeDisplay")]
        public string LocationSubTypeDisplay { get; set; } // phc_chc_type
        [JsonPropertyName("AddressUse")]
        public string AddressUse { get; set; }
        [JsonPropertyName("AddressType")]
        public string AddressType { get; set; }
        [JsonPropertyName("AddressLine1")]
        public string AddressLine1 { get; set; }
        [JsonPropertyName("AddressLine2")]
        public string AddressLine2 { get; set; }
        [Required]
        [JsonPropertyName("CountryCode")]
        public int CountryCode { get; set; }
        [Required]
        [JsonPropertyName("CountryDisplay")]
        public string CountryDisplay { get; set; }
        [Required]
        [JsonPropertyName("StateCode")]
        public int StateCode { get; set; }
        [Required]
        [JsonPropertyName("StateDisplay")]
        public string StateDisplay { get; set; }
        [Required]
        [JsonPropertyName("DistrictCode")]
        public int DistrictCode { get; set; }
        [Required]
        [JsonPropertyName("DistrictDisplay")]
        public string DistrictDisplay { get; set; }
        [Required]
        [JsonPropertyName("CityCode")]
        public int CityCode { get; set; }
        [Required]
        [JsonPropertyName("CityDisplay")]
        public string CityDisplay { get; set; }

        [JsonPropertyName("MandalType")]
        public char? MandalType { get; set; } //
        [JsonPropertyName("MandalCode")]
        public int? MandalCode { get; set; }
        [JsonPropertyName("MandalDisplay")]
        public string MandalDisplay { get; set; }
        [JsonPropertyName("BlockCode")]
        public int? BlockCode { get; set; }
        [JsonPropertyName("BlockDisplay")]
        public string BlockDisplay { get; set; }
        [JsonPropertyName("SecretariatCode")]
        public int? SecretariatCode { get; set; }
        [JsonPropertyName("SecretariatDisplay")]
        public string SecretariatDisplay { get; set; }
        [JsonPropertyName("TalukaCode")]
        public int? TalukaCode { get; set; }
        [JsonPropertyName("TalukaDisplay")]
        public string TalukaDisplay { get; set; }
        [JsonPropertyName("PostalCode")]
        [Required]
        [MaxLength(6)]
        public string PostalCode { get; set; } //set 6 char
        [JsonPropertyName("RegionIndicator")]
        public int? RegionIndicator { get; set; }
        //[Required]
        [JsonPropertyName("CreatedByType")]
        public string CreatedByType { get; set; }
        [JsonIgnore]
        [Required]
        public string CreatedBy { get; set; }
        //[JsonIgnore]
        [JsonPropertyName("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }
        [Required]
        [JsonPropertyName("Source")]
        public int Source { get; set; }
        [JsonIgnore]
        public int TotalRecords { get; set; }
        [JsonPropertyName("Active")]
        public bool Active { get; set; }
        [JsonPropertyName("NIN")]
        public string NIN { get; set; } // nin_to_hfi

        [JsonPropertyName("OrganizationDisplay")]
        public string OrganizationDisplay { get; set; } 
        public List<LocationDepartmentModel> lstDepartmentModel { get; set; }
        public HealthCareFacilityProfile healthCareFacilityProfile { get; set; }
        [JsonPropertyName("NINName")]
        public string NINName { get; set; }
        public LocationModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
            Source = 1;
        }
    }

    public class LocationSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string SearchValue { get; set; }
        public int TypeCode { get; set; }
        public int StateCode { get; set; }
        public int DistrictCode { get; set; }
        public int TalukaCode { get; set; }
        public int CityCode { get; set; }
        public bool? Active { get; set; }
    }

    public class LocationCodeMappingModel
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

    public class LocationContactDetailMappingModel
    {
        [JsonPropertyName("Id")]
        public long Id { get; set; }
        [JsonPropertyName("LocationUid")]
        public string LocationUid { get; set; }
        [JsonPropertyName("ContactPointUse")]
        public string ContactPointUse { get; set; }
        [JsonPropertyName("ContactPointType")]
        //[Required]
        public string ContactPointType { get; set; }
        [JsonPropertyName("ContactPointValue")]
        //[Required]
        public string ContactPointValue { get; set; }
        [JsonPropertyName("ContactPointStatus")]
        public bool ContactPointStatus { get; set; }

        [JsonPropertyName("CreatedByType")]
        public string CreatedByType { get; set; }
        [JsonIgnore]

        public string CreatedBy { get; set; }
        //[JsonIgnore]
        [JsonPropertyName("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }

        [JsonPropertyName("Source")]
        public int Source { get; set; }
        public LocationContactDetailMappingModel()
        {
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
            Source = 1;
        }
    }
    public class LocationTypeOrganizationMapping
    {
        [Required]
        [JsonPropertyName("OrganizationUid")]
        public string OrganizationUid { get; set; }
        [Required]
        [JsonPropertyName("TypeCode")]
        public int TypeCode { get; set; }
        //[JsonPropertyName("TypeDisplay")]
        //public string TypeDisplay { get; set; }
    }
    public class LocationSearchModelByOrgUId
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string SearchValue { get; set; }
        public int StateCode { get; set; }
        public int DistrictCode { get; set; }
        public int CityCode { get; set; }
        [JsonPropertyName("OrganizationUid")]
        public string OrganizationUid { get; set; }
        //[Required]
        [JsonPropertyName("TypeCode")]
        public int TypeCode { get; set; }
    }
    public class HealthCareFacilityProfile
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileExt { get; set; }
    }
    public class GetOrganizationNameTypeByLocUId
    {
        public string OrganizationUid { get; set; }
        public string OrganizationDisplay { get; set; }        
    }
}
