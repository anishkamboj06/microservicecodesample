using CommonLibrary.Utility;
using ConfigurationService.Interfaces;
using ConfigurationService.Models;
using ConfigurationService.Utility;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConfigurationService.Services
{
    public class LocationDepartmentNonOperationalService : ILocationDepartmentNonOperational
    {
        #region "SetUp"
        private DBGateway _DBGateway;
        public LocationDepartmentNonOperationalService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }
        #endregion
        public async Task<ResultModel<object>> getAllByDate(TokenModel oTokenModel, LocationDepartmentNonOperationalSearchByDateModel oSearchModel)
        {

            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pStartDate", oSearchModel.StartDate);
                Pars.Add("@pEndDate", oSearchModel.EndDate);
                Pars.Add("@pActive", oSearchModel.Active);
                string queryString = "SELECT nonOper.Message,  nonOper.LocationDepartmentNonOpUid, loc.LocationUid,ld.LocationDepartmentUid,  nonOper.LocationDepartmentStateUid,nonOper.StartDate,nonOper.StartTime,"
                                   + " nonOper.EndTime,nonOper.EndDate,nonOper.IsFullDay,loc.Name as HealthFaciltyName,lds.StateDisplay,ld.DepartmentDisplay "
                                   + " FROM "
                                   + " mp_location_department_nonoperational nonOper"
                                   + " left join mp_location_department_state lds on lds.LocationDepartmentStateUid=nonOper.LocationDepartmentStateUid"
                                   + " left join mp_location_department ld on ld.LocationDepartmentUid=lds.LocationDepartmentUid"
                                   + " left join md_location loc on loc.LocationUid = ld.LocationUid"
                                   + " WHERE loc.Active =true and DATE(StartDate) >= date(@pStartDate) AND DATE(EndDate) <= date(@pEndDate);";
                Result.LstModel = await _DBGateway.ExeQueryList<object>(queryString, Pars);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> GetAllLocationDepartmentNonOperational(TokenModel oTokenModel, LocationDepartmentNonOperationalSearchModel oSearchModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            string strLocationDepartmentStateUid = "";
            try
            {
                if (!string.IsNullOrEmpty(oSearchModel.LocationDepartmentStateUids))
                {
                    strLocationDepartmentStateUid = oSearchModel.LocationDepartmentStateUids;
                    //calling GetAllRoaster
                    await GetAllRoasterTimeOffCommon(strLocationDepartmentStateUid, oSearchModel, Result);
                }
                else if (!string.IsNullOrEmpty(oSearchModel.LocationDepartmentUids))
                {
                    //for mulitple location department uids, in the case we will get all LocationDepartmentStateUids from LocationDepartmentState by LocationDepartmentUids
                    foreach (var oLocationDepartmentUid in oSearchModel.LocationDepartmentUids.Split(","))
                    {
                        Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                        pars.Add("@LocationDepartmentUid", oLocationDepartmentUid);

                        List<string> LocationDeptStateUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentStateUid from mp_location_department_state Where LocationDepartmentUid=@LocationDepartmentUid", pars);
                        if (LocationDeptStateUids != null && LocationDeptStateUids.Count() > 0)
                        {
                            // getting all the LocationDepartmentStateUids
                            foreach (var oLocationDepartmentStateUid in LocationDeptStateUids)
                            {
                                strLocationDepartmentStateUid += oLocationDepartmentStateUid + ",";
                            }
                        }
                    }
                    if (strLocationDepartmentStateUid.EndsWith(","))
                    {
                        strLocationDepartmentStateUid = strLocationDepartmentStateUid.Remove(strLocationDepartmentStateUid.Length - 1);
                    }
                    //calling GetAllRoaster
                    await GetAllRoasterTimeOffCommon(strLocationDepartmentStateUid, oSearchModel, Result);
                }
                else if (!string.IsNullOrEmpty(oSearchModel.LocationUids))
                {
                    // get all dept and then states for location and get for all
                    // loop over location ids to get depts uids
                    foreach (var oLocationUid in oSearchModel.LocationUids.Split(","))
                    {
                        Dapper.DynamicParameters parm = new Dapper.DynamicParameters();
                        parm.Add("@LocationUid", oLocationUid);
                        // get depts uids
                        List<string> LocationDeptUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentUid from mp_location_department Where LocationUid=@LocationUid", parm);
                        if (LocationDeptUids != null && LocationDeptUids.Count() > 0)
                        {
                            // loop over deptIds to get State uids
                            foreach (var oLocationDepartmentUid in LocationDeptUids)
                            {
                                Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                                pars.Add("@LocationDepartmentUid", oLocationDepartmentUid);
                                // get state Uids
                                List<string> LocationDeptStateUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentStateUid from mp_location_department_state Where LocationDepartmentUid=@LocationDepartmentUid", pars);
                                if (LocationDeptStateUids != null && LocationDeptStateUids.Count() > 0)
                                {
                                    // insert for every dept state uid
                                    foreach (var oLocationDepartmentStateUid in LocationDeptStateUids)
                                    {
                                        strLocationDepartmentStateUid += oLocationDepartmentStateUid + ",";
                                    }
                                }
                            }
                        }
                    }
                    if (strLocationDepartmentStateUid.EndsWith(","))
                    {
                        strLocationDepartmentStateUid = strLocationDepartmentStateUid.Remove(strLocationDepartmentStateUid.Length - 1);
                    }
                    //calling GetAllRoaster
                    await GetAllRoasterTimeOffCommon(strLocationDepartmentStateUid, oSearchModel, Result);
                }
                else
                {
                    await GetAllRoasterTimeOffCommon(strLocationDepartmentStateUid, oSearchModel, Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }
        private async Task<ResultModel<object>> GetAllRoasterTimeOffCommon(string oLocationDepartmentStateUids, LocationDepartmentNonOperationalSearchModel oSearchModel, ResultModel<object> Result)
        {
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Skip", oSearchModel.Skip);
                Pars.Add("@Take", oSearchModel.ItemsPerPage);
                Pars.Add("@LocationDepartmentStateUid", oSearchModel.LocationDepartmentStateUids);
                Pars.Add("@Id", oSearchModel.Id ?? 0);
                Pars.Add("@pStartDate", oSearchModel.StartDate);
                Pars.Add("@pEndDate", oSearchModel.EndDate);

                Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_department_nonoperational_getall", Pars);
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

        //This method is used to insert the timing off values for multiple states with multiple departments and multiple locations possible
        public async Task<ResultModel<object>> Insert(TokenModel oTokenModel, LocationDepartmentNonOperationalModelTimeOff oLocationDepartmentNonOperationalModelTimeOff)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                if (!string.IsNullOrEmpty(oLocationDepartmentNonOperationalModelTimeOff.LocationDepartmentStateUids))
                {
                    //case 1 : multiple locationDepartmentState uids
                    foreach (var oLocationDepartmentStateUid in oLocationDepartmentNonOperationalModelTimeOff.LocationDepartmentStateUids.Split(","))
                    {
                        // call commmon function that loop over Raoster model and insert value to db
                        await InsertRoasterTimeOffCommon(oLocationDepartmentStateUid, oLocationDepartmentNonOperationalModelTimeOff, Result);
                    }
                }
                else if (!string.IsNullOrEmpty(oLocationDepartmentNonOperationalModelTimeOff.LocationDepartmentUids))
                {
                    // case : mulitple location department uids, so we will get all LocationDepartmentStateUids from LocationDepartmentState by LocationDepartmentUid s
                    foreach (var oLocationDepartmentUid in oLocationDepartmentNonOperationalModelTimeOff.LocationDepartmentUids.Split(","))
                    {
                        Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                        pars.Add("@LocationDepartmentUid", oLocationDepartmentUid);

                        List<string> LocationDeptStateUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentStateUid from mp_location_department_state Where LocationDepartmentUid=@LocationDepartmentUid", pars);
                        if (LocationDeptStateUids != null && LocationDeptStateUids.Count() > 0)
                        {
                            // insert for every dept state uid
                            foreach (var oLocationDepartmentStateUid in LocationDeptStateUids)
                            {
                                // call commmon function that loop over Raoster model and insert value to db
                                await InsertRoasterTimeOffCommon(oLocationDepartmentStateUid, oLocationDepartmentNonOperationalModelTimeOff, Result);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(oLocationDepartmentNonOperationalModelTimeOff.LocationUids))
                {
                    // get all dept and then states for location and insert for all
                    // loop over location ids to get depts uids
                    foreach (var oLocationUid in oLocationDepartmentNonOperationalModelTimeOff.LocationUids.Split(","))
                    {
                        Dapper.DynamicParameters parm = new Dapper.DynamicParameters();
                        parm.Add("@LocationUid", oLocationUid);
                        // get depts uids
                        List<string> LocationDeptUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentUid from mp_location_department Where LocationUid=@LocationUid", parm);
                        if (LocationDeptUids != null && LocationDeptUids.Count() > 0)
                        {
                            // loop over deptIds to get State uids
                            foreach (var oLocationDepartmentUid in LocationDeptUids)
                            {
                                Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                                pars.Add("@LocationDepartmentUid", oLocationDepartmentUid);
                                // get state Uids
                                List<string> LocationDeptStateUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentStateUid from mp_location_department_state Where LocationDepartmentUid=@LocationDepartmentUid", pars);
                                if (LocationDeptStateUids != null && LocationDeptStateUids.Count() > 0)
                                {
                                    // insert for every dept state uid
                                    foreach (var oLocationDepartmentStateUid in LocationDeptStateUids)
                                    {
                                        // call commmon function that loop over Raoster model and insert value to db
                                        await InsertRoasterTimeOffCommon(oLocationDepartmentStateUid, oLocationDepartmentNonOperationalModelTimeOff, Result);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        //common method that inserts time off values into db
        private async Task<ResultModel<object>> InsertRoasterTimeOffCommon(string oLocationDepartmentStateUid, LocationDepartmentNonOperationalModelTimeOff oLocationDepartmentNonOperationalModelTimeOff, ResultModel<object> Result)
        {
            try
            {
                var regex = @"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$"; // 10:00  12 hour time format
                var match = Regex.Match(oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.StartTime, regex, RegexOptions.IgnoreCase);
                var match2 = Regex.Match(oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.EndTime, regex, RegexOptions.IgnoreCase);

                if (!match.Success || !match2.Success)
                {
                    Result.Success = false;
                    Result.Message = "Roaster Time Off format is not valid";
                    Result.MsgCode = Constants.VALIDATION_ERROR;
                    return Result;
                }

                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();

                Pars.Add("@LocationDepartmentNonOpUid", Helper.GenerateUniqueNumber());
                Pars.Add("@LocationDepartmentStateUid", oLocationDepartmentStateUid);
                // Pars.Add("@LocationUid", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.LocationUid);
                //check if name already exist in system
                //Result.Model = await _DBGateway.ExeScalarQuery<object>("Select * from mp_location_department_nonoperational Where LocationDepartmentNonOpUid=@LocationDepartmentNonOpUid", Pars);
                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select 1 from mp_location_department_nonoperational Where LocationDepartmentNonOpUid=@LocationDepartmentNonOpUid and LocationDepartmentStateUid=@LocationDepartmentStateUid", Pars);
                if (Result.Model != null)
                {
                    Result.Model = null;
                    Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                    Result.MsgCode = Constants.NOTCREATED;
                    return Result;
                }

                //Pars.Add("@DepartmentCode", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.DepartmentCode);
                // Pars.Add("@DepartmentDisplay", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.DepartmentDisplay);
                Pars.Add("@StartDate", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.StartDate);
                Pars.Add("@EndDate", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.EndDate);
                Pars.Add("@StartTime", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.StartTime);
                Pars.Add("@EndTime", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.EndTime);
                Pars.Add("@Message", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.Message);
                //Pars.Add("@CreatedByType", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.CreatedByType);
                Pars.Add("@CreatedBy", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.CreatedBy);
                Pars.Add("@CreatedDate", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.CreatedDate);
                Pars.Add("@ModifiedBy", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.ModifiedBy);
                Pars.Add("@ModifiedDate", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.CreatedDate);
                Pars.Add("@Source", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.Source);
                Pars.Add("@Active", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.Active);
                Pars.Add("@IsFullDay", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.IsFullDay);

                var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_location_department_nonoperational "
                + " (LocationDepartmentNonOpUid,StartDate,EndDate,StartTime,EndTime,Message, "
                + "CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active,LocationDepartmentStateUid, IsFullDay) values "

                + "(@LocationDepartmentNonOpUid,@StartDate,@EndDate,@StartTime,@EndTime,@Message, "
                + "@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Source,@Active,@LocationDepartmentStateUid, @IsFullDay);"
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

        //This method is used to update the timing off values for multiple states with multiple departments and multiple locations possible
        public async Task<ResultModel<object>> Update(TokenModel oTokenModel, LocationDepartmentNonOperationalModelTimeOff oLocationDepartmentNonOperationalModelTimeOff)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                if (!string.IsNullOrEmpty(oLocationDepartmentNonOperationalModelTimeOff.LocationDepartmentStateUids))
                {
                    //case 1 : multiple locationDepartmentState uids
                    foreach (var oLocationDepartmentStateUid in oLocationDepartmentNonOperationalModelTimeOff.LocationDepartmentStateUids.Split(","))
                    {
                        // call commmon function that loop over Raoster model and insert value to db
                        await UpdateRoasterTimeOffCommon(oLocationDepartmentStateUid, oLocationDepartmentNonOperationalModelTimeOff, Result);
                    }
                }
                else if (!string.IsNullOrEmpty(oLocationDepartmentNonOperationalModelTimeOff.LocationDepartmentUids))
                {
                    // case : mulitple location department uids, so we will get all LocationDepartmentStateUids from LocationDepartmentState by LocationDepartmentUid s
                    foreach (var oLocationDepartmentUid in oLocationDepartmentNonOperationalModelTimeOff.LocationDepartmentUids.Split(","))
                    {
                        Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                        pars.Add("@LocationDepartmentUid", oLocationDepartmentUid);

                        List<string> LocationDeptStateUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentStateUid from mp_location_department_state Where LocationDepartmentUid=@LocationDepartmentUid", pars);
                        if (LocationDeptStateUids != null && LocationDeptStateUids.Count() > 0)
                        {
                            // insert for every dept state uid
                            foreach (var oLocationDepartmentStateUid in LocationDeptStateUids)
                            {
                                // call commmon function that loop over Raoster model and insert value to db
                                await UpdateRoasterTimeOffCommon(oLocationDepartmentStateUid, oLocationDepartmentNonOperationalModelTimeOff, Result);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(oLocationDepartmentNonOperationalModelTimeOff.LocationUids))
                {
                    // get all dept and then states for location and insert for all
                    // loop over location ids to get depts uids
                    foreach (var oLocationUid in oLocationDepartmentNonOperationalModelTimeOff.LocationUids.Split(","))
                    {
                        Dapper.DynamicParameters parm = new Dapper.DynamicParameters();
                        parm.Add("@LocationUid", oLocationUid);
                        // get depts uids
                        List<string> LocationDeptUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentUid from mp_location_department Where LocationUid=@LocationUid", parm);
                        if (LocationDeptUids != null && LocationDeptUids.Count() > 0)
                        {
                            // loop over deptIds to get State uids
                            foreach (var oLocationDepartmentUid in LocationDeptUids)
                            {
                                Dapper.DynamicParameters pars = new Dapper.DynamicParameters();
                                pars.Add("@LocationDepartmentUid", oLocationDepartmentUid);
                                // get state Uids
                                List<string> LocationDeptStateUids = await _DBGateway.ExeQueryList<string>("Select LocationDepartmentStateUid from mp_location_department_state Where LocationDepartmentUid=@LocationDepartmentUid", pars);
                                if (LocationDeptStateUids != null && LocationDeptStateUids.Count() > 0)
                                {
                                    // insert for every dept state uid
                                    foreach (var oLocationDepartmentStateUid in LocationDeptStateUids)
                                    {
                                        // call commmon function that loop over Raoster model and insert value to db
                                        await UpdateRoasterTimeOffCommon(oLocationDepartmentStateUid, oLocationDepartmentNonOperationalModelTimeOff, Result);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        //common method that update time off values into db
        private async Task<ResultModel<object>> UpdateRoasterTimeOffCommon(string oLocationDepartmentStateUid, LocationDepartmentNonOperationalModelTimeOff oLocationDepartmentNonOperationalModelTimeOff, ResultModel<object> Result)
        {
            try
            {
                var regex = @"^(?:[01][0-9]|2[0-3]):[0-5][0-9]$"; // 10:00  24 hour time format
                var match = Regex.Match(oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.StartTime, regex, RegexOptions.IgnoreCase);
                var match2 = Regex.Match(oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.EndTime, regex, RegexOptions.IgnoreCase);

                if (!match.Success || !match2.Success)
                {
                    Result.Success = false;
                    Result.Message = "Roaster Time Off format is not valid";
                    Result.MsgCode = Constants.VALIDATION_ERROR;
                    return Result;
                }

                Dapper.DynamicParameters Parms = new Dapper.DynamicParameters();
                Parms.Add("@LocationDepartmentStateUid", oLocationDepartmentStateUid);
                Parms.Add("@LocationDepartmentNonOpUid", oLocationDepartmentNonOperationalModelTimeOff.locationDepartmentNonOperationalModel.LocationDepartmentNonOpUid);

                var isExist = await _DBGateway.ExeScalarQuery<int>("select 1 from mp_location_department_nonoperational "
                + " where LocationDepartmentStateUid=@LocationDepartmentStateUid and LocationDepartmentNonOpUid = @LocationDepartmentNonOpUid;"
                , Parms);

                if (isExist == 1)
                {
                    // delete the entries and insert new one 
                    var resp = await _DBGateway.ExeQuery("delete from mp_location_department_nonoperational "
                    + " where LocationDepartmentStateUid=@LocationDepartmentStateUid and LocationDepartmentNonOpUid = @LocationDepartmentNonOpUid;"
                    , Parms);

                    // insert th roasters
                    await InsertRoasterTimeOffCommon(oLocationDepartmentStateUid, oLocationDepartmentNonOperationalModelTimeOff, Result);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Result;
        }

        // This method is used to delete the Time Off values by Id
        public async Task<ResultModel<object>> DeleteLocationDepartmentNonOperational(TokenModel oTokenModel, string LocDepartNonOpUid, bool IsHardDelete = true)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                foreach (var item in LocDepartNonOpUid.Split(','))
                {
                    Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                    Pars.Add("@LocationDepartmentNonOpUid", item);
                    {
                        Pars.Add("@CreatedById", oTokenModel.LoginId);
                        // dynamic query for hard delete and soft delete, it depends on the para.
                        string query = IsHardDelete ? "delete from mp_location_department_nonoperational where LocationDepartmentNonOpUid in (@LocationDepartmentNonOpUid)" :
                            "update mp_location_department_nonoperational set Active=false Where LocationDepartmentNonOpUid in (@LocationDepartmentNonOpUid)";

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }

        public async Task<ResultModel<object>> GetLocationDepartmentNonOperationalById(long id)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@Id", id);
                Result.Model = await _DBGateway.ExeSPScaler<object>("sp_mp_location_department_nonoperational_getbyId", Pars);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
    }
}
