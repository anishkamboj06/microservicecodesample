using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class DepartmentMappingModel
    {
        #region Location Department State Mapping
        public DepartmentMappingModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
        public long Id { get; set; }
        public string LocationDepartmentStateUid { get; set; }
        [Required]
        public string LocationDepartmentUid { get; set; }
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
    }
    public class LocationDepartmentStateInsertDeleteModel
    {
        public List<DepartmentMappingModel> ListLocationDepartmentStateModel { get; set; }
        public List<long> LocationDepartmentStateId { get; set; }
    }

    public class LocationDepartmentStateSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Type { get; set; }
        public string LocationDepartmentUid { get; set; }
    }

    #endregion

    #region "LocationDepartmentStateTimeModel"
    public class LocationDepartmentStateTimeModel
    {
        public List<LocationDepartmentStateTimeRoasterModel> ListLocationDepartmentStateTimeRoasterModel { get; set; }
        public string LocationDepartmentStateUids { get; set; }
        public string LocationDepartmentUids { get; set; }
        public string LocationUids { get; set; }
        public string RosterType { get; set; }

    }
     
    public class LocationDepartmentStateTimeRoasterModel
    {
        public long Id { get; set; }
       // public string LocationDepartmentStateTimeUid { get; set; }
        //public string LocationDepartmentStateUid { get; set; }
        

        public List<RoasterTime> RoasterTime { get; set; }

        [Required]
        public int WeekCode { get; set; }
        [Required]
        public string DayDisplay { get; set; }
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
        public LocationDepartmentStateTimeRoasterModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
    }

    public class RoasterTime
    {
        [Required]
        //[RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string StartTime { get; set; } // format : HH:mm:ss
        [Required]
        //[RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string EndTime { get; set; } // format : HH:mm:ss

    }
    public class LocationDepartmentStateTimeInsertDeleteModel
    {
        public List<LocationDepartmentStateTimeModel> ListLocationDepartmentStateTimeModel { get; set; }
        public List<long> LocationDepartmentStateTimeIds { get; set; }         
    }

    public class LocationDepartmentStateTimeSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Type { get; set; }
        //public string LocationDepartmentStateUid { get; set; }
        public string DayDisplay { get; set; }

        public string LocationDepartmentStateUids { get; set; }
        public string LocationDepartmentUids { get; set; }
        public string LocationUids { get; set; }
    }
    #endregion

    #region "LocationDepartmentNonOperationalModel"
    public class LocationDepartmentNonOperationalModel
    {
        public long Id { get; set; }
        public string LocationDepartmentNonOpUid { get; set; }
       //// [Required]
       // public string DepartmentCode { get; set; }
       // //[Required]
       // public string DepartmentDisplay { get; set; }
       // public string LocationUid { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }


        [Required]
        //[RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string StartTime { get; set; }
        [Required]
        //[RegularExpression(@"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$", ErrorMessage = "Invalid Time.")]
        public string EndTime { get; set; }


        [Required]
        public string Message { get; set; }
       // public string CreatedByType { get; set; }
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
        public bool IsFullDay { get; set; }
        public LocationDepartmentNonOperationalModel()
        {
            Active = true;
            IsFullDay = false;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
    }
    public class LocationDepartmentNonOperationalInsertDeleteModel
    {
        public List<LocationDepartmentNonOperationalModel> LocationDepartmentNonOperationalModel { get; set; }
        public List<long> LocationDepartmentNonOperationalIds { get; set; }
    }

    public class LocationDepartmentNonOperationalSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        //public string Type { get; set; }
             public string LocationDepartmentStateUids { get; set; }
        public string LocationDepartmentUids { get; set; }
         public string LocationUids { get; set; }
        //public string DepartmentDisplay { get; set; }
        public long? Id { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class LocationDepartmentNonOperationalSearchByDateModel
    {
        [Required]
        public string StartDate { get; set; }
        [Required]
        public string EndDate { get; set; }
        public bool Active { get; set; }
    }
    public class LocationDepartmentNonOperationalModelTimeOff
    {
        public LocationDepartmentNonOperationalModel locationDepartmentNonOperationalModel { get; set; }
        public string LocationDepartmentStateUids { get; set; }
        public string LocationDepartmentUids { get; set; }
        public string LocationUids { get; set; }
    }
    #endregion

    #region "LocationDepartmentStateSettingsModel"
    public class LocationDepartmentStateSettingsModel
    {
        public long Id { get; set; }
        public string LocationDepartmentStateSetUid { get; set; }
        [Required]
        public string LocationDepartmentStateUid { get; set; }
        public bool TokenAllowed { get; set; }
        public int TokenLimit { get; set; }
        public int IncreaseTokenLimit { get; set; }
        [Required]
        public string Message { get; set; }
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
        public LocationDepartmentStateSettingsModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
    }
    public class LocationDepartmentStateSettingsInsertDeleteModel
    {
        public List<LocationDepartmentStateSettingsModel> ListLocationDepartmentStateSettingsModel { get; set; }
        public List<long> LocationDepartmentStateSettingsIds { get; set; }
    }

    public class LocationDepartmentStateSettingsSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
    #endregion
}
