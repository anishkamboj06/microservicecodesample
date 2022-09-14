using CommonLibrary.Utility;
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
    public class LocationDepartmentStateTimeController : BaseController
    {
        #region SetUp
        private readonly ILocationDepartmentStateTime _srvLocationDepartmentStateTime;
        private readonly ILogger _logger;
        /// <summary>
        /// Location Department State Time Constructor
        /// </summary>
        public LocationDepartmentStateTimeController(ILocationDepartmentStateTime srvLocationDepartmentStateTime, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<LocationDepartmentStateTimeController>();
            _srvLocationDepartmentStateTime = srvLocationDepartmentStateTime;
        }
        #endregion

        #region LocationDepartmentStateTime API's

        /// <summary>
        /// This Method Get the Location LocationDepartmentStateTime List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        /// <param name="oSearchModel"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.GetAllLocDeptSTime")]
        [HttpPost("getAll")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetAllLocationDepartmentStateTime([FromBody] LocationDepartmentStateTimeSearchModel oSearchModel)
        {
            oSearchModel.Skip = Helper.GetSkipCount(oSearchModel.CurrentPage, oSearchModel.ItemsPerPage);
            return await _srvLocationDepartmentStateTime.GetAllLocationDepartmentStateTime(Me, oSearchModel);
        }


        /// <summary>
        /// This Method is used to insert multiple Location Department StateTime record / Roaster Insert
        /// </summary>
        /// <param name="oLocationDepartmentStateModel">Pass LocationDepartmentStateTime as Parameter</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.MultiAddLocDeptSTime")]
        [HttpPost("insertAll")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> InsertAll([FromBody] LocationDepartmentStateTimeModel oLocationDepartmentStateTimeModel)
        {
            return await _srvLocationDepartmentStateTime.InsertMultiple(Me, oLocationDepartmentStateTimeModel);
        }

        /// <summary>
        /// This Method is used to update multiple Location Department StateTime record / Roaster Insert
        /// </summary>
        /// <param name="oLocationDepartmentStateModel">Pass LocationDepartmentStateTime as Parameter</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.MultiUpLocDeptSTime")]
        [HttpPut("updateAll")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> UpdateAll([FromBody] LocationDepartmentStateTimeModel oLocationDepartmentStateTimeModel)
        {
            return await _srvLocationDepartmentStateTime.UpdateMultiple(Me, oLocationDepartmentStateTimeModel);
        }

        #endregion
    }
}
