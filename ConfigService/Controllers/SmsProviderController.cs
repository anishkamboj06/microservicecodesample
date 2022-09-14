using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ConfigurationService.Interfaces;
using Models;
using Microsoft.AspNetCore.Authorization;

namespace ConfigurationService.Controllers
{
    /// <summary>
    /// This class used for GEt list and activate/Deactivate SMS Providers
    /// Only Authorised User can access the mehtods of this Api Controller
    /// </summary>
   // [Authorize()]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiController]
    public class SmsProviderController : BaseController
    {
        #region SetUp
        private readonly ISmsProvider _srvSmsProvider;
        private readonly ILogger _logger;

        /// <summary>
        /// SmsProvider Controller Constructor
        /// </summary>
        public SmsProviderController(ISmsProvider srvSmsProvider, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<SmsProviderController>();
            _srvSmsProvider = srvSmsProvider;
        }
        #endregion

        /// <summary>
        /// This Method Get the SMS Providers List 
        /// </summary>
        /// 
        [Authorize(Roles = "CS.GetAllSMSProviders")]
        [HttpGet("getAllSMSProviders")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> GetAllSMSProviders()
        { 
            return await _srvSmsProvider.GetAllSMSProviders(Me);
        }

        /// <summary>
        /// This Method used to activate the SMSProvider 
        /// Pass Smsprovider Id as Parameter
        /// </summary>
        /// 

        [Authorize(Roles = "CS.SwitchSmsProvider")]
        [HttpPut("switchSmsProvider")]
        [MapToApiVersion("1")]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ResultModel<object>> SwitchSmsProvider(long Id)
        {
            return await _srvSmsProvider.SwitchSmsProvider(Me, Id);
        }
    }
}
