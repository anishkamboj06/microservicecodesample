using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class SmsProviderModel
    {
        public long Id { get; set; }
        public string SmsProviderUid { get; set; }

        public string ProviderName { get; set; }

        [JsonIgnore]
        public string CreatedBy { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
        [JsonIgnore]
        public string ModifiedBy { get; set; }
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }
      
        public bool IsActive { get; set; }
        public SmsProviderModel()
        {
            IsActive = false;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
    }
 
}