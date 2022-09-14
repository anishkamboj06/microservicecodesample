using System;
using System.Threading.Tasks;
using ConfigurationService.Models;
using ConfigurationService.Interfaces;
using ConfigurationService.Utility;
using System.Collections.Generic;
using System.Linq;
using Models;
using CommonLibrary.Utility;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace ConfigurationService.Services
{
    public class LocationService : ILocation
    {
        private DBGateway _DBGateway;
        private string Connection;
        public IConfiguration _configuration { get; }
        public LocationService(string _connection, IConfiguration configuration)
        {
            this.Connection = _connection;
            this._DBGateway = new DBGateway(_connection);
            _configuration = configuration;
        }

        #region "Private Function"
        /// <summary>
        /// Private function to add record in location code table when input CODE Field is not null.
        /// </summary>
        /// <param name="oLocationCodeModel"></param>
        /// <returns></returns>
        private async Task AddLocationCode(LocationModel oLocationModel)
        {
            // Add new record in location code mapping table

            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            Pars.Add("@LocationUid", oLocationModel.LocationUid);
            Pars.Add("@CodeId", oLocationModel.locationCodeMappingModel.CodeId);
            Pars.Add("@Code", oLocationModel.locationCodeMappingModel.Code);
            Pars.Add("@CodeDisplay", oLocationModel.locationCodeMappingModel.CodeDisplay);
            Pars.Add("@CodeValue", oLocationModel.locationCodeMappingModel.CodeValue);
            Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
            Pars.Add("@CreatedBy", oLocationModel.CreatedBy);
            Pars.Add("@CreatedDate", oLocationModel.CreatedDate);
            Pars.Add("@ModifiedBy", oLocationModel.ModifiedBy);
            Pars.Add("@ModifiedDate", oLocationModel.ModifiedDate);
            Pars.Add("@Source", oLocationModel.Source);
            Pars.Add("@Active", oLocationModel.Active);

            await _DBGateway.ExeScalarQuery<int>("insert into mp_location_code"
           + " (LocationUid,CodeId,Code,CodeDisplay,CodeValue,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Active,Source) "
           + " values(@LocationUid,@CodeId,@Code,@CodeDisplay,@CodeValue,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Active,@Source); "
           , Pars);
        }
        /// <summary>
        /// Private function to update record in location code table when input CODE Field is not null.
        /// </summary>
        /// <param name="oLocationCodeModel"></param>
        /// <returns></returns>
        private async Task UpdateLocationCode(LocationModel oLocationModel)
        {
            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            Pars.Add("@LocationUid", oLocationModel.LocationUid);

            //check if LocationUid exist in system
            // if exist then update code value else insert new record in mp_location_code table.
            int exist = await _DBGateway.ExeScalarQuery<int>("Select 1 from mp_location_code Where LocationUid=@LocationUid", Pars);

            Pars.Add("@CodeId", oLocationModel.locationCodeMappingModel.CodeId);
            Pars.Add("@Code", oLocationModel.locationCodeMappingModel.Code);
            Pars.Add("@CodeDisplay", oLocationModel.locationCodeMappingModel.CodeDisplay);
            Pars.Add("@CodeValue", oLocationModel.locationCodeMappingModel.CodeValue);
            Pars.Add("@ModifiedBy", oLocationModel.ModifiedBy);
            Pars.Add("@ModifiedDate", oLocationModel.ModifiedDate);
            Pars.Add("@Source", oLocationModel.Source);
            Pars.Add("@Active", oLocationModel.Active);
            // if exist then update code value
            if (exist > 0)
            {
                await _DBGateway.ExeScalarQuery<int>("update mp_location_code"
             + " set CodeId=@CodeId,Code=@Code,CodeDisplay=@CodeDisplay,CodeValue=@CodeValue,"
             + "ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source,Active=@Active"
             + " where LocationUid=@LocationUid; "
             , Pars);
            }
            else// add new record in table: mp_location_code
            {
                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@CreatedBy", oLocationModel.CreatedBy);
                Pars.Add("@CreatedDate", oLocationModel.CreatedDate);

                await _DBGateway.ExeScalarQuery<int>("insert into mp_location_code"
           + " (LocationUid,CodeId,Code,CodeDisplay,CodeValue,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Active,Source) "
           + " values(@LocationUid,@CodeId,@Code,@CodeDisplay,@CodeValue,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Active,@Source); "
           , Pars);
            }
        }
        #endregion


        #region "location contact detail add amd update Function"
        /// <summary>
        /// Private function to add record in location Contact Detail table when input Contact Detail Field is not null.
        /// </summary>
        /// <param name="LocationContactDetailMappingModel"></param>
        /// <returns></returns>
        private async Task AddLocationContactDetail(LocationContactDetailMappingModel oLocationModel, string locationUid)
        {
            // Add new record in location code mapping table

            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            Pars.Add("@LocationUid", locationUid);
            Pars.Add("@ContactPointUse", oLocationModel.ContactPointUse);
            Pars.Add("@ContactPointType", oLocationModel.ContactPointType);
            Pars.Add("@ContactPointValue", oLocationModel.ContactPointValue);
            Pars.Add("@ContactPointStatus", oLocationModel.ContactPointStatus);
            Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
            Pars.Add("@CreatedBy", oLocationModel.CreatedBy);
            Pars.Add("@CreatedDate", oLocationModel.CreatedDate);
            Pars.Add("@ModifiedBy", oLocationModel.CreatedBy);
            Pars.Add("@ModifiedDate", oLocationModel.CreatedDate);
            Pars.Add("@Source", oLocationModel.Source);
            await _DBGateway.ExeScalarQuery<int>("insert into mp_location_contact_detail"
           + " (LocationUid,ContactPointUse,ContactPointType,ContactPointValue,ContactPointStatus,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source) "
           + " values(@LocationUid,@ContactPointUse,@ContactPointType,@ContactPointValue,@ContactPointStatus,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Source); "
           , Pars);
        }
        /// <summary>
        /// Private function to update record in location Contact Detail table when input Contact Detail Field is not null.
        /// </summary>
        /// <param name="LocationContactDetailMappingModel"></param>
        /// <returns></returns>
        private async Task UpdateLocationContactDetail(LocationContactDetailMappingModel oLocationModel, string locationUid)
        {
            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            Pars.Add("@LocationUid", locationUid);
            Pars.Add("@Id", oLocationModel.Id);

            //check if LocationUid exist in system
            // if exist then update code value else insert new record in mp_location_code table.
            int exist = await _DBGateway.ExeScalarQuery<int>("Select 1 from mp_location_contact_detail Where LocationUid=@LocationUid and Id = @Id", Pars);

            Pars.Add("@ContactPointUse", oLocationModel.ContactPointUse);
            Pars.Add("@ContactPointType", oLocationModel.ContactPointType);
            Pars.Add("@ContactPointValue", oLocationModel.ContactPointValue);
            Pars.Add("@ContactPointStatus", oLocationModel.ContactPointStatus);
            Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
            Pars.Add("@CreatedBy", oLocationModel.CreatedBy);
            Pars.Add("@CreatedDate", oLocationModel.CreatedDate);
            Pars.Add("@ModifiedBy", oLocationModel.CreatedBy);
            Pars.Add("@ModifiedDate", oLocationModel.CreatedDate);
            Pars.Add("@Source", oLocationModel.Source);
            // if exist then update contact_detail
            if (exist > 0)
            {
                await _DBGateway.ExeScalarQuery<int>("update mp_location_contact_detail"
             + " set ContactPointUse=@ContactPointUse,ContactPointType=@ContactPointType,ContactPointValue=@ContactPointValue,ContactPointStatus=@ContactPointStatus,CreatedByType=@CreatedByType,CreatedBy=@CreatedBy,CreatedDate=@CreatedDate,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source"
            + " where LocationUid=@LocationUid and Id = @Id ; "
             , Pars);
            }
            else// add new record in table: mp_location_contact_detail
            {
                await _DBGateway.ExeScalarQuery<int>("insert into mp_location_contact_detail"
           + " (LocationUid,ContactPointUse,ContactPointType,ContactPointValue,ContactPointStatus,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source) "
           + " values(@LocationUid,@ContactPointUse,@ContactPointType,@ContactPointValue,@ContactPointStatus,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Source); "
           , Pars);
            }
        }

        private async Task<int> DeleteLocationContactDetail(long Id)
        {
            int MsgCode = 0;
            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            Pars.Add("@Id", Id);

            //check if LocationUid exist in system
            // if exist then update code value else insert new record in mp_location_code table.
            string locationUid = await _DBGateway.ExeScalarQuery<string>("Select LocationUid from mp_location_code Where Id = @Id", Pars);
            // if exist then update contact_detail
            if (string.IsNullOrEmpty(locationUid))
            {
                Pars.Add("@LocationUid", locationUid);
                MsgCode = await _DBGateway.ExeScalarQuery<int>("update mp_location_contact_detail set ContactPointStatus = false where LocationUid=@LocationUid ; "
             , Pars);
            }
            return MsgCode;
        }

        #endregion

        public async Task<ResultModel<object>> Delete(TokenModel oTokenModel, long Id)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                if (!checkLocationExistance(Id))
                {
                    Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                    Pars.Add("@Id", Id);
                    Pars.Add("@CreatedById", oTokenModel.LoginId);
                    Result.MsgCode = await _DBGateway.ExeQuery("DELETE FROM md_location WHERE Id=@Id", Pars) == 1 ? 1 : 2;
                    if (Result.MsgCode == 1)
                    {
                        Result.Message = Constants.DELETE_MESSAGE;

                        await DeleteLocationContactDetail(Id);
                    }

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
        public async Task<ResultModel<object>> GetById(TokenModel oTokenModel, string LocationUid)
        {

            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pLocationUid", LocationUid);
                LocationModel locationModel = await _DBGateway.ExeSPScaler<LocationModel>("sp_md_location_getbyid", Pars);
                if (locationModel != null)
                {
                    // Fetch HealthRecords from blob
                    if (!string.IsNullOrEmpty(locationModel.ImagePath))
                    {
                        var healthRecordPath = locationModel.ImagePath.Split("/");                        
                        locationModel.ImagePath = await StorageService.GetObject(healthRecordPath[2] + "/" + healthRecordPath[3] + "/" + healthRecordPath[4]);                        
                    }
                    locationModel.locationContactDetailMappingModel = new List<LocationContactDetailMappingModel>();
                    locationModel.locationContactDetailMappingModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocationContactDetailMappingModel>>(locationModel.LocationContactDetail ?? "[]");
                }
                Result.Model = locationModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<LocationModel>> GetAll(TokenModel oTokenModel, LocationSearchModel oSearchModel)
        {
            ResultModel<LocationModel> Result = new ResultModel<LocationModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Skip", oSearchModel.Skip);
                Pars.Add("@Take", oSearchModel.ItemsPerPage);
                Pars.Add("@SearchValue", oSearchModel.SearchValue);
                Pars.Add("@TypeCode", oSearchModel.TypeCode);
                Pars.Add("@StateCode", oSearchModel.StateCode);
                Pars.Add("@DistrictCode", oSearchModel.DistrictCode);
                Pars.Add("@TalukaCode", oSearchModel.TalukaCode);
                Pars.Add("@CityCode", oSearchModel.CityCode);
                Pars.Add("@Active", oSearchModel.Active);
                Result.LstModel = await _DBGateway.ExeSPList<LocationModel>("sp_md_location_getall", Pars);

                if (Result.LstModel != null)
                {
                    Result.TotalRecords = Result.LstModel.FirstOrDefault() != null ? Result.LstModel.FirstOrDefault().TotalRecords : 0;
                    //foreach (var item in Result.LstModel)
                    //{
                    //    item.locationContactDetailMappingModel = new List<LocationContactDetailMappingModel>();
                    //    item.locationContactDetailMappingModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocationContactDetailMappingModel>>(item.LocationContactDetail ?? "[]");
                    //}

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationModel oLocationModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Name", oLocationModel.Name);
                Pars.Add("@NIN", oLocationModel.NIN); // nin_to_hfi
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select 1 from md_location Where Name=@Name or NIN=@NIN", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }

                Pars.Add("@Id", oLocationModel.Id);
                var uniqueNumber = Helper.GenerateUniqueNumber();
                oLocationModel.LocationUid = uniqueNumber;
                Pars.Add("@LocationUid", uniqueNumber);
                Pars.Add("@OrganizationUid", oLocationModel.OrganizationUid);
                Pars.Add("@TypeCode", oLocationModel.TypeCode);
                Pars.Add("@TypeDisplay", oLocationModel.TypeDisplay);

                // get max priority value from table and make new prority as Old Max Priority PLUS 50.
                long maxPriority = await _DBGateway.ExeScalarQuery<long>("Select MAX(Priority) FROM md_location");
                Pars.Add("@Priority", maxPriority + 50);
                Pars.Add("@ImagePath", "");
                //Pars.Add("@ImagePath", oLocationModel.ImagePath);

                #region Upload HCF Image on blob
                // Validate the consultation files and store into the blob storage  
                var bolbRefrence = string.Empty;
                if (oLocationModel.healthCareFacilityProfile != null)
                {
                    var isValidate = Helper.CheckValidFileUplaod(oLocationModel.healthCareFacilityProfile.FilePath, oLocationModel.healthCareFacilityProfile.FileName, oLocationModel.healthCareFacilityProfile.FileExt);
                    if (isValidate.Success)
                    {
                        //Create the blobRefrencePath for save into Ecounter table                            
                        bolbRefrence = _configuration.GetSection("Azure:HCFContainerName").Value + oLocationModel.LocationUid + "/" + oLocationModel.healthCareFacilityProfile.FileName;
                        var imageUploadResult = await StorageService.UploadObject(Convert.FromBase64String(oLocationModel.healthCareFacilityProfile.FilePath), bolbRefrence);
                        if (imageUploadResult.Success)
                        {
                            oLocationModel.ImagePath = "/" + _configuration.GetSection("Azure:ContainerName").Value + bolbRefrence;
                            Pars.Add("@ImagePath", oLocationModel.ImagePath);
                        }
                    }
                    else
                    {
                        Result.MsgCode = Constants.INVALIDFILE;
                        Result.Message = Constants.INVALIDFILE_MESSAGE + oLocationModel.healthCareFacilityProfile.FileName;
                        return Result;
                    }
                }
                #endregion

                if (oLocationModel.LocationSubTypeCode == 0)
                {
                    // If LocationSubTypeCode is not provided from UI then LocationSubTypeCode = 10001 and  LocationSubTypeDisplay is 'Not Applicable as per ms_locationsubtype Table  
                    Pars.Add("@LocationSubTypeCode", "10001"); //HFi_Type
                    Pars.Add("@LocationSubTypeDisplay", "Not Applicable"); //// phc_chc_type
                }
                else
                {
                    Pars.Add("@LocationSubTypeCode", oLocationModel.LocationSubTypeCode); //HFi_Type
                    Pars.Add("@LocationSubTypeDisplay", oLocationModel.LocationSubTypeDisplay); //// phc_chc_type
                }
                Pars.Add("@AddressUse", oLocationModel.AddressUse);
                Pars.Add("@AddressType", oLocationModel.AddressType);
                Pars.Add("@AddressLine1", oLocationModel.AddressLine1);
                Pars.Add("@AddressLine2", oLocationModel.AddressLine2);
                Pars.Add("@CountryCode", oLocationModel.CountryCode);
                Pars.Add("@CountryDisplay", oLocationModel.CountryDisplay);
                Pars.Add("@StateCode", oLocationModel.StateCode);
                Pars.Add("@StateDisplay", oLocationModel.StateDisplay);
                Pars.Add("@DistrictCode", oLocationModel.DistrictCode);
                Pars.Add("@DistrictDisplay", oLocationModel.DistrictDisplay);
                Pars.Add("@CityCode", oLocationModel.CityCode);
                Pars.Add("@CityDisplay", oLocationModel.CityDisplay);
                Pars.Add("@MandalType", oLocationModel.MandalType);
                Pars.Add("@MandalCode", oLocationModel.MandalCode);
                Pars.Add("@MandalDisplay", oLocationModel.MandalDisplay);
                Pars.Add("@BlockCode", oLocationModel.BlockCode);
                Pars.Add("@BlockDisplay", oLocationModel.BlockDisplay);
                Pars.Add("@SecretariatCode", oLocationModel.SecretariatCode);
                Pars.Add("@SecretariatDisplay", oLocationModel.SecretariatDisplay);
                Pars.Add("@TalukaCode", oLocationModel.TalukaCode);
                Pars.Add("@TalukaDisplay", oLocationModel.TalukaDisplay);
                Pars.Add("@PostalCode", oLocationModel.PostalCode);
                Pars.Add("@RegionIndicator", oLocationModel.RegionIndicator);
                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@CreatedBy", oTokenModel.LoginId);
                Pars.Add("@CreatedDate", oLocationModel.CreatedDate);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", oLocationModel.CreatedDate);
                Pars.Add("@Source", oLocationModel.Source);
                Pars.Add("@Active", oLocationModel.Active);
                Pars.Add("@NINName", oLocationModel.NINName);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into md_location "
                + " (Id,LocationUid,Name,OrganizationUid,TypeCode,TypeDisplay,Priority,ImagePath, "
                + "LocationSubTypeCode,LocationSubTypeDisplay,AddressUse,AddressType, AddressLine1, "
                + "AddressLine2,CountryCode,CountryDisplay,StateCode,StateDisplay,DistrictCode,DistrictDisplay,CityCode,CityDisplay,MandalType, "
                + "MandalCode,MandalDisplay,BlockCode,BlockDisplay,SecretariatCode,SecretariatDisplay,TalukaCode,TalukaDisplay,PostalCode, "
                + "RegionIndicator,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active,"
                + "NIN,NINName) values "

                + "(@Id,@LocationUid,@Name,@OrganizationUid,@TypeCode,@TypeDisplay,@Priority,@ImagePath, "
                + "@LocationSubTypeCode,@LocationSubTypeDisplay,@AddressUse,@AddressType,@AddressLine1, "
                + "@AddressLine2,@CountryCode,@CountryDisplay,@StateCode,@StateDisplay,@DistrictCode,@DistrictDisplay,@CityCode,@CityDisplay,@MandalType, "
                + "@MandalCode,@MandalDisplay,@BlockCode,@BlockDisplay,@SecretariatCode,@SecretariatDisplay,@TalukaCode,@TalukaDisplay,@PostalCode, "
                + "@RegionIndicator,@CreatedByType,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Source,@Active,"
                + "@NIN,@NINName); "
                + "select 1"
                , Pars);

                if (res == 0)
                {
                    Result.Message = Constants.NOTCREATED_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                }
                else
                {
                    //if code is not null or empty then add new record in mp_location_code table
                    if (!string.IsNullOrEmpty(oLocationModel.locationCodeMappingModel.Code))
                    {
                        oLocationModel.LocationUid = uniqueNumber;
                        // Add new record in location code mapping table
                        await AddLocationCode(oLocationModel);
                    }

                    // call insert service 
                    if (oLocationModel.lstDepartmentModel != null)
                    {
                        LocationDepartmentService oLocationDepartmentService = new LocationDepartmentService(Connection);
                        foreach (var insert in oLocationModel.lstDepartmentModel)
                        {
                            insert.LocationUid = uniqueNumber;

                            await oLocationDepartmentService.Insert(oTokenModel, insert, oLocationModel.Name);
                        }
                    }

                    //if Contact detail list is not null or empty then add new record in mp_location_contact_detail table
                    if (oLocationModel.locationContactDetailMappingModel != null && oLocationModel.locationContactDetailMappingModel.Count > 0)
                    {
                        oLocationModel.LocationUid = uniqueNumber;
                        // Add new record in location code mapping table
                        foreach (LocationContactDetailMappingModel item in oLocationModel.locationContactDetailMappingModel)
                        {
                            // Get Vlaues from location model and fill all location details in mapping model
                            item.CreatedByType = CreatedByType.Admin.ToString();
                            item.CreatedBy = oLocationModel.CreatedBy;
                            item.CreatedDate = oLocationModel.CreatedDate;
                            item.ModifiedBy = oLocationModel.ModifiedBy;
                            item.ModifiedDate = oLocationModel.ModifiedDate;
                            item.Source = oLocationModel.Source;
                            await AddLocationContactDetail(item, oLocationModel.LocationUid);
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
        public async Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationModel oLocationModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", oLocationModel.Id);
                Pars.Add("@Name", oLocationModel.Name);
                Pars.Add("@NIN", oLocationModel.NIN); // nin_to_hfi
                //check if name already exist in system
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select 1 from md_location Where (Name=@Name or NIN=@NIN) and Id<>@Id", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }
                Pars.Add("@LocationUid", oLocationModel.LocationUid);
                // Pars.Add("@OrganizationUid", oLocationModel.OrganizationUid);
                Pars.Add("@TypeCode", oLocationModel.TypeCode);
                Pars.Add("@TypeDisplay", oLocationModel.TypeDisplay);
                Pars.Add("@ImagePath", "");
                #region Update Image Profile and delete in case not exist in payload
                var bolbRefrence = string.Empty;
                if (oLocationModel.healthCareFacilityProfile != null && (!string.IsNullOrEmpty(oLocationModel.healthCareFacilityProfile.FilePath)))                
                {
                    var isValidate = Helper.CheckValidFileUplaod(oLocationModel.healthCareFacilityProfile.FilePath, oLocationModel.healthCareFacilityProfile.FileName, oLocationModel.healthCareFacilityProfile.FileExt);
                    if (isValidate.Success)
                    {
                        bolbRefrence = _configuration.GetSection("Azure:HCFContainerName").Value + oLocationModel.LocationUid + "/" + oLocationModel.healthCareFacilityProfile.FileName;
                        var imageUploadResult = await StorageService.UploadObject(Convert.FromBase64String(oLocationModel.healthCareFacilityProfile.FilePath), bolbRefrence);
                        if (imageUploadResult.Success)
                        {
                            HealthCareFacilityProfile oHealthCareFacilityProfile = new HealthCareFacilityProfile();
                            oHealthCareFacilityProfile.FilePath = "/" + _configuration.GetSection("Azure:ContainerName").Value + bolbRefrence;
                            Pars.Add("@ImagePath", oHealthCareFacilityProfile.FilePath);
                        }
                    }
                    else
                    {
                        Result.Success = false;
                        Result.MsgCode = Constants.INVALIDFILE;
                        Result.Message = Constants.INVALIDFILE_MESSAGE;
                        return Result;
                    }
                }
                else
                {
                    var model = await _DBGateway.ExeScalarQuery<LocationModel>("Select * from md_location Where LocationUid="+oLocationModel.LocationUid, Pars);
                    Pars.Add("@ImagePath", model.ImagePath);
                }
                //else// delete profile image from blob reference
                //{
                //    if (Result.Model. != null && itemBody.profileImageModel.ProfileImageBase64 != null)
                //    {
                //        // Get Image path from profileImageModel and split it as file name
                //        string blobProfileImagePath = itemBody.profileImageModel.ProfileImageBase64;
                //        var imageUploadResult = await StorageService.RemoveObject(blobProfileImagePath.Split("/")[2] + "/" + blobProfileImagePath.Split("/")[3] + "/" + blobProfileImagePath.Split("/")[4]);
                //        if (imageUploadResult.Success)
                //        {
                //            itemBody.profileImageModel.ProfileImageBase64 = null;
                //        }
                //    }

                //}
                #endregion                
                if (oLocationModel.LocationSubTypeCode == 0)
                {
                    // If LocationSubTypeCode is not provided from UI then LocationSubTypeCode = 10001 and  LocationSubTypeDisplay is 'Not Applicable as per ms_locationsubtype Table  
                    Pars.Add("@LocationSubTypeCode", "10001"); //HFi_Type
                    Pars.Add("@LocationSubTypeDisplay", "Not Applicable"); //// phc_chc_type
                }
                else
                {
                    Pars.Add("@LocationSubTypeCode", oLocationModel.LocationSubTypeCode);
                    Pars.Add("@LocationSubTypeDisplay", oLocationModel.LocationSubTypeDisplay);
                }
                Pars.Add("@AddressUse", oLocationModel.AddressUse);
                Pars.Add("@AddressType", oLocationModel.AddressType);
                Pars.Add("@AddressLine1", oLocationModel.AddressLine1);
                Pars.Add("@AddressLine2", oLocationModel.AddressLine2);
                Pars.Add("@CountryCode", oLocationModel.CountryCode);
                Pars.Add("@CountryDisplay", oLocationModel.CountryDisplay);
                Pars.Add("@StateCode", oLocationModel.StateCode);
                Pars.Add("@StateDisplay", oLocationModel.StateDisplay);
                Pars.Add("@DistrictCode", oLocationModel.DistrictCode);
                Pars.Add("@DistrictDisplay", oLocationModel.DistrictDisplay);
                Pars.Add("@CityCode", oLocationModel.CityCode);
                Pars.Add("@CityDisplay", oLocationModel.CityDisplay);
                Pars.Add("@MandalType", oLocationModel.MandalType);
                Pars.Add("@MandalCode", oLocationModel.MandalCode);
                Pars.Add("@MandalDisplay", oLocationModel.MandalDisplay);
                Pars.Add("@BlockCode", oLocationModel.BlockCode);
                Pars.Add("@BlockDisplay", oLocationModel.BlockDisplay);
                Pars.Add("@SecretariatCode", oLocationModel.SecretariatCode);
                Pars.Add("@SecretariatDisplay", oLocationModel.SecretariatDisplay);
                Pars.Add("@TalukaCode", oLocationModel.TalukaCode);
                Pars.Add("@TalukaDisplay", oLocationModel.TalukaDisplay);
                Pars.Add("@PostalCode", oLocationModel.PostalCode);
                Pars.Add("@RegionIndicator", oLocationModel.RegionIndicator);
                Pars.Add("@ModifiedBy", oTokenModel.LoginId);
                Pars.Add("@ModifiedDate", oLocationModel.ModifiedDate);
                Pars.Add("@Source", oLocationModel.Source);
                Pars.Add("@Active", oLocationModel.Active);
                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                Pars.Add("@NINName", oLocationModel.NINName);

                var res = await _DBGateway.ExeScalarQuery<int>("update md_location "
                + "set LocationUid=@LocationUid,Name=@Name,TypeCode=@TypeCode,TypeDisplay=@TypeDisplay,ImagePath=@ImagePath, "
                + "LocationSubTypeCode=@LocationSubTypeCode,LocationSubTypeDisplay=@LocationSubTypeDisplay,AddressUse=@AddressUse,AddressType=@AddressType,AddressLine1=@AddressLine1, "
                + "AddressLine2=@AddressLine2,CountryCode=@CountryCode,CountryDisplay=@CountryDisplay,StateCode=@StateCode,StateDisplay=@StateDisplay,DistrictCode=@DistrictCode,DistrictDisplay=@DistrictDisplay,CityCode=@CityCode,CityDisplay=@CityDisplay,MandalType=@MandalType, "
                + "MandalCode=@MandalCode,MandalDisplay=@MandalDisplay,BlockCode=@BlockCode,BlockDisplay=@BlockDisplay,SecretariatCode=@SecretariatCode,SecretariatDisplay=@SecretariatDisplay,TalukaCode=@TalukaCode,TalukaDisplay=@TalukaDisplay,PostalCode=@PostalCode, "
                + "RegionIndicator=@RegionIndicator,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Source=@Source,Active=@Active,"
                + "NIN=@NIN,CreatedByType=@CreatedByType,NINName=@NINName"
                + " where Id=@Id;"
                + "select 1"
                , Pars);

                if (res == 0)
                {
                    Result.Message = Constants.NOTUPDATED_MESSAGE;
                    Result.MsgCode = Constants.NOTUPDATED;
                }
                else
                {
                    //if code is not null or empty then update code value in LocationCode table by LocationUid
                    if (!string.IsNullOrEmpty(oLocationModel.locationCodeMappingModel.Code))
                    {
                        // Update record in location code mapping table
                        await UpdateLocationCode(oLocationModel);
                    }

                    //if code is not null or empty then update code value in location Contact Detail table by LocationUid
                    if (oLocationModel.locationContactDetailMappingModel != null && oLocationModel.locationContactDetailMappingModel.Count > 0)
                    {
                        // Update record in location Contact Detail mapping table
                        foreach (LocationContactDetailMappingModel item in oLocationModel.locationContactDetailMappingModel)
                        {
                            item.CreatedByType = CreatedByType.Admin.ToString();
                            item.CreatedBy = oLocationModel.CreatedBy;
                            item.CreatedDate = oLocationModel.CreatedDate;
                            item.ModifiedBy = oLocationModel.ModifiedBy;
                            item.ModifiedDate = oLocationModel.ModifiedDate;
                            item.Source = oLocationModel.Source;
                            await UpdateLocationContactDetail(item, oLocationModel.LocationUid);
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
        public async Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string LocationUid)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@LocationUid", LocationUid);
                //get active status value from table
                bool active = await _DBGateway.ExeScalarQuery<bool>("Select Active from md_location Where LocationUid=@LocationUid", Pars);

                // if active status is true then make it false, else vice versa.
                Pars.Add("@Active", !active);

                var res = await _DBGateway.ExeQuery("update md_location "
                + "set Active=@Active "
                + "where LocationUid=@LocationUid;"
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
        public async Task<ResultModel<object>> GetLocationsByParentOrgId(TokenModel oTokenModel, string OrgUid)
        {

            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@OrgUid", OrgUid);
                Result.LstModel = await _DBGateway.ExeQueryList<object>("Select LocationUid, Name from md_location where OrganizationUid=@OrgUid", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<object>> GetLocationsByParentOrgIdAndType(TokenModel oTokenModel, LocationTypeOrganizationMapping oLocationTypeOrganizationMapping)
        {

            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pOrganizationUid", oLocationTypeOrganizationMapping.OrganizationUid);
                Pars.Add("@pTypeCode", oLocationTypeOrganizationMapping.TypeCode);
                Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_getlocation_by_orguidandloctype", Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> GetActiveLocations(TokenModel oTokenModel)
        {
            ResultModel<object> resultModel = new ResultModel<object>();
            try
            {
                resultModel.LstModel = await _DBGateway.ExeQueryList<object>("Select Id,LocationUid,Name from md_location where Active = true;");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultModel;
        }
        public async Task<ResultModel<object>> GetActiveLocationsOpds(TokenModel oTokenModel)
        {
            ResultModel<object> resultModel = new ResultModel<object>();
            try
            {
                resultModel.LstModel = await _DBGateway.ExeQueryList<object>("Select Id,LocationUid,Name from md_location where Active = true and TypeDisplay = 'OPD';");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultModel;
        }
        public async Task<ResultModel<int>> GetActiveTotalLocations(TokenModel oTokenModel)
        {
            ResultModel<int> resultModel = new ResultModel<int>();
            try
            {
                resultModel.TotalRecords = resultModel.Model = await _DBGateway.ExeScalarQuery<int>("Select count(*) from md_location where Active = true;");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resultModel;
        }
        public async Task<ResultModel<LocNetworkDistanceResponseModel>> GetLocationsBySearchParameters(TokenModel oTokenModel, LocationSearchModelByOrgUId oLocationSearchModelByOrgUId)
        {
            ResultModel<LocNetworkDistanceResponseModel> Result = new ResultModel<LocNetworkDistanceResponseModel>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Skip", oLocationSearchModelByOrgUId.Skip);
                Pars.Add("@Take", oLocationSearchModelByOrgUId.ItemsPerPage);
                Pars.Add("@SearchValue", oLocationSearchModelByOrgUId.SearchValue);
                Pars.Add("@StateCode", oLocationSearchModelByOrgUId.StateCode);
                Pars.Add("@DistrictCode", oLocationSearchModelByOrgUId.DistrictCode);
                Pars.Add("@CityCode", oLocationSearchModelByOrgUId.CityCode);
                Pars.Add("@OrganizationUid", oLocationSearchModelByOrgUId.OrganizationUid);
                Pars.Add("@TypeCode", oLocationSearchModelByOrgUId.TypeCode);

                Result.LstModel = await _DBGateway.ExeSPList<LocNetworkDistanceResponseModel>("sp_md_location_getall_by_searchparam", Pars);

                if (Result.LstModel != null)
                {
                    Result.TotalRecords = Result.LstModel.FirstOrDefault() != null ? Result.LstModel.FirstOrDefault().TotalRecords : 0;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        private bool checkLocationExistance(long Id)
        {
            bool status = false;
            Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
            pars.Add("@Id", Id);
            // check existance in location department table
            var existInDepartment = _DBGateway.ExeScalarQuery<int>("SELECT 1 FROM mp_location_department WHERE LocationUid = (SELECT LocationUid FROM md_location WHERE Id = @Id LIMIT 1) LIMIT 1", pars);
            if (existInDepartment != null && existInDepartment.Result == 1)
            {
                status = true;
            }
            else
            {
                // check existance in network table
                var existInNetwork = _DBGateway.ExeScalarQuery<int>("SELECT 1 FROM mp_location_network WHERE LocationUid = (SELECT LocationUid FROM md_location WHERE Id = @Id LIMIT 1) LIMIT 1", pars);
                if (existInNetwork != null && existInNetwork.Result == 1)
                {
                    status = true;
                }
                else
                {
                    // check existance in practitioner table
                    var existInPractitioner = _DBGateway.ExeScalarQuery<int>("SELECT 1 FROM mp_location_department_practitioner WHERE LocationUid = (SELECT LocationUid FROM md_location WHERE Id = @Id LIMIT 1) LIMIT 1", pars);
                    if (existInPractitioner != null && existInPractitioner.Result == 1)
                    {
                        status = true;
                    }
                    else
                    {
                        // check existance in state department table
                        var existInState = _DBGateway.ExeScalarQuery<int>("SELECT 1 FROM mp_location_department_state WHERE LocationDepartmentUid = (SELECT LocationDepartmentUid FROM mp_location_department WHERE LocationUid = (SELECT LocationUid FROM md_location WHERE Id = @Id LIMIT 1) LIMIT 1) LIMIT 1", pars);
                        if (existInState != null && existInState.Result == 1)
                        {
                            status = true;
                        }
                    }
                }
            }
            return status;
        }
        public async Task<ResultModel<object>> GetLocationForConsultation(TokenModel oTokenModel, string LocationUid)
        {

            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pLocationUid", LocationUid);
                LocationModel locationModel = await _DBGateway.ExeSPScaler<LocationModel>("sp_md_location_getbyid", Pars);
                if (locationModel != null)
                {
                    // Fetch HealthRecords from blob
                    //if (!string.IsNullOrEmpty(locationModel.ImagePath))
                    //{
                    //    var healthRecordPath = locationModel.ImagePath.Split("/");
                    //    locationModel.ImagePath = await StorageService.GetObject(healthRecordPath[2] + "/" + healthRecordPath[3] + "/" + healthRecordPath[4]);
                    //}
                    locationModel.locationContactDetailMappingModel = new List<LocationContactDetailMappingModel>();
                    locationModel.locationContactDetailMappingModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocationContactDetailMappingModel>>(locationModel.LocationContactDetail ?? "[]");
                }
                Result.Model = locationModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
    }
}