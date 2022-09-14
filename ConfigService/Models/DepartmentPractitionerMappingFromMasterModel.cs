using System;

namespace ConfigurationService.Models
{
    public class DepartmentPractitionerMappingFromMasterModel
    {
        public string PractitionerUid { get; set; }
        public string Type { get; set; }
        public string FunctionalType { get; set; }
        public string DepartmentPractitionerUid { get; set; }
        public string LocDepartmentUid { get; set; }
        public string LocDepartmentName { get; set; }
        public string LocationUid { get; set; }
        public string LocationName { get; set; }
        public string OrganizationUid { get; set; }
        public string OrganizationName { get; set; }
        public string PractitionerProviderUid { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DepartmentPractitionerMappingFromMasterModel()
        {
            ModifiedDate = DateTime.Now;
        }
    }
}
