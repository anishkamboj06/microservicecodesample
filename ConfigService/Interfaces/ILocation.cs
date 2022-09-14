using ConfigurationService.Models;
using Models;
using System.Threading.Tasks;

namespace ConfigurationService.Interfaces
{
    public interface ILocation
    {
        Task<ResultModel<object>> GetById(TokenModel oTokenModel, string LocationUId);
        Task<ResultModel<LocationModel>> GetAll(TokenModel oTokenModel, LocationSearchModel oSearchModel);
        Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationModel oLocationModel);
        Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationModel oLocationModel);
        Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id);
        Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationUid);
        Task<ResultModel<object>> GetLocationsByParentOrgId(TokenModel oTokenModel, string OrgUid);
        Task<ResultModel<object>> GetLocationsByParentOrgIdAndType(TokenModel oTokenModel, LocationTypeOrganizationMapping oLocationTypeOrganizationMapping);
        //Task<int> GetTotalLocations(TokenModel oTokenModel);        
        Task<ResultModel<int>> GetActiveTotalLocations(TokenModel oTokenModel);
        Task<ResultModel<object>> GetActiveLocations(TokenModel oTokenModel);
        Task<ResultModel<object>> GetActiveLocationsOpds(TokenModel oTokenModel);
        Task<ResultModel<LocNetworkDistanceResponseModel>> GetLocationsBySearchParameters(TokenModel oTokenModel, LocationSearchModelByOrgUId oLocationSearchModelByOrgUId);
        Task<ResultModel<object>> GetLocationForConsultation(TokenModel oTokenModel, string LocationUId);
    }
}
