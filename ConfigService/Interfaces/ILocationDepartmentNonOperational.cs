using ConfigurationService.Models;
using Models;
using System.Threading.Tasks;

namespace ConfigurationService.Interfaces
{
    public interface ILocationDepartmentNonOperational
    {
        Task<ResultModel<object>> getAllByDate(TokenModel oTokenModel, LocationDepartmentNonOperationalSearchByDateModel oSearchModel);
        Task<ResultModel<object>> GetAllLocationDepartmentNonOperational(TokenModel oTokenModel, LocationDepartmentNonOperationalSearchModel oSearchModel);
        Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationDepartmentNonOperationalModelTimeOff oLocationDepartmentNonOperationalModelTimeOff);
        Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationDepartmentNonOperationalModelTimeOff oLocationDepartmentNonOperationalModelTimeOff);
        Task<ResultModel<object>> DeleteLocationDepartmentNonOperational(TokenModel oTokenModel, string LocDepartNonOpUid, bool IsHardDelete = false);
        Task<ResultModel<object>> GetLocationDepartmentNonOperationalById(long id); 
    }
}
