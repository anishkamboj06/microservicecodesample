using ConfigurationService.Models;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationService.Interfaces
{
    public interface ILocationDepartment
    {
        Task<ResultModel<LocationDepartmentModel>> GetById(TokenModel oTokenModel, long Id);
         Task<ResultModel<object>> getAllOPDByPaging(TokenModel oTokenModel, LocationDepartmentSearchModel oSearchModel);
        Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationDepartmentModel oLocationDepartmentModel,string Location);
        Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationDepartmentModel oLocationDepartmentModel);
        Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id, bool IsHardDelete = false);
        Task<ResultModel<LocationDepartmentModel>> GetLocDeptByLocationUId(TokenModel oTokenModel, string LocUId);
        Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationDepartmentUid);
        Task<string> UnmapLocDeptPrac(TokenModel oTokenModel, string locationDepartmentPractitionerUid);
        //    Task<ResultModel<object>> GetAllUnmappedDeptByLocationUId(TokenModel oTokenModel, string LocationUid);
        //  Task<ResultModel<object>> UnMap(TokenModel oTokenModel, string id);
        // Task<ResultModel<object>> GetLocDeptByLocationUIdWithPagination(TokenModel oTokenModel, LocationDepartmentSearchModel oSearchModel);
    }
}
