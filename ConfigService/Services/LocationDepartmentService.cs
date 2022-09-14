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
    public class LocationDepartmentService : ILocationDepartment
    {
        private DBGateway _DBGateway;

        public LocationDepartmentService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }

        public async Task<ResultModel<LocationDepartmentModel>> GetById(TokenModel oTokenModel, long Id)
        {
            ResultModel<LocationDepartmentModel> Result = new ResultModel<LocationDepartmentModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", Id);
                Result.LstModel = await _DBGateway.ExeSPList<LocationDepartmentModel>("sp_mp_location_department_getbyid", Pars);
                if (Result.LstModel != null && Result.LstModel.Count > 0)
                {
                    Result.Model = Result.LstModel.FirstOrDefault();
                    Result.LstModel = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> getAllOPDByPaging(TokenModel oTokenModel, LocationDepartmentSearchModel oSearchModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pSkip", oSearchModel.Skip);
                Pars.Add("@pTake", oSearchModel.ItemsPerPage);
                Pars.Add("@pType", oSearchModel.Type);
                Pars.Add("@pLocationUid", oSearchModel.LocationUid);
                Pars.Add("@pDepartmentDisplay", oSearchModel.DepartmentDisplay);
                Pars.Add("@pOrganizationType", oSearchModel.OrganizationType);
                Pars.Add("@pActive", oSearchModel.Active);
                Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_department_getall", Pars);
                if (Result.LstModel != null && Result.LstModel.Count > 0)
                {
                    var data = (IDictionary<string, object>)Result.LstModel.FirstOrDefault();
                    Result.TotalRecords = data != null && data["TotalRecords"] != null ? Convert.ToInt32(data["TotalRecords"]) : 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationDepartmentModel oLocationDepartmentModel, string LocationName)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                oLocationDepartmentModel.LocationDepartmentName = LocationName + "(" + oLocationDepartmentModel.DepartmentDisplay + ")";

                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();

                Pars.Add("@pDepartmentDisplay", oLocationDepartmentModel.DepartmentDisplay);
                Pars.Add("@pLocationUid", oLocationDepartmentModel.LocationUid);
                Pars.Add("@pDepartmentCode", oLocationDepartmentModel.DepartmentCode);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select  * from mp_location_department Where DepartmentCode=@pDepartmentCode and LocationUid = @pLocationUid", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }

                Pars.Add("@pId", oLocationDepartmentModel.Id);
                Pars.Add("@pLocationDepartmentUid", Helper.GenerateUniqueNumber());
                Pars.Add("@pLocationDepartmentName", oLocationDepartmentModel.LocationDepartmentName);
                Pars.Add("@pOrganizationUid", oLocationDepartmentModel.OrganizationUid);
                Pars.Add("@pIsSpecial", oLocationDepartmentModel.IsSpecial);
                Pars.Add("@pCreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@pCreatedBy", oTokenModel.LoginId);
                Pars.Add("@pCreatedDate", oLocationDepartmentModel.CreatedDate);
                Pars.Add("@pModifiedBy", oTokenModel.LoginId);// first time modifiedby will be same as createdby
                Pars.Add("@pModifiedDate", oLocationDepartmentModel.CreatedDate);// first time modifieddate will be same as createddate
                Pars.Add("@pSource", oLocationDepartmentModel.Source);
                Pars.Add("@pActive", oLocationDepartmentModel.Active);
                Pars.Add("@pAddressLine1", oLocationDepartmentModel.AddressLine1);
                Pars.Add("@pAddressLine2", oLocationDepartmentModel.AddressLine2);
                Pars.Add("@pStateCode", oLocationDepartmentModel.StateCode);
                Pars.Add("@pStateDisplay", oLocationDepartmentModel.StateDisplay);
                Pars.Add("@pDistrictCode", oLocationDepartmentModel.DistrictCode);
                Pars.Add("@pDistrictDisplay", oLocationDepartmentModel.DistrictDisplay);
                Pars.Add("@pCityCode", oLocationDepartmentModel.CityCode);
                Pars.Add("@pCityDisplay", oLocationDepartmentModel.CityDisplay);
                Pars.Add("@pPostalCode", oLocationDepartmentModel.PostalCode);
                Pars.Add("@pMobile", oLocationDepartmentModel.Mobile);
                Pars.Add("@pEmail", oLocationDepartmentModel.Email);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_location_department "
                + " (Id,LocationDepartmentUid,LocationUid,DepartmentCode,DepartmentDisplay,OrganizationUid,IsSpecial, "
                + "CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active,AddressLine1,AddressLine2,"
                + " StateCode,StateDisplay,DistrictCode,DistrictDisplay,CityCode,CityDisplay,PostalCode,Mobile,Email,LocationDepartmentName) values "

                + "(@pId,@pLocationDepartmentUid,@pLocationUid,@pDepartmentCode,@pDepartmentDisplay,@pOrganizationUid, "
                + "@pIsSpecial,@pCreatedByType,@pCreatedBy,@pCreatedDate,@pModifiedBy,@pModifiedDate,@pSource,@pActive, "
                + " @pAddressLine1,@pAddressLine2,@pStateCode,@pStateDisplay,@pDistrictCode,@pDistrictDisplay,@pCityCode,@pCityDisplay,@pPostalCode,@pMobile,@pEmail,@pLocationDepartmentName);"
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

        public async Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationDepartmentModel oLocationDepartmentModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pId", oLocationDepartmentModel.Id);
                Pars.Add("@pDepartmentCode", oLocationDepartmentModel.DepartmentCode);
                Pars.Add("@pLocationUid", oLocationDepartmentModel.LocationUid);
                Pars.Add("@pLocationDepartmentName", oLocationDepartmentModel.LocationDepartmentName);
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select 1 from mp_location_department where Id<>@pId and DepartmentCode=@pDepartmentCode and LocationUid=@pLocationUid and LocationDepartmentName =@pLocationDepartmentName", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }
                Pars.Add("@pOrganizationUid", oLocationDepartmentModel.OrganizationUid);
                Pars.Add("@pDepartmentDisplay", oLocationDepartmentModel.DepartmentDisplay);
                Pars.Add("@pIsSpecial", oLocationDepartmentModel.IsSpecial);
                Pars.Add("@pModifiedBy", oLocationDepartmentModel.ModifiedBy);
                Pars.Add("@pModifiedDate", oLocationDepartmentModel.ModifiedDate);
                Pars.Add("@pSource", oLocationDepartmentModel.Source);
                Pars.Add("@pActive", oLocationDepartmentModel.Active);
                Pars.Add("@pAddressLine1", oLocationDepartmentModel.AddressLine1);
                Pars.Add("@pAddressLine2", oLocationDepartmentModel.AddressLine2);
                Pars.Add("@pStateCode", oLocationDepartmentModel.StateCode);
                Pars.Add("@pStateDisplay", oLocationDepartmentModel.StateDisplay);
                Pars.Add("@pDistrictCode", oLocationDepartmentModel.DistrictCode);
                Pars.Add("@pDistrictDisplay", oLocationDepartmentModel.DistrictDisplay);
                Pars.Add("@pCityCode", oLocationDepartmentModel.CityCode);
                Pars.Add("@pCityDisplay", oLocationDepartmentModel.CityDisplay);
                Pars.Add("@pPostalCode", oLocationDepartmentModel.PostalCode);
                Pars.Add("@pMobile", oLocationDepartmentModel.Mobile);
                Pars.Add("@pEmail", oLocationDepartmentModel.Email);

                var res = await _DBGateway.ExeScalarQuery<int>("update mp_location_department "
                + "set LocationUid=@pLocationUid,DepartmentCode=@pDepartmentCode"
                + ",DepartmentDisplay=@pDepartmentDisplay,OrganizationUid=@pOrganizationUid,IsSpecial=@pIsSpecial"
                + ",ModifiedBy=@pModifiedBy,ModifiedDate=@pModifiedDate,Source=@pSource,Active=@pActive"
                + ",AddressLine1=@pAddressLine1,AddressLine2=@pAddressLine2,StateCode=@pStateCode,StateDisplay=@pStateDisplay"
                + ",DistrictCode=@pDistrictCode,DistrictDisplay=@pDistrictDisplay,CityCode=@pCityCode,CityDisplay=@pCityDisplay,PostalCode=@pPostalCode,Mobile=@pMobile,Email=@pEmail,LocationDepartmentName=@pLocationDepartmentName"
                + " where Id=@pId;"
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


        public async Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id, bool IsHardDelete = false)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                if (!checkOPDExistance(Id))
                {
                    Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                    Pars.Add("@Id", Id);
                    Pars.Add("@CreatedById", oTokenModel.LoginId);

                    // dynamic query for hard delete and soft delete, it depends on the para.
                    string query = IsHardDelete ? "delete from mp_location_department where Id=@Id" :
                        "update mp_location_department set Active=false Where Id=@Id";

                    Result.MsgCode = await _DBGateway.ExeQuery(query, Pars) == 1 ? 1 : 2;
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

        public async Task<ResultModel<LocationDepartmentModel>> GetLocDeptByLocationUId(TokenModel oTokenModel, string LocUId)
        {
            ResultModel<LocationDepartmentModel> Result = new ResultModel<LocationDepartmentModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@LocUId", LocUId);
                Result.LstModel = await _DBGateway.ExeSPList<LocationDepartmentModel>("sp_mp_location_department_by_locationuid", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationDepartmentUid)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@LocationDepartmentUid", LocationDepartmentUid);
                //get active status value from table
                bool active = await _DBGateway.ExeScalarQuery<bool>("Select Active from mp_location_department Where LocationDepartmentUid=@LocationDepartmentUid", Pars);

                // if active status is true then make it false, else vice versa.
                Pars.Add("@Active", !active);

                var res = await _DBGateway.ExeQuery("update mp_location_department "
                + "set Active=@Active "
                + "where LocationDepartmentUid=@LocationDepartmentUid;"
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
        //public async Task<ResultModel<object>> UnMap(TokenModel oTokenModel, string id)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@Id", id);
        //        //get active status value from table
        //        int deletedRecords = await _DBGateway.ExeQuery("Delete from mp_location_department Where Id=@Id", Pars);

        //        if (deletedRecords == 0)
        //        {
        //            Result.Message = Constants.NOTDELETE_MESSAGE;
        //            Result.MsgCode = Constants.NOTDELETED;
        //        }
        //        else
        //            Result.Message = Constants.DELETE_MESSAGE;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}

        //public async Task<ResultModel<object>> GetAllUnmappedDeptByLocationUId(TokenModel oTokenModel, String LocationUid)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@LocationUid", LocationUid);
        //        string queryString = "SELECT * FROM mp_location_department WHERE LocationUid != @LocationUid ;";
        //        Result.LstModel = await _DBGateway.ExeQueryList<object>(queryString, Pars);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}

        //public async Task<ResultModel<object>> GetLocDeptByLocationUIdWithPagination(TokenModel oTokenModel, LocationDepartmentSearchModel oSearchModel)
        //{
        //    ResultModel<object> Result = new ResultModel<object>();
        //    try
        //    {
        //        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
        //        Pars.Add("@LocationUid", oSearchModel.LocationUid);
        //        Pars.Add("@Skip", oSearchModel.Skip);
        //        Pars.Add("@Take", oSearchModel.ItemsPerPage);
        //        Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_department_by_locationuid_with_pagination", Pars);
        //        if (Result.LstModel != null && Result.LstModel.Count > 0)
        //        {
        //            var data = (IDictionary<string, object>)Result.LstModel.FirstOrDefault();
        //            Result.TotalRecords = data != null && data["TotalRecords"] != null ? Convert.ToInt32(data["TotalRecords"]) : 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Result;
        //}

        private bool checkOPDExistance(long Id)
        {
            bool status = false;
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Id", Id);
            // check existance in practitioner mapping table
            var existInPractitionerMapping = _DBGateway.ExeScalarQuery<int>("SELECT 1 FROM mp_location_department_practitioner WHERE LocationDepartmentUid = (SELECT LocationDepartmentUid FROM mp_location_department WHERE Id = @Id LIMIT 1) LIMIT 1", pars);
            if (existInPractitionerMapping != null && existInPractitionerMapping.Result == 1)
            {
                status = true;
            }
            else
            {
                // check existance in location department state table
                var existInState = _DBGateway.ExeScalarQuery<int>("SELECT 1 FROM mp_location_department_state WHERE LocationDepartmentUid = (SELECT LocationDepartmentUid FROM mp_location_department WHERE Id = @Id LIMIT 1) LIMIT 1", pars);
                if (existInState != null && existInState.Result == 1)
                {
                    status = true;
                }
            }
            return status;
        }       
        public async Task<string> UnmapLocDeptPrac(TokenModel oTokenModel, string locationDepartmentPractitionerUid)
        {
            int Result = 0;
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@PractitionerProviderDepartmentUid", locationDepartmentPractitionerUid);              

                string query = "delete from mp_location_department_practitioner where PractitionerProviderDepartmentUid=@PractitionerProviderDepartmentUid";
                Result = await _DBGateway.ExeQuery(query, Pars) == 1 ? 1 : 2;                 
            }
            catch (Exception ex)
            {               
                throw ex;
            }
            return Result.ToString();
        }
    }
}