using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class LocationDepartmentModel
    {
        public LocationDepartmentModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
        public long Id { get; set; }
        public string LocationDepartmentUid { get; set; }

        public string LocationDepartmentName { get; set; }
        public string LocationUid { get; set; }
        //[JsonIgnore]
        public string LocationName { get; set; }
        [Required]
        public int DepartmentCode { get; set; }
        [Required]
        public string DepartmentDisplay { get; set; }
        [Required]
        public string OrganizationUid { get; set; }
        public bool IsSpecial { get; set; }
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
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int StateCode { get; set; }
        public string StateDisplay { get; set; }
        public int DistrictCode { get; set; }
        public string DistrictDisplay { get; set; }
        public int CityCode { get; set; }
        public string CityDisplay { get; set; }
        public string PostalCode { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string FunctionalType { get; set; }
    }
    public class DepartmentInsertDeleteModel
    {
        public string LocationName { get; set; }
        public List<LocationDepartmentModel> Departments { get; set; }
        public List<long> DepartmentIds { get; set; }
    }

    public class LocationDepartmentSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Type { get; set; }
        public string LocationUid { get; set; }
        public string DepartmentDisplay { get; set; }
        public bool? Active { get; set; }
        public string OrganizationType { get; set; }

    }
}
