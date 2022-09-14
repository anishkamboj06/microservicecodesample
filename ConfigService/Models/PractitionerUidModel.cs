using Newtonsoft.Json;
using System;

namespace ConfigurationService.Models
{
    public class PractitionerUidModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "PractitionerUid")]
        public string PractitionerUid { get; set; }
        public string LastName { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public PractitionerUidModel()
        {
     //       Id = Guid.NewGuid().ToString();
          PractitionerUid = Guid.NewGuid().ToString();
        }
    }
}
