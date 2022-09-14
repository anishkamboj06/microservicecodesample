using ConfigurationService.Models;
using Models;
using System.Threading.Tasks;

namespace ConfigurationService.Interfaces
{
    public interface ILocationDepartmentStateSetting
    {
        Task<ResultModel<LocationDepartmentStateSettingsModel>> GetLocationDepartmentStateSettingsById(TokenModel oTokenModel, long Id);
        Task<ResultModel<object>> GetAllLocationDepartmentStateSettings(TokenModel oTokenModel, LocationDepartmentStateSettingsSearchModel oSearchModel);
        Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationDepartmentStateSettingsModel oLocationDepartmentStateSettingsModel);
        Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationDepartmentStateSettingsModel oLocationDepartmentStateSettingsModel);
        Task<ResultModel<object>> DeleteLocationDepartmentStateSettings(TokenModel oTokenModel, long Id, bool IsHardDelete = false);
        Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationDepartmentStateSetUid);
    }
}
