using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.Threading.Tasks;

namespace ConfigurationService.Controllers
{
    /// <summary>
    /// This class used for Get, insert,  update, delete 
    /// Only Authorised User can access the methods of this Api Controller
    /// </summary>
    // [Authorize(Roles = "Admin")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LocationDepartmentStateController : BaseController
    {
        #region SetUp
        private readonly ILocationDepartmentState _srvLocationDepartmentState;
        private readonly ILogger _logger;
        /// <summary>
        /// Location Department State Constructor
        /// </summary>
        public LocationDepartmentStateController(ILocationDepartmentState srvLocationDepartmentState, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<LocationDepartmentStateController>();
            _srvLocationDepartmentState = srvLocationDepartmentState;
        }
        #endregion

     

        /// <summary>
        /// This Method is used to insert and delete list of location department state records
        /// </summary>
        /// <param name="oLocationDepartmentStateModel">Pass List of LocationDepartmentStateModel as Parameter</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.MultiAddDelLocDeptState")]
        [HttpPost("insertDelete")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] LocationDepartmentStateInsertDeleteModel oLocationDepartmentStateInsertDeleteModel)
        {
            ResultModel<object> Result = new ResultModel<object>();

            // Call insert delete service for list
            if (oLocationDepartmentStateInsertDeleteModel != null)
            {
                // call delete service
                if (oLocationDepartmentStateInsertDeleteModel.LocationDepartmentStateId != null && oLocationDepartmentStateInsertDeleteModel.LocationDepartmentStateId.Count > 0)
                {
                    foreach (var delete in oLocationDepartmentStateInsertDeleteModel.LocationDepartmentStateId)
                    {
                        await _srvLocationDepartmentState.Delete(Me, delete, true);
                    }
                }

                // call insert service 
                if (oLocationDepartmentStateInsertDeleteModel.ListLocationDepartmentStateModel != null && oLocationDepartmentStateInsertDeleteModel.ListLocationDepartmentStateModel.Count > 0)
                {
                    foreach (var insert in oLocationDepartmentStateInsertDeleteModel.ListLocationDepartmentStateModel)
                    {
                        await _srvLocationDepartmentState.Insert(Me, insert);
                    }
                }
                // return message for multiple insert delete
                Result.Message = Constants.MULTIINSERTDELETE_MESSAGE;
                Result.MsgCode = Constants.SUCCESS;
            }
            return Result;
        }

      
        /// <summary>
        /// This method use to get the State on the basis of LocationDepartementUId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.getMapStatesByLocDeptUid")]
        [HttpGet("getMappedStatesByLocationDepartmentUid/{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<DepartmentMappingModel>> GetStateListByLocationDepartmentUid(string id)
        {
            return await _srvLocationDepartmentState.GetMappedStatesByLocationDepartmentUid(Me, id);
        }
      
    }
}
