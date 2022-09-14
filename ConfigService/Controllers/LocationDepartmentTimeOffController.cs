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
    public class LocationDepartmentTimeOffController : BaseController
    {
        #region SetUp
        private readonly ILocationDepartmentNonOperational _srvLocationDepartmentNonOperational;
        private readonly ILogger _logger;
        /// <summary>
        /// Location Department NonOperational Constructor
        /// </summary>
        public LocationDepartmentTimeOffController(ILocationDepartmentNonOperational srvLocationDepartmentNonOperational, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<LocationDepartmentTimeOffController>();
            _srvLocationDepartmentNonOperational = srvLocationDepartmentNonOperational;
        }
        #endregion

        #region LocationDepartmentNonOperational API's

        /// <summary> 
        /// This Method Get the Location Department NonOperational List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        /// <param name="oSearchModel"></param>
        /// <returns></returns>
        /// 
      //  [Authorize(Roles = "CS.GetAllLocDeptNonOp")]
         [HttpPost("getAll")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetAllLocationDepartmentNonOperational([FromBody] LocationDepartmentNonOperationalSearchModel oSearchModel)
        {
            oSearchModel.Skip = Helper.GetSkipCount(oSearchModel.CurrentPage, oSearchModel.ItemsPerPage);
            return await _srvLocationDepartmentNonOperational.GetAllLocationDepartmentNonOperational(Me, oSearchModel);
        }

        /// <summary>
        /// This method Get the LocationDepartmentNonOperational Record by Passing LocationDepartmentNonOperationalId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.LocDeptTimeOffGetAllByDate")]
        [HttpPost("getAllByDate")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> getAllByDate([FromBody] LocationDepartmentNonOperationalSearchByDateModel oSearchModel)
        {
            return await _srvLocationDepartmentNonOperational.getAllByDate(Me, oSearchModel);
        }

        /// <summary>
        /// This Method is used to insert single Location Department NonOperational record
        /// </summary>
        /// <param name="oLocationDepartmentNonOperationalModel">Pass LocationDepartmentNonOperational as Parameter</param>
        /// <returns></returns> 

        [Authorize(Roles = "CS.AddLocDeptNonOpt")]
        [HttpPost("Insert")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] LocationDepartmentNonOperationalModelTimeOff oLocationDepartmentNonOperationalModelTimeOff)
        {
            return await _srvLocationDepartmentNonOperational.Insert(Me, oLocationDepartmentNonOperationalModelTimeOff);
        }

        /// <summary>
        /// This Method is used to Update the Location Department NonOperational record
        /// </summary>
        /// <param name="oLocationDepartmentNonOperationalModel"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.UpLocDeptNonOpt")]
        [HttpPut("Update")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Put([FromBody] LocationDepartmentNonOperationalModelTimeOff oLocationDepartmentNonOperationalModelTimeOff)
        {
            return await _srvLocationDepartmentNonOperational.Update(Me, oLocationDepartmentNonOperationalModelTimeOff);
        }

        /// <summary>
        /// This method is used to multi delete the Location Department StateTime record
        /// </summary>
        /// <param name="LocDepartNonOpUid">Pass LocDepartNonOpUid as Parameter</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.DelLocDeptNonOpt")]
        [HttpDelete("{LocDepartNonOpUid}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> DeleteLocationDepartmentTimeOffByLocDepartNonOpUid(string LocDepartNonOpUid)
        {
            return await _srvLocationDepartmentNonOperational.DeleteLocationDepartmentNonOperational(Me, LocDepartNonOpUid, true);
        }

        /// <summary>
        /// This Method Get the Location Department NonOperational records by Id
        /// Pass SearchModel as Parameter
        /// </summary>
        /// <param name="oSearchModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "CS.getByIdLocDeptNonOpt")]
        [HttpGet("getById")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetLocationDepartmentNonOperationalById(long Id)
        {
            return await _srvLocationDepartmentNonOperational.GetLocationDepartmentNonOperationalById(Id);
        }

        #endregion
    }
}
