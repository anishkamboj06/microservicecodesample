using Microsoft.Extensions.Logging;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace ConfigurationService.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class PractitionerDepartmentController : BaseController
    {
        #region SetUp
        private readonly IPractitionerDepartment _srvPractitionerDepartment;
        private readonly ILogger _logger;
        private IConfiguration _configuration;
        string PractitionerServiceEndPoint = string.Empty;        

        /// <summary>
        /// Practitioner Department Constructor
        /// </summary>
        public PractitionerDepartmentController(IPractitionerDepartment srvDepartment, ILoggerFactory logFactory, IConfiguration configuration)
        {
            _logger = logFactory.CreateLogger<PractitionerDepartmentController>();
            _srvPractitionerDepartment = srvDepartment;
            _configuration = configuration;
            PractitionerServiceEndPoint = _configuration["PractitionerServiceEndPoints:Url"];            
        }
        #endregion

        /// <summary>
        /// This Method used to Insert the Practitioner Department record in LIST
        /// Pass Practitioner Department Model List as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.AddPracPovLocDeptMap")]
        [HttpPost("insertPractitionerProviderLocationDepartmentMapping")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<DepartmentPractitionerMappingFromMasterModel>> InsertPractitionerProviderLocationDepartmentMapping([FromBody] List<PractitionerDepartmentModel> oPractitionerDepartmentModel)
        {
            return await _srvPractitionerDepartment.InsertPractitionerProviderLocationDepartmentMapping(Me, oPractitionerDepartmentModel, PractitionerServiceEndPoint);
        }

        /// <summary>
        /// This Method used to Update the mp_practitioner_department active status
        /// Pass PractitionerDepartmentUid Unique Number as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.PracProDeptUpStatus")]
        [HttpPut("updateStatus")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> UpdateStatus(string pracProDeptUId)
        {
            return await _srvPractitionerDepartment.UpdateStatus(Me, pracProDeptUId);
        }

        /// <summary>
        /// This Method used to DELETE the mp_location_department_practitioner
        /// Pass PractitionerProviderDepartmentUid as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.DelPracProDeptUid")]
        [HttpDelete("{practitionerProviderDepartmentUid}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Delete(string practitionerProviderDepartmentUid)
        {
            return await _srvPractitionerDepartment.Delete(Me, practitionerProviderDepartmentUid, PractitionerServiceEndPoint);
        }
    }
}
