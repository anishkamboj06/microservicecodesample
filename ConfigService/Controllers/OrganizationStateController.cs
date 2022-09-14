using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace ConfigurationService.Controllers
{
    /// <summary>
    /// This class used for Get, insert,  update, delete the Organization State 
    /// Only Authorised User can access the mehtods of this Api Controller
    /// </summary>
    // [Authorize()]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class OrganizationStateController: BaseController
    {
        #region SetUp
        private readonly IOrganizationState _srvOrganizationState;
        private readonly ILogger _logger;

        /// <summary>
        /// Organization State Controller Comstructor
        /// </summary>
        public OrganizationStateController(IOrganizationState srvOrganization, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<OrganizationStateController>();
            _srvOrganizationState = srvOrganization;
        }
        #endregion

     

        /// <summary>
        /// This Method is used to insert and delete list of Organization state records
        /// </summary>
        /// <param name="OrganizationStateModel">Pass List of OrganizationStateModel as Parameter</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.MultiAddDelOrgState")]
        [HttpPost("insertDelete")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] OrganizationStateInsertDeleteModel oOrganizationStateInsertDeleteModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            // Call insert delete service for list
            if (oOrganizationStateInsertDeleteModel != null)
            {
                // call delete service
                if (oOrganizationStateInsertDeleteModel.OrganizationStateId != null && oOrganizationStateInsertDeleteModel.OrganizationStateId.Count > 0)
                {
                    foreach (var delete in oOrganizationStateInsertDeleteModel.OrganizationStateId)
                    {
                        await _srvOrganizationState.Delete(Me, delete, true);
                    }
                }
                // call insert service
                if (oOrganizationStateInsertDeleteModel.ListOrganizationStateModel != null && oOrganizationStateInsertDeleteModel.ListOrganizationStateModel.Count > 0)
                {
                    foreach (var insert in oOrganizationStateInsertDeleteModel.ListOrganizationStateModel)
                    {
                        await _srvOrganizationState.Insert(Me, insert);
                    }
                }
                // return message for multiple insert delete
                Result.Message = Constants.MULTIINSERTDELETE_MESSAGE;
                Result.MsgCode = Constants.SUCCESS;
            }
            return Result;
        }

        /// <summary>
        /// This Method Get the Organization State List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        [Authorize(Roles = "CS.GetOrgStateByOrgId")]
        [HttpPost("getOrganizationStateByOrganizationId/{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<SubOrganizationStateModel>> GetOrganizationStateByOrganizationId(string id)
        {          
            return await _srvOrganizationState.GetOrganizationStateByOrganizationId(id);
        }

    }
}