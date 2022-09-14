using ConfigurationService.Models;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationService.Interfaces
{
    public interface ILocationDepartmentState
    {
        //  Task<ResultModel<DepartmentMappingModel>> GetById(TokenModel oTokenModel, long Id);
        //   Task<ResultModel<object>> GetAll(TokenModel oTokenModel, LocationDepartmentStateSearchModel oSearchModel);
        Task<ResultModel<object>> Insert(TokenModel oTokenModel, DepartmentMappingModel oLocationDepartmentStateModel);
        //   Task<ResultModel<object>> Update(TokenModel oTokenModel, DepartmentMappingModel oLocationDepartmentStateModel);
        Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id, bool IsHardDelete = false);
        //  Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationDepartmentStateUid);
        Task<ResultModel<DepartmentMappingModel>> GetMappedStatesByLocationDepartmentUid(TokenModel oTokenModel, string locationDepartmentUid);
    }
}
