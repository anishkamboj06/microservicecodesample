using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationService.Models
{
    public class SubOrganizationStateModel
    {
        public long Id { get; set; }
        public int StateCode { get; set; }
        public string StateDisplay { get; set; }
    }
}
