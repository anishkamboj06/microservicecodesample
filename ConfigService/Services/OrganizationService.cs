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
    public class OrganizationService : IOrganization
    {
        private DBGateway _DBGateway;
        public OrganizationService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }

        #region "Private Functions"
        private async Task<bool> AddOrganizationCodeModel(OrganizationModel oOrganizationModel, OrganizationCodeMappingModel oOrganizationCodeMappingModel)
        {
            try
            {
                Dapper.DynamicParameters Pars1 = new Dapper.DynamicParameters();
                Pars1.Add("@OrganizationUid", oOrganizationModel.OrganizationUid);
                Pars1.Add("@CodeId", oOrganizationCodeMappingModel.CodeId);
                Pars1.Add("@Code", oOrganizationCodeMappingModel.Code);
                Pars1.Add("@CodeDisplay", oOrganizationCodeMappingModel.CodeDisplay);
                Pars1.Add("@CodeValue", oOrganizationCodeMappingModel.CodeValue);
                Pars1.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars1.Add("@CreatedBy", oOrganizationModel.CreatedBy);
                Pars1.Add("@CreatedDate", oOrganizationModel.CreatedDate);
                Pars1.Add("@ModifiedBy", oOrganizationModel.ModifiedBy);
                Pars1.Add("@ModifiedDate", oOrganizationModel.ModifiedDate);
                Pars1.Add("@Source", oOrganizationModel.Source);
                Pars1.Add("@Active", oOrganizationModel.Active);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_organization_code"
               + " (OrganizationUid,CodeId,Code,CodeDisplay,CodeValue,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Active,Source) "
               + " values(@OrganizationUid,@CodeId,@Code,@CodeDisplay,@CodeValue,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Active,@Source); "
               + " select 1 "
               , Pars1);
                if (res == 0)
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<bool> UpdateOrganizationCodeModel(OrganizationModel oOrganizationModel, OrganizationCodeMappingModel oOrganizationCodeMappingModel)
        {
            try
            {
                var res = 0;
                Dapper.DynamicParameters Pars1 = new Dapper.DynamicParameters();
                Pars1.Add("@OrganizationUid", oOrganizationModel.OrganizationUid);

                //check if OrganizationUid exist in system
                // if exist then update code value else insert new record in mp_organization_code table.
                int exist = await _DBGateway.ExeScalarQuery<int>("Select 1 from mp_organization_code Where OrganizationUid=@OrganizationUid", Pars1);

                Pars1.Add("@CodeId", oOrganizationCodeMappingModel.CodeId);
                Pars1.Add("@Code", oOrganizationCodeMappingModel.Code);
                Pars1.Add("@CodeDisplay", oOrganizationCodeMappingModel.CodeDisplay);
                Pars1.Add("@CodeValue", oOrganizationCodeMappingModel.CodeValue);
                Pars1.Add("@ModifiedBy", oOrganizationModel.ModifiedBy);
                Pars1.Add("@ModifiedDate", oOrganizationModel.ModifiedDate);
                Pars1.Add("@Source", oOrganizationModel.Source);
                Pars1.Add("@Active", oOrganizationModel.Active);
                // if exist then update code value
                if (exist > 0)
                {
                    res = await _DBGateway.ExeScalarQuery<int>("update mp_organization_code"
                    + " set CodeId=@CodeId,Code=@Code,CodeDisplay=@CodeDisplay,CodeValue=@CodeValue,"
                    + "ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source,Active=@Active"
                    + " where OrganizationUid=@OrganizationUid; "
                    + " select 1 "
                    , Pars1);
                }
                else// add new record in table: mp_organization_code
                {
                    Pars1.Add("@CreatedByType", CreatedByType.Admin.ToString());
                    Pars1.Add("@CreatedBy", oOrganizationModel.CreatedBy);
                    Pars1.Add("@CreatedDate", oOrganizationModel.CreatedDate);

                    res = await _DBGateway.ExeScalarQuery<int>("insert into mp_organization_code"
                  + " (OrganizationUid,CodeId,Code,CodeDisplay,CodeValue,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Active,Source) "
                  + " values(@OrganizationUid,@CodeId,@Code,@CodeDisplay,@CodeValue,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Active,@Source); "
                  + " select 1 "
                  , Pars1);
                }
                if (res == 0)
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion 
        public async Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                if (!checkOrganizationExistance(Id))
                {
                    Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                    Pars.Add("@Id", Id);
                    Pars.Add("@CreatedById", oTokenModel.LoginId);
                    Result.MsgCode = await _DBGateway.ExeQuery("DELETE FROM md_organization Where Id = @Id", Pars) == 1 ? 1 : 2;
                    if (Result.MsgCode == 1)
                        Result.Message = Constants.DELETE_MESSAGE;
                    else
                    {
                        Result.MsgCode = Constants.NOTDELETED;
                        Result.Message = Constants.NOTDELETE_MESSAGE;
                    }
                }
                else
                {
                    Result.MsgCode = Constants.CANNOT_DELETED;
                    Result.Message = Constants.CANNOT_DELETED_MESSAGE;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<OrganizationModel>> GetById(TokenModel oTokenModel, long Id)
        {
            ResultModel<OrganizationModel> Result = new ResultModel<OrganizationModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);
                Result.Model = await _DBGateway.ExeSPScaler<OrganizationModel>("sp_md_organization_getbyid", Pars);
                if (Result.Model != null)
                {
                    Result.Model.LstOrganizationCodeMappingModel = new List<OrganizationCodeMappingModel>();
                    Result.Model.LstOrganizationCodeMappingModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OrganizationCodeMappingModel>>(Result.Model.lstOrganizationCode ?? "[]");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<object>> GetAllByPaging(TokenModel oTokenModel, OrganizationSearchModel oSearchModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Skip", oSearchModel.Skip);
                Pars.Add("@Take", oSearchModel.ItemsPerPage);
                //Pars.Add("@Type", oSearchModel.Type);
                Pars.Add("@OrganizationType", oSearchModel.OrganizationType);
                //Pars.Add("@OrganizationDisplay", oSearchModel.OrganizationDisplay);
                Pars.Add("@Active", oSearchModel.Active);
                Pars.Add("@SearchValue", oSearchModel.SearchValue);
                Result.LstModel = await _DBGateway.ExeSPList<object>("sp_md_organization_getall", Pars);
                if (Result.LstModel != null && Result.LstModel.Count > 0)
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
        public async Task<ResultModel<object>> GetAllOrganization(TokenModel oTokenModel, bool isActive)
        {
            string query = string.Empty;
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                if(isActive)
                {
                    query = "SELECT  Id , OrganizationUid AS Uid , OrganizationDisplay AS Display, OrganizationDefinition FROM md_organization  WHERE Archived = FALSE AND Active = true;";
                }
                else
                {
                    query = "SELECT  Id , OrganizationUid AS Uid , OrganizationDisplay AS Display, OrganizationDefinition FROM md_organization  WHERE Archived = FALSE;";
                }
                
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Result.LstModel = await _DBGateway.ExeQueryList<object>(query, Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<object>> Insert(TokenModel oTokenModel, OrganizationModel oOrganizationModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@OrganizationType", oOrganizationModel.OrganizationType);
                Pars.Add("@OrganizationDisplay", oOrganizationModel.OrganizationDisplay);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select * from md_organization Where (OrganizationDisplay=@OrganizationDisplay and Archived = true) OR OrganizationDisplay=@OrganizationDisplay;", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }
                Pars.Add("@Id", oOrganizationModel.Id);
                string uniqueNumber = Helper.GenerateUniqueNumber();
                Pars.Add("@OrganizationUid", uniqueNumber);
                Pars.Add("@OrganizationDefinition", oOrganizationModel.OrganizationDefinition);
                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@GovernedBy", oOrganizationModel.GovernedBy);
                Pars.Add("@CreatedBy", oTokenModel.LoginId);
                Pars.Add("@CreatedDate", oOrganizationModel.CreatedDate);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", oOrganizationModel.ModifiedDate);
                Pars.Add("@Source", oOrganizationModel.Source);
                Pars.Add("@Active", oOrganizationModel.Active);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into md_organization"
                    + " (Id,OrganizationUid,OrganizationType,OrganizationDisplay,OrganizationDefinition,CreatedByType,GovernedBy,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active) "
                    + " values(@Id,@OrganizationUid,@OrganizationType,@OrganizationDisplay,@OrganizationDefinition,@CreatedByType,@GovernedBy,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Source,@Active); "
                    + " select 1 "
                    , Pars);

                if (res == 0)
                {
                    Result.Message = Constants.NOTCREATED_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                }
                else
                {
                    // mapping code with organization only if code id is greater than zero
                    foreach (var organizationCodeModel in oOrganizationModel.LstOrganizationCodeMappingModel)
                    {
                        // code id should be greater than zero
                        if (organizationCodeModel.CodeId > 0)
                        {
                            oOrganizationModel.OrganizationUid = uniqueNumber;
                            var result = await AddOrganizationCodeModel(oOrganizationModel, organizationCodeModel);
                        }
                    }
                    Result.Message = Constants.CREATED_MESSAGE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<object>> Update(TokenModel oTokenModel, OrganizationModel oOrganizationModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", oOrganizationModel.Id);
                Pars.Add("@OrganizationType", oOrganizationModel.OrganizationType);
                Pars.Add("@OrganizationDisplay", oOrganizationModel.OrganizationDisplay);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select * from md_organization Where (OrganizationDisplay=@OrganizationDisplay and Archived = true and Id<>@Id) OR (OrganizationDisplay=@OrganizationDisplay and Id<>@Id);", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }
                Pars.Add("@OrganizationUid", oOrganizationModel.OrganizationUid);
                Pars.Add("@OrganizationDefinition", oOrganizationModel.OrganizationDefinition);
                Pars.Add("@GovernedBy", oOrganizationModel.GovernedBy);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", oOrganizationModel.ModifiedDate);
                Pars.Add("@Source", oOrganizationModel.Source);
                Pars.Add("@Active", oOrganizationModel.Active);

                var res = await _DBGateway.ExeScalarQuery<int>("update md_organization"
                    + " set OrganizationUid = @OrganizationUid, OrganizationType=@OrganizationType,OrganizationDisplay=@OrganizationDisplay,OrganizationDefinition=@OrganizationDefinition,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source,Active=@Active,GovernedBy=@GovernedBy "
                    + " where Id=@Id; "

                    + " select 1 "
                    , Pars);
                if (res == 0)
                {
                    Result.Message = Constants.NOTUPDATED_MESSAGE;
                    Result.MsgCode = Constants.NOTUPDATED;
                }
                else
                {
                    // firstly delete all codes of organization and then insert new ones.
                    Dapper.DynamicParameters Pars1 = new Dapper.DynamicParameters();
                    Pars1.Add("@OrganizationUid", oOrganizationModel.OrganizationUid);
                    int deleted = await _DBGateway.ExeQuery("Delete from mp_organization_code Where OrganizationUid=@OrganizationUid", Pars1);

                    // mapping code with organization only if code id is greater than zero
                    foreach (var organizationCodeModel in oOrganizationModel.LstOrganizationCodeMappingModel)
                    {
                        // code id should be greater than zero
                        if (organizationCodeModel.CodeId > 0)
                        {
                            var result = await AddOrganizationCodeModel(oOrganizationModel, organizationCodeModel);
                        }
                    }
                    Result.Message = Constants.UPDATED_MESSAGE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<string>> GetOrganizationTypes(TokenModel oTokenModel)
        {
            ResultModel<string> Result = new ResultModel<string>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                List<string> OrganizationTypes = new List<string>() { "State", "UT", "Organization", "Instituition" };

                Result.LstModel = OrganizationTypes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string OrganizationUid)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@OrganizationUid", OrganizationUid);
                //get active status value from table
                bool active = await _DBGateway.ExeScalarQuery<bool>("Select Active from md_organization Where OrganizationUid=@OrganizationUid", Pars);

                // if active status is true then make it false, else vice versa.
                Pars.Add("@Active", !active);

                var res = await _DBGateway.ExeQuery("update md_organization "
                + "set Active=@Active "
                + "where OrganizationUid=@OrganizationUid;"
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

        private bool checkOrganizationExistance(long Id)
        {
            bool status = false;
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Id", Id);
            var existInHealthFacility = _DBGateway.ExeScalarQuery<int>("SELECT 1 FROM md_location WHERE OrganizationUid = (SELECT OrganizationUid FROM md_organization WHERE Id = @Id LIMIT 1) LIMIT 1", pars);
            if (existInHealthFacility != null && existInHealthFacility.Result == 1)
            {
                status = true;
            }
            else
            {
                var existInState = _DBGateway.ExeScalarQuery<int>("SELECT 1 FROM mp_organization_state WHERE OrganizationUid = (SELECT OrganizationUid FROM md_organization WHERE Id = @Id LIMIT 1) LIMIT 1", pars);
                if (existInState != null && existInState.Result == 1)
                {
                    status = true;
                }
            }
            return status;
        }
        public async Task<string> GetGovernedByCount(TokenModel oTokenModel, string GovernedBy)
        {
            int GovernedByCount = 0;
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@GovernedBy", GovernedBy);
                GovernedByCount = await _DBGateway.ExeScalarQuery<int>("Select count(GovernedBy) from md_organization where GovernedBy=@GovernedBy", Pars);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return GovernedByCount.ToString();
        }
        public async Task<string> GetOrganizationTypeCount(TokenModel oTokenModel, string OrganizationType)
        {
            int OrganizationTypeCount = 0;
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@OrganizationType", OrganizationType);
                OrganizationTypeCount = await _DBGateway.ExeScalarQuery<int>("Select count(OrganizationType) from md_organization where OrganizationType=@OrganizationType", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OrganizationTypeCount.ToString();
        }
    }
}
