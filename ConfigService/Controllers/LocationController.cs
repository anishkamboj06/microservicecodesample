using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CommonLibrary.Utility;
using ConfigurationService.Controllers;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace ConfigurationService.Controller
{
    /// <summary>
    /// This class used for Get, insert,  update, delete the Location
    /// Only Authorised User can access the mehtods of this Api Controller
    /// </summary>
   // [Authorize(Roles = "Admin")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class LocationController : BaseController
    {
        #region SetUp
        private readonly ILocation _srvLocation;
        private readonly ILogger _logger;

        /// <summary>
        /// Location Controller Comstructor
        /// </summary>
        public LocationController(ILocation srvLocation, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<LocationController>();
            _srvLocation = srvLocation;
        }
        #endregion

        /// <summary>
        /// This Method Get the Location List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        [Authorize(Roles = "CS.LocGetAllByPaging")]
        [HttpPost("getAllHWCByPaging")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<LocationModel>> GetAllByPaging([FromBody] LocationSearchModel oSearchModel)
        {
            oSearchModel.Skip = Helper.GetSkipCount(oSearchModel.CurrentPage, oSearchModel.ItemsPerPage);
            return await _srvLocation.GetAll(Me, oSearchModel);
        }

        /// <summary>
        /// This Method Get the Location Record by Passing LocationId
        /// </summary>
        /// 
        [Authorize(Roles = "CS.LocGetById")]
        [HttpGet]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Get(string LocationUId)
        {
            return await _srvLocation.GetById(Me, LocationUId);
        }

        /// <summary>
        /// This Method used to Insert the Location record
        /// Pass LocationModel as Parameter
        /// </summary>
        [Authorize(Roles = "CS.AddLoc")]
        [HttpPost]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] LocationModel oLocationModel)
        {
            return await _srvLocation.Insert(Me, oLocationModel);
        }

        /// <summary>
        /// This Method used to Update the Location record
        /// Pass LocationModel as Parameter
        /// </summary>
        [Authorize(Roles = "CS.UpLoc")]
        [HttpPut]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Put([FromBody] LocationModel oLocationModel)
        {
            return await _srvLocation.Update(Me, oLocationModel);
        }

        /// <summary>
        /// This Method Delete the Location Record by Passing LocationId
        /// </summary>
        [Authorize(Roles = "CS.DelLoc")]
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Delete([Range(1, int.MaxValue)] int id)
        {
            return await _srvLocation.Delete(Me, id);
        }

        /// <summary>
        /// This Method used to Update the Location active status
        /// Pass Location Unique Number as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.UpLocStatus")]
        [HttpPut("updateStatus")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> UpdateStatus(string locationUid)
        {
            return await _srvLocation.UpdateStatus(Me, locationUid);
        }

        /// <summary>
        /// Api to get locations by Org id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CS.getLocByOrgId")]
        [HttpGet("getLocationsByParentOrgId")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetLocationsByParentOrgId(string id)
        {
            return await _srvLocation.GetLocationsByParentOrgId(Me, id);
        }

        /// <summary>
        /// Api to return mapped location on the basis of OrgUId and Typeocde
        /// </summary>
        /// <param name=""></param>
        /// <param name="oLocationTypeOrganizationMapping"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.GetLocByOrgIdAndType")]
        [HttpPost("getLocationsByParentOrgIdAndType")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetLocationsByParentOrgIdAndType([FromBody] LocationTypeOrganizationMapping oLocationTypeOrganizationMapping)
        {
            return await _srvLocation.GetLocationsByParentOrgIdAndType(Me, oLocationTypeOrganizationMapping);
        }

        /// <summary>
        /// This Method returns the total count of location
        /// </summary>
        /// 
        [Authorize(Roles = "CS.GetAllActiveLoc")]
        [HttpGet("getActiveTotalLocations")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<int>> GetActiveTotalLocations()
        {
            return await _srvLocation.GetActiveTotalLocations(Me);
        }

        /// <summary>
        /// This Method returns the total count of location
        /// </summary>
        /// 
        [Authorize(Roles = "CS.GetActiveLoc")]
        [HttpGet("getActiveLocations")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetActiveLocations()
        {
            return await _srvLocation.GetActiveLocations(Me);
        }

        /// <summary>
        /// This Method returns All locations of OPD type
        /// </summary>
        /// 
        [Authorize(Roles = "CS.GetActiveLoc")]
        [HttpGet("getActiveLocationsOpds")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetAllActiveOpds()
        {
            return await _srvLocation.GetActiveLocationsOpds(Me);
        }

        /// <summary>
        /// This Method Get the Location List by Search Parameter like OrgUId,HCFType,StateCode,DistrictCode,CityCode
        /// Pass SearchModel as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.GetLocBySearchParam")]
        [HttpPost("getLocationsBySearchParameters")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<LocNetworkDistanceResponseModel>> GetLocationsBySearchParameters([FromBody] LocationSearchModelByOrgUId oLocationSearchModelByOrgUId)
        {
            oLocationSearchModelByOrgUId.Skip = Helper.GetSkipCount(oLocationSearchModelByOrgUId.CurrentPage, oLocationSearchModelByOrgUId.ItemsPerPage);
            return await _srvLocation.GetLocationsBySearchParameters(Me, oLocationSearchModelByOrgUId);
        }
        //Test is added
    }
}