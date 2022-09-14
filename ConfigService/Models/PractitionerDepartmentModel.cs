using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class PractitionerDepartmentModel
    {
        public PractitionerDepartmentModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
        public long Id { get; set; }
        public string PractitionerProviderDepartmentUid { get; set; }
        [Required]
        public string PractitionerProviderUid { get; set; }
        [Required]
        public string LocationDepartmentUid { get; set; }
        
        public string LocationDepartmentName { get; set; }
        [Required]
        public string LocationUid { get; set; }
        [Required]
        public string LocationName { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string FunctionalType { get; set; }
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
    }

    public class DepartmentPractitionerMappingModel
    {
        public string DepartmentPractitionerUid { get; set; }
        public string LocDepartmentUid { get; set; }
        public string LocDepartmentName { get; set; }
        public string LocationUid { get; set; }
        public string LocationName { get; set; }
        public string PractitionerProviderUid { get; set; }
    }
}
