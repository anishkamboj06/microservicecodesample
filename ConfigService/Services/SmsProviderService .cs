using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using ConfigurationService.Utility;
using System.Linq;
using Models;
using CommonLibrary.Utility;

namespace ConfigurationService.Services
{
    public class SmsProviderService: ISmsProvider
    {
        private DBGateway _DBGateway;
        public SmsProviderService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }
        /* It will get master list of all sms providers */
        public async Task<ResultModel<object>> GetAllSMSProviders(TokenModel oTokenModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Result.LstModel = await _DBGateway.ExeQueryList<object>("select Id,ProviderName,IsActive from md_sms_providers", null);
               
            }
            catch (Exception ex)
            {
                throw ex;                
            }
            return Result;
        }

        /*
         * logic  : we deactivate other providers and activate single provider with given request Id
         */
       public async Task<ResultModel<object>> SwitchSmsProvider(TokenModel oTokenModel, long Id)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);  
                var res = await _DBGateway.ExeScalarQuery<int>("update md_sms_providers"
                    + " set IsActive = 1"
                    + " where Id=@Id;" +
                    "  update md_sms_providers "
                    + " set IsActive = 0"
                    + " where Id!=@Id;" 
                    + " select 1 "
                    , Pars);
                if (res == 0)
                {
                    Result.Message = Constants.NOTUPDATED_MESSAGE;
                    Result.MsgCode = Constants.NOTUPDATED;
                }
                else
                    Result.Message = Constants.UPDATED_MESSAGE;
            }
            catch (Exception ex)
            {
                throw ex;               
            }
            return Result;
        }
    }
}