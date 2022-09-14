using System;
using System.Threading.Tasks;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using ConfigurationService.Utility;
using System.Collections.Generic;
using System.Linq;
using Models;
using CommonLibrary.Utility;

namespace ConfigurationService.Services
{
    public class OrganizationStateService : IOrganizationState
    {
        private DBGateway _DBGateway;
        public OrganizationStateService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }
        public async Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);
                {
                    Pars.Add("@CreatedById", oTokenModel.LoginId);
                    Result.MsgCode = await _DBGateway.ExeQuery("update mp_organization_state set Archived = true Where Id=@Id", Pars) == 1 ? 1 : 2;
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
        //public async Task<ResultModel<OrganizationStateModel>> GetById(TokenModel oTokenModel, long Id)
        //{
        //    ResultModel<OrganizationStateModel> Result = new ResultModel<OrganizationStateModel>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Id", Id);
        //        Result.Model = await _DBGateway.ExeScalarQuery<OrganizationStateModel>("Select  * from mp_organization_state Where Id=@Id", Pars);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}

        //public async Task<ResultModel<object>> GetAll(TokenModel oTokenModel, OrganizationStateSearchModel oSearchModel)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Skip", oSearchModel.Skip);
        //        Pars.Add("@Take", oSearchModel.ItemsPerPage);
        //        Pars.Add("@Type", oSearchModel.Type);
        //        Pars.Add("@OrganizationDisplay", oSearchModel.OrganizationDisplay);
        //        Pars.Add("@OrganizationStateUid", oSearchModel.OrganizationStateUid);
        //        Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_organization_state_getall", Pars);
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

        public async Task<ResultModel<object>> Insert(TokenModel oTokenModel, OrganizationStateModel oOrganizationStateModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                //check if Organization Display already exist in system
                Pars.Add("@OrganizationDisplay", oOrganizationStateModel.OrganizationDisplay);
                Pars.Add("@OrganizationUid", oOrganizationStateModel.OrganizationUid);
                Pars.Add("@StateCode", oOrganizationStateModel.StateCode);
                Pars.Add("@StateDisplay", oOrganizationStateModel.StateDisplay);
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select  * from mp_organization_state Where StateCode = @StateCode and OrganizationUid=@OrganizationUid", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }
                Pars.Add("@Id", oOrganizationStateModel.Id);
                Pars.Add("@OrganizationStateUid", Helper.GenerateUniqueNumber());
                Pars.Add("@OrganizationUid", oOrganizationStateModel.OrganizationUid);
                Pars.Add("@StateCode", oOrganizationStateModel.StateCode);
                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@CreatedBy", oTokenModel.LoginId);
                Pars.Add("@CreatedDate", oOrganizationStateModel.CreatedDate);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", oOrganizationStateModel.ModifiedDate);
                Pars.Add("@Source", oOrganizationStateModel.Source);
                Pars.Add("@Active", oOrganizationStateModel.Active);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_organization_state"
                    + " (Id,OrganizationStateUid,OrganizationUid,OrganizationDisplay,StateCode,StateDisplay,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active) "
                    + " values(@Id, @OrganizationStateUid,@OrganizationUid,@OrganizationDisplay,@StateCode,@StateDisplay,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Source,@Active); "
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
        //public async Task<ResultModel<object>> Update(TokenModel oTokenModel, OrganizationStateModel oOrganizationStateModel)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Id", oOrganizationStateModel.Id);
        //        Pars.Add("@OrganizationDisplay", oOrganizationStateModel.OrganizationDisplay);
        //        Pars.Add("@StateDisplay", oOrganizationStateModel.StateDisplay);
        //        Pars.Add("@StateCode", oOrganizationStateModel.StateCode);
        //        //check if Organization Display already exist in system
        //        Result.Model = await _DBGateway.ExeScalarQuery<object>("Select  * from mp_organization_state Where StateCode=@StateCode and Id<>@Id", Pars);
        //        if (Result.Model != null)
        //        {
        //            Result.Model = null;
        //            Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
        //            Result.MsgCode = Constants.NOTCREATED;
        //            return Result;
        //        }
        //        Pars.Add("@OrganizationStateUid", oOrganizationStateModel.OrganizationStateUid);
        //        Pars.Add("@OrganizationUid", oOrganizationStateModel.OrganizationUid);
        //        Pars.Add("@StateCode", oOrganizationStateModel.StateCode);
        //        Pars.Add("@StateDisplay", oOrganizationStateModel.StateDisplay);
        //        Pars.Add("@ModifiedBy", oOrganizationStateModel.ModifiedBy);
        //        Pars.Add("@ModifiedDate", oOrganizationStateModel.ModifiedDate);
        //        Pars.Add("@Source", oOrganizationStateModel.Source);
        //        Pars.Add("@Active", oOrganizationStateModel.Active);

        //        var res = await _DBGateway.ExeScalarQuery<int>("update mp_organization_state"
        //            + " set OrganizationStateUid = @OrganizationStateUid,OrganizationUid = @OrganizationUid, OrganizationDisplay = @OrganizationDisplay,StateCode=@StateCode,StateDisplay=@StateDisplay,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source,Active=@Active"
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
                    string query = IsHardDelete ? "delete from mp_organization_state where Id=@Id" :
                        "update mp_organization_state set Active=false Where Id=@Id";
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
        public async Task<ResultModel<SubOrganizationStateModel>> GetOrganizationStateByOrganizationId(string organizationUId)
        {
            ResultModel<SubOrganizationStateModel> Result = new ResultModel<SubOrganizationStateModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pOrganizationUId", organizationUId);
                Result.LstModel = await _DBGateway.ExeSPList<SubOrganizationStateModel>("sp_mp_organization_state_by_organizationuid", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        //    public async Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string OrganizationStateUid)
        //    {
        //        ResultModel<object> Result = new ResultModel<object>();
        //        try
        //        {
        //            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //            Pars.Add("@OrganizationStateUid", OrganizationStateUid);
        //            //get active status value from table
        //            bool active = await _DBGateway.ExeScalarQuery<bool>("Select Active from mp_organization_state Where OrganizationStateUid=@OrganizationStateUid", Pars);

        //            // if active status is true then make it false, else vice versa.
        //            Pars.Add("@Active", !active);

        //            var res = await _DBGateway.ExeQuery("update mp_organization_state "
        //            + "set Active=@Active "
        //            + "where OrganizationStateUid=@OrganizationStateUid;"
        //            , Pars);

        //            if (res == 0)
        //            {
        //                Result.Message = Constants.NOTUPDATED_MESSAGE;
        //                Result.MsgCode = Constants.NOTUPDATED;
        //            }
        //            else
        //                Result.Message = Constants.UPDATED_MESSAGE;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        return Result;
        //    }
    }
}