using ConfigurationService.Models;
using Models;
using System.Threading.Tasks;

namespace ConfigurationService.Interfaces
{
    public interface ILocationDepartmentStateTime
    {
        Task<ResultModel<object>> GetAllLocationDepartmentStateTime(TokenModel oTokenModel, LocationDepartmentStateTimeSearchModel oSearchModel);
        Task<ResultModel<object>> InsertMultiple(TokenModel oTokenModel, LocationDepartmentStateTimeModel oLocationDepartmentStateTimeModel);
        Task<ResultModel<object>> UpdateMultiple(TokenModel oTokenModel, LocationDepartmentStateTimeModel oLocationDepartmentStateTimeModel);
        
    }
}
