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
    public class LocationNetworkService : ILocationNetwork
    {
        private DBGateway _DBGateway;
        public LocationNetworkService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }
        public async Task<ResultModel<object>> Delete(TokenModel oTokenModel, string Id,bool IsHardDelete = false)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);
                {
                    Pars.Add("@CreatedById", oTokenModel.LoginId);

                    // dynamic query for hard delete and soft delete, it depends on the para.
                    string query = IsHardDelete ? "delete from mp_location_network where LocationNetworkUid=@Id" :
                        "update mp_location_network set Active=false Where LocationNetworkUid=@Id";
                    
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
        public async Task<ResultModel<LocationNetworkModel>> GetById(TokenModel oTokenModel, long Id)
        {
            ResultModel<LocationNetworkModel> Result = new ResultModel<LocationNetworkModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);
                Result.Model = await _DBGateway.ExeScalarQuery<LocationNetworkModel>("Select  * from mp_location_network Where Id=@Id", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        //public async Task<ResultModel<object>> GetAll(TokenModel oTokenModel, LocationNetworkSearchModel oSearchModel)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Skip", oSearchModel.Skip);
        //        Pars.Add("@Take", oSearchModel.ItemsPerPage);
        //        Pars.Add("@Type", oSearchModel.Type);
        //        Pars.Add("@LocationNetworkUid", oSearchModel.LocationNetworkUid);
        //        Pars.Add("@OrganizationUid", oSearchModel.OrganizationUid);
        //        Pars.Add("@ParentLocationUid", oSearchModel.ParentLocationUid);
        //        Pars.Add("@LocationUid", oSearchModel.ParentLocationUid);
        //        Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_network_getall", Pars);
        //        if (Result.LstModel != null && Result.LstModel.Count > 0)
        //        {
        //            var data = ((IDictionary<string, object>)Result.LstModel.FirstOrDefault());
        //            Result.TotalRecords = data != null && data["TotalRecords"] != null ? Convert.ToInt32(data["TotalRecords"]) : 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}
        public async Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationNetworkModel oLocationNetworkModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@ParentLocationUid", oLocationNetworkModel.ParentLocationUid);
                Pars.Add("@LocationUid", oLocationNetworkModel.LocationUid);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select * from mp_location_network Where(ParentLocationUid = @ParentLocationUid and LocationUid = @LocationUid) or (LocationUid = @ParentLocationUid and ParentLocationUid = @LocationUid)", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }
                Pars.Add("@Id", oLocationNetworkModel.Id);
                Pars.Add("@LocationNetworkUid", Helper.GenerateUniqueNumber());                
                Pars.Add("@OrganisationUid", oLocationNetworkModel.OrganisationUid);
                Pars.Add("@Distance", oLocationNetworkModel.Distance);
                Pars.Add("@TotalTime", oLocationNetworkModel.TotalTime);
                Pars.Add("@Fare", oLocationNetworkModel.Fare);
                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@CreatedBy", oTokenModel.LoginId);
                Pars.Add("@CreatedDate", oLocationNetworkModel.CreatedDate);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", oLocationNetworkModel.ModifiedDate);
                Pars.Add("@Source", oLocationNetworkModel.Source);
                Pars.Add("@Active", oLocationNetworkModel.Active);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_location_network"
                    + " (Id,LocationNetworkUid,ParentLocationUid,LocationUid,OrganisationUid,Distance,TotalTime,Fare,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active) "
                    + " values(@Id,@LocationNetworkUid,@ParentLocationUid,@LocationUid,@OrganisationUid,@Distance,@TotalTime,@Fare,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Source,@Active); "
                    + " select 1 "
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
        //public async Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationNetworkModel oLocationNetworkModel)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Id", oLocationNetworkModel.Id);
        //        Pars.Add("@LocationNetworkUid", oLocationNetworkModel.LocationNetworkUid);
        //        //check if name already exist in system
        //        //Result.Model = await _DBGateway.ExeScalarQuery<object>("Select  * from mp_location_network Where  LocationNetworkUid=@LocationNetworkUid and Id<>@Id", Pars);
        //        //if (Result.Model != null)
        //        //{
        //        //    Result.Model = null;
        //        //    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
        //        //    Result.MsgCode = Constants.NOTCREATED;
        //        //    return Result;
        //        //}
        //        Pars.Add("@ParentLocationUid", oLocationNetworkModel.ParentLocationUid);
        //        Pars.Add("@LocationUid", oLocationNetworkModel.LocationUid);
        //        Pars.Add("@OrganisationUid", oLocationNetworkModel.OrganisationUid);
        //        Pars.Add("@Distance", oLocationNetworkModel.Distance);
        //        Pars.Add("@TotalTime", oLocationNetworkModel.TotalTime);
        //        Pars.Add("@Fare", oLocationNetworkModel.Fare);
        //        Pars.Add("@ModifiedBy", oLocationNetworkModel.ModifiedBy);
        //        Pars.Add("@ModifiedDate", oLocationNetworkModel.ModifiedDate);
        //        Pars.Add("@Source", oLocationNetworkModel.Source);
        //        Pars.Add("@Active", oLocationNetworkModel.Active);

        //        var res = await _DBGateway.ExeScalarQuery<int>("update mp_location_network"
        //            + " set LocationNetworkUid = @LocationNetworkUid, ParentLocationUid=@ParentLocationUid,LocationUid=@LocationUid,OrganisationUid=@OrganisationUid,Distance=@Distance,TotalTime=@TotalTime,Fare=@Fare,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source,Active=@Active"
        //            + " where Id=@Id; "

        //            + " select 1 "
        //            , Pars);
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
        //public async Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationNetworkUid)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@LocationNetworkUid", LocationNetworkUid);
        //        //get active status value from table
        //        bool active = await _DBGateway.ExeScalarQuery<bool>("Select Active from mp_location_network Where LocationNetworkUid=@LocationNetworkUid", Pars);

        //        // if active status is true then make it false, else vice versa.
        //        Pars.Add("@Active", !active);

        //        var res = await _DBGateway.ExeQuery("update mp_location_network "
        //        + "set Active=@Active "
        //        + "where LocationNetworkUid=@LocationNetworkUid;"
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
        //public async Task<ResultModel<object>> GetFacilityListByTypeAndOrgUid(TokenModel oTokenModel, LocationNetworkSearchModel oSearchModel)
        //{

        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@OrganizationUid", oSearchModel.OrganizationUid);
        //        Pars.Add("@TypeCode", oSearchModel.TypeCode);
        //        Result.LstModel = await _DBGateway.ExeQueryList<object>("Select LocationUid, Name from md_location where OrganizationUid=@OrganizationUid AND TypeCode = @TypeCode", Pars);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}
        //public async Task<ResultModel<object>> GetFacilityListByLocationUid(TokenModel oTokenModel, LocationNetworkSearchModel oSearchModel)
        //{

        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Skip", oSearchModel.Skip);
        //        Pars.Add("@Take", oSearchModel.ItemsPerPage);
        //        Pars.Add("@Type", oSearchModel.Type);
        //        Pars.Add("@LocationUid", oSearchModel.ParentLocationUid);
        //        Result.LstModel = await _DBGateway.ExeSPList<object>("sp_get_facility_by_locUid", Pars);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}
        //public async Task<ResultModel<object>> UnMappedfacility(TokenModel oTokenModel, string LocationNetworkUid)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@LocationNetworkUid", LocationNetworkUid);
        //        Result.MsgCode = await _DBGateway.ExeQuery("update mp_location_network set Archived = true Where LocationNetworkUid=@LocationNetworkUid", Pars) == 1 ? 1 : 2;
        //        if (Result.MsgCode == 1)
        //            Result.Message = Constants.DELETE_MESSAGE;
        //        else
        //        {
        //            Result.MsgCode = Constants.NOTDELETED;
        //            Result.Message = Constants.NOTDELETE_MESSAGE;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}
        //public async Task<ResultModel<object>> SaveFacilityNetwork(TokenModel oTokenModel, List<LocationNetworkModel> oLocationNetworkModel)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        foreach (var item in oLocationNetworkModel)
        //        {
        //            await Insert(oTokenModel, item);
        //        }

        //        Result.Message = Constants.CREATED_MESSAGE;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}
        public async Task<ResultModel<object>> GetChildLocByParentLocUId(TokenModel oTokenModel, string ParentLocUId)
        {

            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@ParentLocUId", ParentLocUId);
                Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_network_getall_ChildLoc_By_ParentLocUid", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> GetParentLocByChildLocUId(TokenModel oTokenModel, string ParentLocUId)
        {

            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pChildtLocUId", ParentLocUId);
                Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_network_getall_ParentLoc_By_ChildLocUid", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        
        public async Task<ResultModel<object>> UpdateLocNetworkDistanceParam(TokenModel oTokenModel, LocNetworkDistanceParameterModel oLocNetworkDistanceParameterModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();                
                Pars.Add("@LocationNetworkUid", oLocNetworkDistanceParameterModel.LocationNetworkUid);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select * from mp_location_network Where LocationNetworkUid=@LocationNetworkUid", Pars);
                if (Result.Model == null)
                {
                    Result.Model = null;
                    Result.Message = Constants.NOTUPDATED_MESSAGE;
                    Result.MsgCode = Constants.NOTUPDATED;
                    return Result;
                }
                if (int.Parse(oLocNetworkDistanceParameterModel.Distance) >= 0 && int.Parse(oLocNetworkDistanceParameterModel.TotalTime) >= 0 && int.Parse(oLocNetworkDistanceParameterModel.Fare) >= 0)
                {


                    Pars.Add("@Distance", oLocNetworkDistanceParameterModel.Distance);
                    Pars.Add("@TotalTime", oLocNetworkDistanceParameterModel.TotalTime);
                    Pars.Add("@Fare", oLocNetworkDistanceParameterModel.Fare);
                    Pars.Add("@ModifiedBy", oLocNetworkDistanceParameterModel.ModifiedBy);
                    Pars.Add("@ModifiedDate", oLocNetworkDistanceParameterModel.ModifiedDate);

                    // Update records
                    var res = await _DBGateway.ExeScalarQuery<int>("update mp_location_network"
                        + " set Distance=@Distance,TotalTime=@TotalTime,Fare=@Fare,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate"
                        + " where LocationNetworkUid=@LocationNetworkUid; "

                        + " select 1 "
                        , Pars);
                    if (res == 0)
                    {
                        Result.Message = Constants.NOTUPDATED_MESSAGE;
                        Result.MsgCode = Constants.NOTUPDATED;
                    }
                    else
                    {
                        Result.Message = Constants.UPDATED_MESSAGE;
                        Result.Model = res;
                    }
                }
                else
                {
                    Result.Message = Constants.CANNOT_UPDATE_NEGATIVE_MESSAGE;
                    Result.MsgCode = Constants.NEGATIVE_VALUE_CODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
    }
}

