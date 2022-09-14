using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class PractitionerLocationModel
    {
        public PractitionerLocationModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
        public long Id { get; set; }
        public string PractitionerLocationUid { get; set; }
        [Required]
        public string PractitionerUid { get; set; }
        [Required]
        public string LocationUid { get; set; }
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
}
