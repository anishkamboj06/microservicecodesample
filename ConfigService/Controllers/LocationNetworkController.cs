using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ConfigurationService.Controllers
{
    /// <summary>
    /// This class used for Get, insert,  update, delete 
    /// Only Authorised User can access the methods of this Api Controller
    /// </summary>
    // [Authorize(Roles = "Admin")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class LocationNetworkController : BaseController
    {
        #region SetUp
        private readonly ILocationNetwork _srvLocationNetwork;
        private readonly ILogger _logger;

        /// <summary>
        /// Location Network Controller Constructor
        /// </summary>
        public LocationNetworkController(ILocationNetwork srvLocationNetwork, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<LocationNetworkController>();
            _srvLocationNetwork = srvLocationNetwork;
        }
        #endregion

       

        /// <summary>
        /// This Method Get the LocationNetwork Record by Passing LocationNetworkId
        /// </summary>
        /// 
        [Authorize(Roles = "CS.LocNetGetById")]
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<LocationNetworkModel>> Get([Range(1, int.MaxValue)] int id)
        {
            return await _srvLocationNetwork.GetById(Me, id);
        }

        /// <summary>
        /// This Method used to Insert the LocationNetwork record
        /// Pass LocationNetworkModel as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.AddLocNet")]
        [HttpPost]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] List<LocationNetworkModel> olstLocationNetworkModel)
        {
            //Set result object
            ResultModel<object> Result = new ResultModel<object>();
            foreach (var locationNetworkModel in olstLocationNetworkModel)
            {
                await _srvLocationNetwork.Insert(Me, locationNetworkModel);
            }
            return Result;
        }

        
        ///// <summary>
        ///// API to return hild Locations By Parent Location UId
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        ///
        [Authorize(Roles = "CS.GetChildLocByParentLocUId")]
        [HttpGet("getChildLocByParentLocUId")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetChildLocByParentLocUId(string id)
        {
            return await _srvLocationNetwork.GetChildLocByParentLocUId(Me, id);
        }

        ///// <summary>
        ///// API to return hild Locations By Parent Location UId
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        ///
        [Authorize(Roles = "CS.GetChildLocByParentLocUId")]
        [HttpGet("getTeleconsultLocations")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> getTeleconsultLocations()
        {
            string id = Me.LocationUId;
            return await _srvLocationNetwork.GetParentLocByChildLocUId(Me, id);
        }

        /// <summary>
        /// This Method is used to Unmap loc networks by loc networkUid
        /// </summary>
        /// <param name="UnMapNetwork"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.getUnMapNetByLocNetUId")]
        [HttpPost("unMapNetwork")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> UnMapNetworkByLocNetworkUId([FromBody] UnMapNetwork oUnMapNetwork)
        {
            ResultModel<object> Result = new ResultModel<object>();

            // Call insert delete service for list
            if (oUnMapNetwork != null)
            {
                // call delete service
                if (oUnMapNetwork.LocNetworkUIds != null && oUnMapNetwork.LocNetworkUIds.Count > 0)
                {
                    foreach (var locNetwork in oUnMapNetwork.LocNetworkUIds)
                    {
                        await _srvLocationNetwork.Delete(Me, locNetwork, true);
                    }
                }
                // return message for multiple insert delete
                Result.Message = Constants.UNMAPPED_SUCCESS;
                Result.MsgCode = Constants.SUCCESS;
            }
            return Result;
        }

        /// <summary>
        /// This Method used to Update the LocationNetwork distance param
        /// Pass LocNetworkDistanceParameterModel as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.UpLocNetDistParam")]
        [HttpPut("updateLocNetworkDistanceParameters")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Put([FromBody] LocNetworkDistanceParameterModel oLocNetworkDistanceParameterModel)
        {
            return await _srvLocationNetwork.UpdateLocNetworkDistanceParam(Me, oLocNetworkDistanceParameterModel);
        }
    }
}