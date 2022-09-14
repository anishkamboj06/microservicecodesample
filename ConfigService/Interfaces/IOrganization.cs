using System.Threading.Tasks;
using ConfigurationService.Models;
using Models;

namespace ConfigurationService.Interfaces
{
    public interface IOrganization
    {
        Task<ResultModel<OrganizationModel>> GetById(TokenModel oTokenModel, long Id);
        Task<ResultModel<object>> GetAllByPaging(TokenModel oTokenModel, OrganizationSearchModel oSearchModel);
        Task<ResultModel<object>> GetAllOrganization(TokenModel oTokenModel, bool isActive);
        Task<ResultModel<object>> Insert(TokenModel oTokenModel, OrganizationModel oLocationModel);
        Task<ResultModel<object>> Update(TokenModel oTokenModel, OrganizationModel oLocationModel);
        Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id);
        Task<ResultModel<string>> GetOrganizationTypes(TokenModel oTokenModel);
        Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string OrganizationUid);
        Task<string> GetGovernedByCount(TokenModel oTokenModel, string GovernedBy);
        Task<string> GetOrganizationTypeCount(TokenModel oTokenModel, string OrganizationType);
    }
}
