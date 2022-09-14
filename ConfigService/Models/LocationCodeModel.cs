﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class LocationCodeModel
    {
        public long Id { get; set; }
        public string LocationUid { get; set; }
        [Required]
        public int CodeId { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string CodeDisplay { get; set; }
        [Required]
        public string CodeValue { get; set; }
        [Required]
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
        public LocationCodeModel()
        {
            Active = true;
            CreatedDate = ModifiedDate = DateTime.Now;
            CreatedBy = ModifiedBy = "1";
        }
    }

    public class LocationCodeSearchModel
    {
        public int Skip { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public string Type { get; set; }
        public string CodeDisplay { get; set; }
    }
}