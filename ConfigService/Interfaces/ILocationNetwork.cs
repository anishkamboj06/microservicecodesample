using ConfigurationService.Models;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationService.Interfaces
{
    public interface ILocationNetwork
    {
        Task<ResultModel<LocationNetworkModel>> GetById(TokenModel oTokenModel, long Id);
        //     Task<ResultModel<object>> GetAll(TokenModel oTokenModel, LocationNetworkSearchModel oSearchModel);
        Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationNetworkModel oLocationNetworkModel);
        //    Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationNetworkModel oLocationNetworkModel);
        Task<ResultModel<object>> Delete(TokenModel oTokenModel, string Id, bool IsHardDelete = false);
        //Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationNetworkUid);
        //Task<ResultModel<object>> GetFacilityListByTypeAndOrgUid(TokenModel oTokenModel, LocationNetworkSearchModel oSearchModel);
        //Task<ResultModel<object>> GetFacilityListByLocationUid(TokenModel oTokenModel, LocationNetworkSearchModel oSearchModel);
        //Task<ResultModel<object>> UnMappedfacility(TokenModel oTokenModel, string LocationNetworkUid);
        //Task<ResultModel<object>> SaveFacilityNetwork(TokenModel oTokenModel, List<LocationNetworkModel> oLocationNetworkModel);
        Task<ResultModel<object>> GetChildLocByParentLocUId(TokenModel oTokenModel, string ParentLocUId);
        Task<ResultModel<object>> GetParentLocByChildLocUId(TokenModel oTokenModel, string ParentLocUId);
        Task<ResultModel<object>> UpdateLocNetworkDistanceParam(TokenModel oTokenModel, LocNetworkDistanceParameterModel oLocNetworkDistanceParameterModel);
    }
}
