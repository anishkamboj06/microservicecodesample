using CommonLibrary.Utility;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using Grpc.Net.Client;
using Models;
using Newtonsoft.Json;
using PractitionerService.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationService.Services
{
    public class PractitionerDepartmentService : IPractitionerDepartment
    {
        private DBGateway _DBGateway;
        public PractitionerDepartmentService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }
        public async Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string PractitionerProviderDepartmentUid)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@PractitionerProviderDepartmentUid", PractitionerProviderDepartmentUid);
                //get active status value from table
                bool active = await _DBGateway.ExeScalarQuery<bool>("Select Active from mp_location_department_practitioner Where PractitionerProviderDepartmentUid=@PractitionerProviderDepartmentUid", Pars);

                // if active status is true then make it false, else vice versa.
                Pars.Add("@Active", !active);

                var res = await _DBGateway.ExeQuery("update mp_location_department_practitioner "
                + "set Active=@Active "
                + "where PractitionerProviderDepartmentUid=@PractitionerProviderDepartmentUid;"
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
        public async Task<ResultModel<DepartmentPractitionerMappingFromMasterModel>> InsertPractitionerProviderLocationDepartmentMapping(TokenModel oTokenModel, List<PractitionerDepartmentModel> oPractitionerDepartmentModel, string PractitionerServiceUpdateDeptPractEndPoint)
        {
            ResultModel<DepartmentPractitionerMappingFromMasterModel> Result = new ResultModel<DepartmentPractitionerMappingFromMasterModel>();
            try
            {

                // Master list of DepartmentPractitionerMapping
                List<DepartmentPractitionerMappingFromMasterModel> masterDepartmentPractitionerMappingFromMasterModel = new List<DepartmentPractitionerMappingFromMasterModel>();
                // Get the list of result of created DepartmentPractitionerMapping
                var saveDepartmentPractitionerMappingResult = Insert(oTokenModel, oPractitionerDepartmentModel).Result;
                if (saveDepartmentPractitionerMappingResult.MsgCode == 1)
                {
                    //Fetch Organization detail from table
                    var organization = await GetOrganizationByLocationUId(saveDepartmentPractitionerMappingResult.LstModel[0].LocationUid);
                    GetOrganizationNameTypeByLocUId getOrganizationByLocationUId = new GetOrganizationNameTypeByLocUId() { OrganizationUid = organization.OrganizationUid, OrganizationDisplay = organization.OrganizationDisplay };
                    if (saveDepartmentPractitionerMappingResult != null && saveDepartmentPractitionerMappingResult.MsgCode == Constants.SUCCESS)
                        foreach (var departmentPractitioner in saveDepartmentPractitionerMappingResult.LstModel)
                        {
                            // PractitionerDepartmentModel details
                            DepartmentPractitionerMappingFromMasterModel practitionerDepartmentMapping = new DepartmentPractitionerMappingFromMasterModel();
                            practitionerDepartmentMapping.PractitionerUid = departmentPractitioner.PractitionerProviderUid;
                            practitionerDepartmentMapping.Type = departmentPractitioner.Type;
                            practitionerDepartmentMapping.FunctionalType = departmentPractitioner.FunctionalType;
                            practitionerDepartmentMapping.DepartmentPractitionerUid = departmentPractitioner.PractitionerProviderDepartmentUid;
                            practitionerDepartmentMapping.LocDepartmentUid = departmentPractitioner.LocationDepartmentUid;
                            practitionerDepartmentMapping.LocDepartmentName = departmentPractitioner.LocationDepartmentName;
                            practitionerDepartmentMapping.LocationUid = departmentPractitioner.LocationUid;
                            practitionerDepartmentMapping.LocationName = departmentPractitioner.LocationName;
                            practitionerDepartmentMapping.PractitionerProviderUid = departmentPractitioner.PractitionerProviderUid;
                            practitionerDepartmentMapping.ModifiedBy = departmentPractitioner.ModifiedBy;
                            practitionerDepartmentMapping.ModifiedDate = departmentPractitioner.ModifiedDate;
                            practitionerDepartmentMapping.OrganizationUid = getOrganizationByLocationUId.OrganizationUid;
                            practitionerDepartmentMapping.OrganizationName = getOrganizationByLocationUId.OrganizationDisplay;

                            masterDepartmentPractitionerMappingFromMasterModel.Add(practitionerDepartmentMapping);
                        }

                    //Convert masterDepartmentPractitionerMappingFromMasterModel Object to Json string 
                    string jsonData = JsonConvert.SerializeObject(masterDepartmentPractitionerMappingFromMasterModel);
                    using var channel = GrpcChannel.ForAddress(PractitionerServiceUpdateDeptPractEndPoint);
                    var client = new PractitionerProtoService.PractitionerProtoServiceClient(channel);

                    //Call the Practitioner service to update practitionerDepartmentMapping api using gRPC call                  
                    var update = await client.UpdateDepartmentPractitionerListAsync(
                                    new UpdateDepartmentPractitionerListRequest
                                    {
                                        Request = jsonData
                                    });
                    Result.Message = update.Reply;
                }
                else
                {
                    Result.Message = saveDepartmentPractitionerMappingResult.Message;
                    Result.MsgCode = saveDepartmentPractitionerMappingResult.MsgCode;
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                throw ex;
            }
            return Result;
        }
        public async Task<ResultModel<PractitionerDepartmentModel>> Insert(TokenModel oTokenModel, List<PractitionerDepartmentModel> lstPractitionerDepartmentModel)
        {
            // Master list of PractitionerDepartmentModel
            List<PractitionerDepartmentModel> masterPractitionerDepartmentModel = new List<PractitionerDepartmentModel>();
            ResultModel<PractitionerDepartmentModel> Result = new ResultModel<PractitionerDepartmentModel>();

            try
            {
                foreach (var item in lstPractitionerDepartmentModel)
                {
                    if (!await CheckSpokeMappingCount(item.PractitionerProviderUid, item.LocationDepartmentUid))
                    {
                        // Set parameter values in model
                        PractitionerDepartmentModel practitionerDepartmentModel = new PractitionerDepartmentModel();
                        practitionerDepartmentModel.PractitionerProviderDepartmentUid = Helper.GenerateUniqueNumber();
                        practitionerDepartmentModel.PractitionerProviderUid = item.PractitionerProviderUid;
                        practitionerDepartmentModel.LocationDepartmentUid = item.LocationDepartmentUid;
                        practitionerDepartmentModel.LocationDepartmentName = item.LocationDepartmentName;
                        practitionerDepartmentModel.LocationUid = item.LocationUid;
                        practitionerDepartmentModel.LocationName = item.LocationName;
                        practitionerDepartmentModel.Type = item.Type;
                        practitionerDepartmentModel.FunctionalType = item.FunctionalType;
                        practitionerDepartmentModel.CreatedByType = CreatedByType.Admin.ToString();
                        practitionerDepartmentModel.CreatedBy = item.CreatedBy;
                        practitionerDepartmentModel.CreatedDate = item.CreatedDate;
                        practitionerDepartmentModel.ModifiedBy = item.ModifiedBy;
                        practitionerDepartmentModel.ModifiedDate = item.ModifiedDate;
                        practitionerDepartmentModel.Source = item.Source;
                        practitionerDepartmentModel.Active = item.Active;

                        // Set parameter values in dynamic parameter for save the record in db
                        Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                        Pars.Add("@pPractitionerProviderDepartmentUid", practitionerDepartmentModel.PractitionerProviderDepartmentUid);
                        Pars.Add("@pPractitionerProviderUid", item.PractitionerProviderUid);
                        Pars.Add("@pLocationDepartmentUid", item.LocationDepartmentUid);
                        Pars.Add("@pLocationDepartmentName", item.LocationDepartmentName);
                        Pars.Add("@pLocationUid", item.LocationUid);
                        Pars.Add("@pLocationName", item.LocationName);
                        Pars.Add("@pType", item.Type);
                        Pars.Add("@pFunctionalType", item.FunctionalType);
                        Pars.Add("@pCreatedByType", CreatedByType.Admin.ToString());
                        Pars.Add("@pCreatedBy", item.CreatedBy);
                        Pars.Add("@pCreatedDate", item.CreatedDate);
                        Pars.Add("@pModifiedBy", item.ModifiedBy);
                        Pars.Add("@pModifiedDate", item.ModifiedDate);
                        Pars.Add("@pSource", item.Source);
                        Pars.Add("@pActive", item.Active);

                        var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_location_department_practitioner"
                            + " (PractitionerProviderDepartmentUid,PractitionerProviderUid,LocationDepartmentUid,LocationDepartmentName,LocationUid,LocationName,Type,FunctionalType,CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active) "
                            + " values(@pPractitionerProviderDepartmentUid,@pPractitionerProviderUid,@pLocationDepartmentUid,@pLocationDepartmentName,@pLocationUid,@pLocationName,@pType,@pFunctionalType,@pCreatedByType,@pCreatedBy,@pCreatedDate,@pModifiedBy,@pModifiedDate,@pSource,@pActive); "
                            + " select 1 "
                            , Pars).ConfigureAwait(true);

                        if (res == 0)
                        {
                            Result.Message = Constants.NOTCREATED_MESSAGE;
                            Result.MsgCode = Constants.NOTCREATED;
                        }
                        else
                        {
                            // Bind Created PractitionerDepartment in lst model                       
                            masterPractitionerDepartmentModel.Add(practitionerDepartmentModel);
                            Result.MsgCode = Constants.SUCCESS;
                            Result.Message = Constants.CREATED_MESSAGE;
                        }
                    }
                    else
                    {
                        Result.MsgCode = Constants.ALREADY_MAPPED;
                        Result.Message = Constants.ALREADY_MAPPED_MESSAGE;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Result.LstModel = masterPractitionerDepartmentModel;

            return Result;
        }
        public async Task<ResultModel<object>> Delete(TokenModel oTokenModel, string practitionerProviderDepartmentUid, string PractitionerServiceUnmapDepartmentPractitionerMapping)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@PractitionerProviderDepartmentUid", practitionerProviderDepartmentUid);
                // Get Location practitioner Mapping Data using ID and then do delete process
                PractitionerDepartmentModel practitionerDepartmentModel = await _DBGateway.ExeScalarQuery<PractitionerDepartmentModel>("SELECT * FROM mp_location_department_practitioner Where PractitionerProviderDepartmentUid=@PractitionerProviderDepartmentUid", Pars);
                Result.MsgCode = await _DBGateway.ExeQuery("DELETE FROM mp_location_department_practitioner Where PractitionerProviderDepartmentUid=@PractitionerProviderDepartmentUid", Pars) == 1 ? 1 : 2;
                if (Result.MsgCode == 1)
                {
                    Result.Message = Constants.DELETE_MESSAGE;
                    // CALL Practitioner Service to remove Mapped Departments.
                    DepartmentPractitionerMappingModel departmentPractitionerMappingModel = new DepartmentPractitionerMappingModel();
                    departmentPractitionerMappingModel.DepartmentPractitionerUid = practitionerDepartmentModel.PractitionerProviderDepartmentUid;
                    departmentPractitionerMappingModel.LocDepartmentUid = practitionerDepartmentModel.LocationDepartmentUid;
                    departmentPractitionerMappingModel.LocDepartmentName = practitionerDepartmentModel.LocationDepartmentName;
                    departmentPractitionerMappingModel.LocationUid = practitionerDepartmentModel.LocationUid;
                    departmentPractitionerMappingModel.LocationName = practitionerDepartmentModel.LocationName;
                    departmentPractitionerMappingModel.PractitionerProviderUid = practitionerDepartmentModel.PractitionerProviderUid;

                    //Convert departmentPractitionerMappingModel Object as Json string 
                    string jsonData = JsonConvert.SerializeObject(departmentPractitionerMappingModel);
                    using var channel = GrpcChannel.ForAddress(PractitionerServiceUnmapDepartmentPractitionerMapping);
                    var client = new PractitionerProtoService.PractitionerProtoServiceClient(channel);

                    //Call the Practitioner service to delet or UnmapDepartmentPractitionerMapping api using gRPC call
                    var update = await client.UnmapDepartmentPractitionerMappingAsync(
                                    new UnmapDepartmentPractitionerMappingRequest
                                    {
                                        Request = jsonData
                                    });
                    Result.Message = update.Reply;
                }
                else
                {
                    Result.MsgCode = Constants.NOTDELETED;
                    Result.Message = Constants.NOTDELETE_MESSAGE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        private async Task<GetOrganizationNameTypeByLocUId> GetOrganizationByLocationUId(string locUId)
        {
            Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
            Pars.Add("@locUId", locUId);
            List<GetOrganizationNameTypeByLocUId> organizationName = await _DBGateway.ExeSPList<GetOrganizationNameTypeByLocUId>("sp_get_Org_By_LocUId", Pars);
            return organizationName[0];
        }

        private async Task<bool> CheckSpokeMappingCount(string PractitionerProviderUid, string LocationUid)
        {
            bool spokeMapping = false;
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@practitionerProviderUid", PractitionerProviderUid);
                Pars.Add("@locationUid", LocationUid);
                string query = $"SELECT COUNT(*) FROM mp_location_department_practitioner WHERE PractitionerProviderUid = {PractitionerProviderUid} AND FunctionalType != 'OPD'";
                int existance = await _DBGateway.ExeScalarQuery<int>(query, Pars);

                spokeMapping = existance > 0;
            }
            catch
            {
            }
            return spokeMapping;
        }
    }
}