using CommonLibrary.Utility;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
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
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LocationDepartmentStateSettingController : BaseController
    {
        #region SetUp
        private readonly ILocationDepartmentStateSetting _srvLocationDepartmentStateSetting;
        private readonly ILogger _logger;
        /// <summary>
        /// Location Department State Constructor
        /// </summary>
        public LocationDepartmentStateSettingController(ILocationDepartmentStateSetting srvLocationDepartmentStateSetting, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<LocationDepartmentStateSettingController>();
            _srvLocationDepartmentStateSetting = srvLocationDepartmentStateSetting;
        }
        #endregion

        #region LocationDepartmentStateSettings API's

        /// <summary>
        /// This Method Get the Location LocationDepartmentStateSettings List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        /// <param name="oSearchModel"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.GetAllLocDeptStateSetting")]
        [HttpPost("getAll")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetAllLocationDepartmentStateSettings([FromBody] LocationDepartmentStateSettingsSearchModel oSearchModel)
        {
            oSearchModel.Skip = Helper.GetSkipCount(oSearchModel.CurrentPage, oSearchModel.ItemsPerPage);
            return await _srvLocationDepartmentStateSetting.GetAllLocationDepartmentStateSettings(Me, oSearchModel);
        }

        /// <summary>
        /// This method Get the LocationDepartmentStateSettings Record by Passing LocationDepartmentStateSettingsId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.GetLocDeptStateSettingsById")]
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<LocationDepartmentStateSettingsModel>> GetLocationDepartmentStateSettings([Range(1, int.MaxValue)] int id)
        {
            return await _srvLocationDepartmentStateSetting.GetLocationDepartmentStateSettingsById(Me, id);
        }

        /// <summary>
        /// This Method is used to insert single Location Department StateTime record
        /// </summary>
        /// <param name="oLocationDepartmentStateModel">Pass LocationDepartmentStateSettings as Parameter</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.AddLocDeptStateSetting")]
        [HttpPost]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] LocationDepartmentStateSettingsModel oLocationDepartmentStateSettingsModel)
        {
            return await _srvLocationDepartmentStateSetting.Insert(Me, oLocationDepartmentStateSettingsModel);
        }

        /// <summary>
        /// This Method is used to insert and delete list of Location Department StateTime records
        /// </summary>
        /// <param name="oLocationDepartmentStateModel">Pass List of LocationDepartmentStateSettings as Parameter</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.MultiAddDelLocDeptStateSetting")]
        [HttpPost("insertDelete")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] LocationDepartmentStateSettingsInsertDeleteModel oLocationDepartmentStateSettingsInsertDeleteModel)
        {
            ResultModel<object> Result = new ResultModel<object>();

            // Call insert delete service for list
            if (oLocationDepartmentStateSettingsInsertDeleteModel != null)
            {
                // call delete service
                if (oLocationDepartmentStateSettingsInsertDeleteModel.LocationDepartmentStateSettingsIds != null && oLocationDepartmentStateSettingsInsertDeleteModel.LocationDepartmentStateSettingsIds.Count > 0)
                {
                    foreach (var delete in oLocationDepartmentStateSettingsInsertDeleteModel.LocationDepartmentStateSettingsIds)
                    {
                        await _srvLocationDepartmentStateSetting.DeleteLocationDepartmentStateSettings(Me, delete, true);
                    }
                }

                // call insert service 
                if (oLocationDepartmentStateSettingsInsertDeleteModel.ListLocationDepartmentStateSettingsModel != null && oLocationDepartmentStateSettingsInsertDeleteModel.ListLocationDepartmentStateSettingsModel.Count > 0)
                {
                    foreach (var insert in oLocationDepartmentStateSettingsInsertDeleteModel.ListLocationDepartmentStateSettingsModel)
                    {
                        await _srvLocationDepartmentStateSetting.Insert(Me, insert);
                    }
                }
                // return message for multiple insert delete
                Result.Message = Constants.MULTIINSERTDELETE_MESSAGE;
                Result.MsgCode = Constants.MULTIINSERTDELETE;
            }
            return Result;
        }

        /// <summary>
        /// This Method is used to Update the Location Department StateTime record
        /// </summary>
        /// <param name="oLocationDepartmentStateModel"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.UpLocDeptStateSetting")]
        [HttpPut]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Put([FromBody] LocationDepartmentStateSettingsModel oLocationDepartmentStateSettingsModel)
        {
            return await _srvLocationDepartmentStateSetting.Update(Me, oLocationDepartmentStateSettingsModel);
        }

        /// <summary>
        /// This method is used to delete the Location Department StateTime record
        /// </summary>
        /// <param name="id">Pass LocationDepartmentStateSettingsId as Parameter</param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.DelLocDeptStateSetting")]
        [HttpDelete("{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> DeleteLocationDepartmentStateSettings([Range(1, int.MaxValue)] int id)
        {
            return await _srvLocationDepartmentStateSetting.DeleteLocationDepartmentStateSettings(Me, id);
        }

        /// <summary>
        /// This Method used to Update the LocationDepartmentStateSetting active status
        /// Pass LocationDepartmentStateSetting Unique Number as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.UpStatusLocDeptStateSetting")]
        [HttpPut("updateStatus")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> UpdateStatus(string locationDepartmentStateSetUid)
        {
            return await _srvLocationDepartmentStateSetting.UpdateStatus(Me, locationDepartmentStateSetUid);
        }
        #endregion
    }
}
