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
    /// This class used for Get, insert,  update, delete the LocationDepartment
    /// Only Authorised User can access the mehtods of this Api Controller
    /// </summary>
    // [Authorize(Roles = "Admin")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class LocationDepartmentController : BaseController
    {
        #region SetUp
        private readonly ILocationDepartment _srvLocation;
        private readonly ILogger _logger;

        /// <summary>
        /// Location Department Constructor
        /// </summary>
        public LocationDepartmentController(ILocationDepartment srvDepartment, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<LocationDepartmentController>();
            _srvLocation = srvDepartment;
        }
        #endregion

        /// <summary>
        /// This Method Get the Location Department List by Search
        /// Pass SearchModel as Parameter
        /// </summary>
        /// <param name="oSearchModel"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.getAllOPDByPaging")]
        [HttpPost("getAllOPDByPaging")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> getAllOPDByPaging([FromBody] LocationDepartmentSearchModel oSearchModel)
        {
            oSearchModel.Skip = Helper.GetSkipCount(oSearchModel.CurrentPage, oSearchModel.ItemsPerPage);
            oSearchModel.Type = "OPD";
            return await _srvLocation.getAllOPDByPaging(Me, oSearchModel);
        }

        /// <summary>
        /// This method Get the location Department Record by Passing DepartmentId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.GetLocDeptById")]
        [HttpGet("{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<LocationDepartmentModel>> Get([Range(1, int.MaxValue)] int id)
        {
            return await _srvLocation.GetById(Me, id);
        }

       

        /// <summary>
        /// This Method is used to Update the Location department record
        /// </summary>
        /// <param name="oDepartmentModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "CS.UpLocDept")]
        [HttpPut]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Put([FromBody] LocationDepartmentModel oLocationDepartmentModel)
        {
            return await _srvLocation.Update(Me, oLocationDepartmentModel);
        }

   

        /// <summary>
        /// This method Get the location Department Record by Passing DepartmentUId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "CS.GetLocDeptByLocUId")]
        [HttpGet("getLocDeptByLocUID/{id}")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<LocationDepartmentModel>> GetLocDeptByLocationUId(string id)
        {
            return await _srvLocation.GetLocDeptByLocationUId(Me, id);
        }

        /// <summary>
        /// This Method is used to insert and delete list of Location Department records
        /// </summary>
        /// <param name="DepartmentModel">Pass List of LocationDepartment Model as Parameter</param>
        /// <returns></returns>
        [Authorize(Roles = "CS.MultiAddDelLocDept")]
        [HttpPost("insertDelete")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> Post([FromBody] DepartmentInsertDeleteModel oDepartmentInsertDeleteModel)
        {
            ResultModel<object> Result = new ResultModel<object>();

            // Call insert delete service for list
            if (oDepartmentInsertDeleteModel != null)
            {
                // call delete service
                if (oDepartmentInsertDeleteModel.DepartmentIds != null && oDepartmentInsertDeleteModel.DepartmentIds.Count > 0)
                {
                    foreach (var delete in oDepartmentInsertDeleteModel.DepartmentIds)
                    {
                        Result = await _srvLocation.Delete(Me, delete, true);
                        if (Result.MsgCode != 1)// if delete functionality not run then break the loop.
                        {
                            break;
                        }
                    }
                }

                if (Result.MsgCode == 1)
                {
                    // call insert service 
                    if (oDepartmentInsertDeleteModel.Departments != null && oDepartmentInsertDeleteModel.Departments.Count > 0)
                    {
                        foreach (var insert in oDepartmentInsertDeleteModel.Departments)
                        {
                            await _srvLocation.Insert(Me, insert, oDepartmentInsertDeleteModel.LocationName);
                        }
                    }
                    // return message for multiple insert delete
                    Result.Message = Constants.MULTIINSERTDELETE_MESSAGE;
                    Result.MsgCode = Constants.SUCCESS;
                }
            }
            return Result;
        }

        /// <summary>
        /// This Method used to Update the LocationDepartment active status
        /// Pass LocationDepartment Unique Number as Parameter
        /// </summary>
        /// 
        [Authorize(Roles = "CS.LocDeptUpStatus")]
        [HttpPut("updateStatus")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> UpdateStatus(string locationDepartmentUid)
        {
            return await _srvLocation.UpdateStatus(Me, locationDepartmentUid);
        }

     
    }
}
