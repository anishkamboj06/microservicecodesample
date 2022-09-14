using CommonLibrary.Utility;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using ConfigurationService.Utility;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationService.Services
{
    public class LocationDepartmentStateService : ILocationDepartmentState
    {
        #region "SetUp"
        private DBGateway _DBGateway;
        public LocationDepartmentStateService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }
        #endregion


        #region "Location Department State Mapping Methods"
        //public async Task<ResultModel<DepartmentMappingModel>> GetById(TokenModel oTokenModel, long Id)
        //{
        //    ResultModel<DepartmentMappingModel> Result = new ResultModel<DepartmentMappingModel>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Id", Id);
        //        Result.Model = await _DBGateway.ExeScalarQuery<DepartmentMappingModel>("Select  * from mp_location_department_state Where Id=@Id", Pars);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}

        //public async Task<ResultModel<object>> GetAll(TokenModel oTokenModel, LocationDepartmentStateSearchModel oSearchModel)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Skip", oSearchModel.Skip);
        //        Pars.Add("@Take", oSearchModel.ItemsPerPage);
        //        Pars.Add("@Type", oSearchModel.Type);
        //        Pars.Add("@LocationDepartmentUid", oSearchModel.LocationDepartmentUid);
        //        Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_department_state_getall", Pars);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}

        public async Task<ResultModel<object>> Insert(TokenModel oTokenModel, DepartmentMappingModel oLocationDepartmentStateModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();

                Pars.Add("@LocationDepartmentUid", oLocationDepartmentStateModel.LocationDepartmentUid);
                Pars.Add("@StateCode", oLocationDepartmentStateModel.StateCode);
                Pars.Add("@StateDisplay", oLocationDepartmentStateModel.StateDisplay);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select * from mp_location_department_state Where LocationDepartmentUid=@LocationDepartmentUid and StateCode=@StateCode", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }

                Pars.Add("@Id", oLocationDepartmentStateModel.Id);
                Pars.Add("@LocationDepartmentStateUid", Helper.GenerateUniqueNumber());
                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@CreatedBy", oTokenModel.LoginId);
                Pars.Add("@CreatedDate", oLocationDepartmentStateModel.CreatedDate);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", oLocationDepartmentStateModel.CreatedDate);
                Pars.Add("@Source", oLocationDepartmentStateModel.Source);
                Pars.Add("@Active", oLocationDepartmentStateModel.Active);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_location_department_state "
                + " (Id,LocationDepartmentStateUid,LocationDepartmentUid,StateCode,StateDisplay, "
                + "CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active) values "

                + "(@Id,@LocationDepartmentStateUid,@LocationDepartmentUid,@StateCode,@StateDisplay, "
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

        //public async Task<ResultModel<object>> Update(TokenModel oTokenModel, DepartmentMappingModel oLocationDepartmentStateModel)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@LocationDepartmentUid", oLocationDepartmentStateModel.LocationDepartmentUid);
        //        Pars.Add("@StateCode", oLocationDepartmentStateModel.StateCode);
        //        Pars.Add("@StateDisplay", oLocationDepartmentStateModel.StateDisplay);
        //        //check if name already exist in system
        //        Result.Model = await _DBGateway.ExeScalarQuery<object>("Select  * from mp_location_department_state Where LocationDepartmentUid=@LocationDepartmentUid and StateCode<>@StateCode", Pars);
        //        if (Result.Model != null)
        //        {
        //            Result.Model = null;
        //            Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
        //            Result.MsgCode = Constants.NOTCREATED;
        //            return Result;
        //        }

        //        Pars.Add("@Id", oLocationDepartmentStateModel.Id);
        //        Pars.Add("@LocationDepartmentStateUid", oLocationDepartmentStateModel.LocationDepartmentStateUid);
        //        Pars.Add("@ModifiedBy", oLocationDepartmentStateModel.ModifiedBy);
        //        Pars.Add("@ModifiedDate", oLocationDepartmentStateModel.ModifiedDate);
        //        Pars.Add("@Source", oLocationDepartmentStateModel.Source);
        //        Pars.Add("@Active", oLocationDepartmentStateModel.Active);

        //        var res = await _DBGateway.ExeScalarQuery<int>("update mp_location_department_state "
        //        + "set LocationDepartmentStateUid=@LocationDepartmentStateUid,LocationDepartmentUid=@LocationDepartmentUid"
        //        + ",StateCode=@StateCode,StateDisplay=@StateDisplay"
        //        + ",ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source,Active=@Active"
        //        + " where Id=@Id;"
        //        + "select 1"
        //        , Pars);

        //        if (res == 0)
        //        {
        //            Result.Message = Constants.NOTUPDATED_MESSAGE;
        //            Result.MsgCode = Constants.NOTUPDATED;
        //        }
        //        else
        //            Result.Message = Constants.UPDATED_MESSAGE;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}

        public async Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id, bool IsHardDelete = false)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);
                {
                    Pars.Add("@CreatedById", oTokenModel.LoginId);
                    // dynamic query for hard delete and soft delete, it depends on the para.
                    string query = IsHardDelete ? "delete from mp_location_department_state where Id=@Id" :
                        "update mp_location_department_state set Active=false Where Id=@Id";

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

        //public async Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationDepartmentStateUid)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@LocationDepartmentStateUid", LocationDepartmentStateUid);
        //        //get active status value from table
        //        bool active = await _DBGateway.ExeScalarQuery<bool>("Select Active from mp_location_department_state Where LocationDepartmentStateUid=@LocationDepartmentStateUid", Pars);

        //        // if active status is true then make it false, else vice versa.
        //        Pars.Add("@Active", !active);

        //        var res = await _DBGateway.ExeQuery("update mp_location_department_state "
        //        + "set Active=@Active "
        //        + "where LocationDepartmentStateUid=@LocationDepartmentStateUid;"
        //        , Pars);

        //        if (res == 0)
        //        {
        //            Result.Message = Constants.NOTUPDATED_MESSAGE;
        //            Result.MsgCode = Constants.NOTUPDATED;
        //        }
        //        else
        //            Result.Message = Constants.UPDATED_MESSAGE;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}

        public async Task<ResultModel<DepartmentMappingModel>> GetMappedStatesByLocationDepartmentUid(TokenModel oTokenModel, string locationDepartmentUid)
        {
            ResultModel<DepartmentMappingModel> Result = new ResultModel<DepartmentMappingModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pLocationDepartmentUid", locationDepartmentUid);
                Result.LstModel = await _DBGateway.ExeSPList<DepartmentMappingModel>("sp_mp_location_department_state_by_locationDepartmentUid", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        #endregion
    }
}