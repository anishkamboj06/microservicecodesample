using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using Models;
using CommonLibrary.Utility;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System;

namespace ConfigurationService.Controllers
{
    /// <summary>
    /// This class used for Get, insert,  update, delete the Organization
    /// Only Authorised User can access the mehtods of this Api Controller
    /// </summary>
   // [Authorize()]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class OrganizationController : BaseController
    {
        #region SetUp
        private readonly IOrganization _srvOrganization;
        private readonly ILogger _logger;

        /// <summary>
        /// Organization Controller Constructor
        /// </summary>
        public OrganizationController(IOrganization srvOrganization, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<OrganizationController>();
            _srvOrganization = srvOrganization;
        }
        #endregion



        /// <summary>
        /// This Method Get the Organization List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        [Authorize(Roles = "CS.GetAllOrgWithPaging")]
        [HttpPost("getAllByPaging")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetAllByPaging([FromBody] OrganizationSearchModel oSearchModel)
        {
            oSearchModel.Skip = Helper.GetSkipCount(oSearchModel.CurrentPage, oSearchModel.ItemsPerPage);
            return await _srvOrganization.GetAllByPaging(Me, oSearchModel);
        }

        /// <summary>
        /// This Method Get the Organization List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.GetAllOrg")]
        [HttpGet("getAllOrganization")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetAllOrganization()
        {
            return await _srvOrganization.GetAllOrganization(Me,false);
        }

        /// <summary>
        /// This Method Get the Organization Record by Passing OrganizationId
        /// </summary>
        /// 
        [Authorize(Roles = "CS.OrgGetById")]
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<OrganizationModel>> Get([Range(1, int.MaxValue)] int id)
        {
            return await _srvOrganization.GetById(Me, id);
        }

        /// <summary>
        /// This Method used to Insert the Organization record
        /// Pass OrganizationModel as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.AddOrg")]
        [HttpPost]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] OrganizationModel oOrganizationModel)
        {
          //  object m = null;
          //  string s = m.ToString();
            return await _srvOrganization.Insert(Me, oOrganizationModel);
        }

        /// <summary>
        /// This Method used to Update the Organization record
        /// Pass OrganizationModel as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.UpOrg")]
        [HttpPut]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Put([FromBody] OrganizationModel oOrganizationModel)
        {
            return await _srvOrganization.Update(Me, oOrganizationModel);
        }

        /// <summary>
        /// This Method Delete the Organization Record by Passing OrganizationId
        /// </summary>
        /// 
        [Authorize(Roles = "CS.DelOrg")]
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Delete(int id)
        {
            return await _srvOrganization.Delete(Me, id);
        }

        /// <summary>
        /// Return Oragnaization Types
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.GetOrgTypes")]
        [HttpPost("getOrganizationTypes")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<string>> GetOrg()
        {
            return await _srvOrganization.GetOrganizationTypes(Me);
        }

        /// <summary>
        /// This Method used to Update the Organization active status
        /// Pass Organization Unique Number as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.OrgUpStatus")]
        [HttpPut("updateStatus")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> UpdateStatus(string organizationUid)
        {
            return await _srvOrganization.UpdateStatus(Me, organizationUid);
        }

        /// <summary>
        /// This Method Get the Active Organization List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.GetActOrg")]
        [HttpGet("getActiveOrganization")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetActiveOrganization()
        {
            return await _srvOrganization.GetAllOrganization(Me,true);
        }
    }
}
