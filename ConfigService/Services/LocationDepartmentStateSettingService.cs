using CommonLibrary.Utility;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using ConfigurationService.Utility;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationService.Services
{
    public class LocationDepartmentStateSettingService : ILocationDepartmentStateSetting
    {
        #region "SetUp"
        private DBGateway _DBGateway;
        public LocationDepartmentStateSettingService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }
        #endregion

        public async Task<ResultModel<LocationDepartmentStateSettingsModel>> GetLocationDepartmentStateSettingsById(TokenModel oTokenModel, long Id)
        {
            ResultModel<LocationDepartmentStateSettingsModel> Result = new ResultModel<LocationDepartmentStateSettingsModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);
                Result.Model = await _DBGateway.ExeScalarQuery<LocationDepartmentStateSettingsModel>("Select  * from mp_location_department_state_settings Where Id=@Id", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> GetAllLocationDepartmentStateSettings(TokenModel oTokenModel, LocationDepartmentStateSettingsSearchModel oSearchModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Skip", oSearchModel.Skip);
                Pars.Add("@Take", oSearchModel.ItemsPerPage);
                Pars.Add("@Type", oSearchModel.Type);
                Pars.Add("@Message", oSearchModel.Message);
                Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_department_state_settings_getall", Pars);
                if (Result.LstModel!=null && Result.LstModel.Count>0)
                {
                    var data = ((IDictionary<string, object>)Result.LstModel.FirstOrDefault());
                    Result.TotalRecords = data != null && data["TotalRecords"] != null ? Convert.ToInt32(data["TotalRecords"]) : 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationDepartmentStateSettingsModel oLocationDepartmentStateSettingsModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();

                Pars.Add("@LocationDepartmentStateSetUid", Helper.GenerateUniqueNumber());
                Pars.Add("@LocationDepartmentStateUid", oLocationDepartmentStateSettingsModel.LocationDepartmentStateUid);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select * from mp_location_department_state_settings Where LocationDepartmentStateUid=@LocationDepartmentStateUid", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }

                Pars.Add("@Id", oLocationDepartmentStateSettingsModel.Id);
                Pars.Add("@TokenAllowed", oLocationDepartmentStateSettingsModel.TokenAllowed);
                Pars.Add("@TokenLimit", oLocationDepartmentStateSettingsModel.TokenLimit);
                Pars.Add("@IncreaseTokenLimit", oLocationDepartmentStateSettingsModel.IncreaseTokenLimit);
                Pars.Add("@Message", oLocationDepartmentStateSettingsModel.Message);
                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@CreatedBy", oTokenModel.LoginId);
                Pars.Add("@CreatedDate", oLocationDepartmentStateSettingsModel.CreatedDate);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", oLocationDepartmentStateSettingsModel.CreatedDate);
                Pars.Add("@Source", oLocationDepartmentStateSettingsModel.Source);
                Pars.Add("@Active", oLocationDepartmentStateSettingsModel.Active);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_location_department_state_settings "
                + " (Id,LocationDepartmentStateSetUid,LocationDepartmentStateUid,TokenAllowed,TokenLimit,IncreaseTokenLimit,Message, "
                + "CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active) values "

                + "(@Id,@LocationDepartmentStateSetUid,@LocationDepartmentStateUid,@TokenAllowed,@TokenLimit,@IncreaseTokenLimit,@Message, "
                + "@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Source,@Active);"
                + "select 1"
                , Pars);

                if (res == 0)
                {
                    Result.Message = Constants.NOTCREATED_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                }
                else
                {
                    Result.Message = Constants.CREATED_MESSAGE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationDepartmentStateSettingsModel oLocationDepartmentStateSettingsModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", oLocationDepartmentStateSettingsModel.Id);
                Pars.Add("@LocationDepartmentStateSetUid", oLocationDepartmentStateSettingsModel.LocationDepartmentStateSetUid);
                Pars.Add("@LocationDepartmentStateUid", oLocationDepartmentStateSettingsModel.LocationDepartmentStateUid);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select  * from mp_location_department_state_settings Where LocationDepartmentStateSettingsUid=@LocationDepartmentStateSettingsUid and Id<>@Id", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }

                Pars.Add("@TokenAllowed", oLocationDepartmentStateSettingsModel.TokenAllowed);
                Pars.Add("@TokenLimit", oLocationDepartmentStateSettingsModel.TokenLimit);
                Pars.Add("@IncreaseTokenLimit", oLocationDepartmentStateSettingsModel.IncreaseTokenLimit);
                Pars.Add("@Message", oLocationDepartmentStateSettingsModel.Message);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", DateTime.Now);
                Pars.Add("@Source", oLocationDepartmentStateSettingsModel.Source);
                Pars.Add("@Active", oLocationDepartmentStateSettingsModel.Active);

                var res = await _DBGateway.ExeScalarQuery<int>("update mp_location_department_state_settings "
                + "set LocationDepartmentStateSetUid=@LocationDepartmentStateSetUid,LocationDepartmentStateUid=@LocationDepartmentStateUid,TokenAllowed=@TokenAllowed"
                + ",TokenLimit=@TokenLimit,IncreaseTokenLimit=@IncreaseTokenLimit,Message=@Message"
                + ",ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source,Active=@Active"
                + " where Id=@Id;"
                + "select 1"
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

        public async Task<ResultModel<object>> DeleteLocationDepartmentStateSettings(TokenModel oTokenModel, long Id, bool IsHardDelete = false)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);
                {
                    Pars.Add("@CreatedById", oTokenModel.LoginId);
                    // dynamic query for hard delete and soft delete, it depends on the para.
                    string query = IsHardDelete ? "delete from mp_location_department_state_settings where Id=@Id" :
                        "update mp_location_department_state_settings set Active=false Where Id=@Id";

                    Result.MsgCode = await _DBGateway.ExeQuery(query, Pars) == 1 ? 1 : 2;

                    if (Result.MsgCode == 1)
                        Result.Message = Constants.DELETE_MESSAGE;
                    else
                    {
                        Result.MsgCode = Constants.NOTDELETED;
                        Result.Message = Constants.NOTDELETE_MESSAGE;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationDepartmentStateSetUid)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@LocationDepartmentStateSetUid", LocationDepartmentStateSetUid);
                //get active status value from table
                bool active = await _DBGateway.ExeScalarQuery<bool>("Select Active from mp_location_department_state_settings Where LocationDepartmentStateSetUid=@LocationDepartmentStateSetUid", Pars);

                // if active status is true then make it false, else vice versa.
                Pars.Add("@Active", !active);

                var res = await _DBGateway.ExeQuery("update mp_location_department_state_settings "
                + "set Active=@Active "
                + "where LocationDepartmentStateSetUid=@LocationDepartmentStateSetUid;"
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