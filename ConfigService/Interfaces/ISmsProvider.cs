using System.Threading.Tasks;
using ConfigurationService.Models;
using Models;

namespace ConfigurationService.Interfaces
{
    public interface ISmsProvider
    {
        
        Task<ResultModel<object>> GetAllSMSProviders(TokenModel oTokenModel); 
        Task<ResultModel<object>> SwitchSmsProvider(TokenModel oTokenModel, long Id);
     
    }
}
