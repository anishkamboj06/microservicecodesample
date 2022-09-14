using System.Threading.Tasks;
using ConfigurationService.Models;
using Models;

namespace ConfigurationService.Interfaces
{
    public interface IOrganizationState
    {
        //Task<ResultModel<OrganizationStateModel>> GetById(TokenModel oTokenModel, long Id);
        //Task<ResultModel<object>> GetAll(TokenModel oTokenModel, OrganizationStateSearchModel oSearchModel);
        Task<ResultModel<object>> Insert(TokenModel oTokenModel, OrganizationStateModel oLocationModel);
        //   Task<ResultModel<object>> Update(TokenModel oTokenModel, OrganizationStateModel oLocationModel);
        Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id, bool IsHardDelete = false);
        Task<ResultModel<SubOrganizationStateModel>> GetOrganizationStateByOrganizationId(string organizationUId);
        //  Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string OrganizationStateUid);
    }
}
