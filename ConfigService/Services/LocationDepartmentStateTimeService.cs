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
    public class LocationDepartmentStateTimeService : ILocationDepartmentStateTime
    {
        #region "SetUp"
        private DBGateway _DBGateway;
        public LocationDepartmentStateTimeService(string _connection)
        {
            this._DBGateway = new DBGateway(_connection);
        }
        #endregion

        public async Task<ResultModel<object>> GetAllLocationDepartmentStateTime(TokenModel oTokenModel, LocationDepartmentStateTimeSearchModel oSearchModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            Result.LstModel = new List<object>();
            // Result.Message = Constants.NORECORDFOUND_MESSAGE;
            //Result.MsgCode = Constants.RECORDNOTEXIST;

            List<object> LstModel = new List<object>();
            try
            {
                if (!string.IsNullOrEmpty(oSearchModel.LocationDepartmentStateUids))
                {
                    //case 1 : multiple locationDepartmentState uids
                    foreach (var oLocationDepartmentStateUid in oSearchModel.LocationDepartmentStateUids.Split(","))
                    {
                        // call commmon function that loop over Raoster model and insert value to db
                        LstModel = await GetAllLocationDeptStateTimeCommon(oLocationDepartmentStateUid, oSearchModel);
                        Result.LstModel.AddRange(LstModel);
                    }
                }
                else if (!string.IsNullOrEmpty(oSearchModel.LocationDepartmentUids))
                {
                    // case : mulitple location department uids, so we will get all LocationDepartmentStateUids from LocationDepartmentState by LocationDepartmentUid s
                    foreach (var oLocationDepartmentUid in oSearchModel.LocationDepartmentUids.Split(","))
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
                                LstModel = await GetAllLocationDeptStateTimeCommon(oLocationDepartmentStateUid, oSearchModel);
                                Result.LstModel.AddRange(LstModel);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(oSearchModel.LocationUids))
                {
                    // get all dept and then states for location and insert for all
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
                                        // call commmon function that loop over Raoster model and insert value to db
                                        LstModel = await GetAllLocationDeptStateTimeCommon(oLocationDepartmentStateUid, oSearchModel);
                                        Result.LstModel.AddRange(LstModel);

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

        private async Task<List<object>> GetAllLocationDeptStateTimeCommon(string oLocationDepartmentStateUid, LocationDepartmentStateTimeSearchModel oSearchModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@pSkip", oSearchModel.Skip);
                Pars.Add("@pTake", oSearchModel.ItemsPerPage);
                Pars.Add("@pLocationDepartmentStateUid", oLocationDepartmentStateUid);
                Result.LstModel = await _DBGateway.ExeSPList<object>("sp_mp_location_department_state_time_getall", Pars);
                if (Result.LstModel != null && Result.LstModel.Count > 0)
                {
                    var data = ((IDictionary<string, object>)Result.LstModel.FirstOrDefault());
                    Result.TotalRecords = data != null && data["TotalRecords"] != null ? Convert.ToInt32(data["TotalRecords"]) : 0;
                }
                else
                {
                    Result.Message = Constants.NORECORDFOUND_MESSAGE;
                    Result.MsgCode = Constants.RECORDNOTEXIST;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result.LstModel;
        }


        /*
         * This method insert the Roaster timing Values for multiple States with multiple Departments and multiple locations possible
         */
        public async Task<ResultModel<object>> InsertMultiple(TokenModel oTokenModel, LocationDepartmentStateTimeModel oLocationDepartmentStateTimeModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            Result.Message = Constants.NORECORDFOUND_MESSAGE;
            //  Result.MsgCode = Constants.RECORDNOTEXIST;
            try
            {
                if (!string.IsNullOrEmpty(oLocationDepartmentStateTimeModel.LocationDepartmentStateUids))
                {
                    //case 1 : multiple locationDepartmentState uids
                    foreach (var oLocationDepartmentStateUid in oLocationDepartmentStateTimeModel.LocationDepartmentStateUids.Split(","))
                    {
                        // call commmon function that loop over Raoster model and insert value to db
                        await InsertRoasterCommon(oLocationDepartmentStateUid, oLocationDepartmentStateTimeModel, Result, oLocationDepartmentStateTimeModel.RosterType);
                    }
                }
                else if (!string.IsNullOrEmpty(oLocationDepartmentStateTimeModel.LocationDepartmentUids))
                {
                    // case : mulitple location department uids, so we will get all LocationDepartmentStateUids from LocationDepartmentState by LocationDepartmentUid s
                    foreach (var oLocationDepartmentUid in oLocationDepartmentStateTimeModel.LocationDepartmentUids.Split(","))
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
                                await InsertRoasterCommon(oLocationDepartmentStateUid, oLocationDepartmentStateTimeModel, Result, oLocationDepartmentStateTimeModel.RosterType);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(oLocationDepartmentStateTimeModel.LocationUids))
                {
                    // get all dept and then states for location and insert for all
                    // loop over location ids to get depts uids
                    foreach (var oLocationUid in oLocationDepartmentStateTimeModel.LocationUids.Split(","))
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
                                        await InsertRoasterCommon(oLocationDepartmentStateUid, oLocationDepartmentStateTimeModel, Result, oLocationDepartmentStateTimeModel.RosterType);

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

        /*
         * common method that inserts roaster values into db 
         */
        private async Task<ResultModel<object>> InsertRoasterCommon(string oLocationDepartmentStateUid, LocationDepartmentStateTimeModel oLocationDepartmentStateTimeModel, ResultModel<object> Result, string RosterType)
        {
            try
            {
                Dapper.DynamicParameters Pars = new Dapper.DynamicParameters();
                Pars.Add("@LocationDepartmentStateUid", oLocationDepartmentStateUid);

                var resp = await _DBGateway.ExeQuery("delete from mp_location_department_state_time "
                + " where LocationDepartmentStateUid=@LocationDepartmentStateUid;"
                , Pars);

                foreach (LocationDepartmentStateTimeRoasterModel locationDepartmentStateTimeRoaster in oLocationDepartmentStateTimeModel.ListLocationDepartmentStateTimeRoasterModel)
                {
                    // there might be list of roasterTime like multiple slots for one day
                    foreach (RoasterTime roasterTime in locationDepartmentStateTimeRoaster.RoasterTime)
                    {
                        var regex = @"^(0[1-9]|1[0-2]):[0-5][0-9] (am|pm|AM|PM)$"; // 10:00 AM
                        var match = Regex.Match(roasterTime.StartTime, regex, RegexOptions.IgnoreCase);
                        var match2 = Regex.Match(roasterTime.EndTime, regex, RegexOptions.IgnoreCase);
                        if (!match.Success || !match2.Success)
                        {
                            Result.Success = false;
                            Result.Message = "RoasterTime format is not valid";
                            Result.MsgCode = Constants.VALIDATION_ERROR;
                            return Result;
                        }

                        // ignore if not selected as Active
                        if (!locationDepartmentStateTimeRoaster.Active)
                        {
                            continue;
                        }
                        // add Days * 5(week code 1 to 5 ) for insert and update in weekly case
                        if (RosterType.ToLower() == "weekly")
                        {
                            int weekCode = 1;
                            for (weekCode = 1; weekCode <= 5; weekCode++)
                            {
                                locationDepartmentStateTimeRoaster.WeekCode = weekCode;

                                #region save to DB insert 
                                Pars = new Dapper.DynamicParameters();
                                Pars.Add("@LocationDepartmentStateTimeUid", Helper.GenerateUniqueNumber());
                                Pars.Add("@LocationDepartmentStateUid", oLocationDepartmentStateUid);
                                //check if name already exist in system
                                Result.Model = await _DBGateway.ExeScalarQuery<object>("Select 1 from mp_location_department_state_time Where LocationDepartmentStateTimeUid=@LocationDepartmentStateTimeUid and LocationDepartmentStateUid=@LocationDepartmentStateUid", Pars);
                                if (Result.Model != null)
                                {
                                    //Result.Model = null;
                                    //Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                                    //Result.MsgCode = Constants.NOTCREATED;
                                    //return Result;
                                    continue;
                                }
                                //Pars.Add("@Id", oLocationDepartmentStateTimeModel.Id);
                                Pars.Add("@StartDate", DateTime.UtcNow);
                                Pars.Add("@EndDate", DateTime.UtcNow);
                                Pars.Add("@StartTime", roasterTime.StartTime);
                                Pars.Add("@EndTime", roasterTime.EndTime);
                                Pars.Add("@WeekCode", locationDepartmentStateTimeRoaster.WeekCode);
                                Pars.Add("@DayDisplay", locationDepartmentStateTimeRoaster.DayDisplay);
                                Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                                Pars.Add("@CreatedBy", locationDepartmentStateTimeRoaster.CreatedBy);
                                Pars.Add("@CreatedDate", locationDepartmentStateTimeRoaster.CreatedDate);
                                Pars.Add("@ModifiedBy", locationDepartmentStateTimeRoaster.ModifiedBy);
                                Pars.Add("@ModifiedDate", locationDepartmentStateTimeRoaster.ModifiedDate);
                                Pars.Add("@Source", locationDepartmentStateTimeRoaster.Source);
                                Pars.Add("@Active", locationDepartmentStateTimeRoaster.Active);

                                var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_location_department_state_time "
                                + " (LocationDepartmentStateTimeUid,LocationDepartmentStateUid,StartDate,EndDate,StartTime,EndTime,WeekCode,DayDisplay, "
                                + "CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active) values "

                                + "(@LocationDepartmentStateTimeUid,@LocationDepartmentStateUid,@StartDate,@EndDate,@StartTime,@EndTime,@WeekCode,@DayDisplay, "
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
                                #endregion
                            }

                        }
                        else
                        {
                            #region save to DB insert 
                            Pars = new Dapper.DynamicParameters();
                            Pars.Add("@LocationDepartmentStateTimeUid", Helper.GenerateUniqueNumber());
                            Pars.Add("@LocationDepartmentStateUid", oLocationDepartmentStateUid);
                            //check if name already exist in system
                            Result.Model = await _DBGateway.ExeScalarQuery<object>("Select 1 from mp_location_department_state_time Where LocationDepartmentStateTimeUid=@LocationDepartmentStateTimeUid and LocationDepartmentStateUid=@LocationDepartmentStateUid", Pars);
                            if (Result.Model != null)
                            {
                                //Result.Model = null;
                                //Result.Message = Constants.ALREADY_EXISTS_MESSAGE;
                                //Result.MsgCode = Constants.NOTCREATED;
                                //return Result;
                                continue;
                            }
                            //Pars.Add("@Id", oLocationDepartmentStateTimeModel.Id);
                            Pars.Add("@StartDate", DateTime.UtcNow);
                            Pars.Add("@EndDate", DateTime.UtcNow);
                            Pars.Add("@StartTime", roasterTime.StartTime);
                            Pars.Add("@EndTime", roasterTime.EndTime);
                            Pars.Add("@WeekCode", locationDepartmentStateTimeRoaster.WeekCode);
                            Pars.Add("@DayDisplay", locationDepartmentStateTimeRoaster.DayDisplay);
                            Pars.Add("@CreatedByType", CreatedByType.Admin.ToString());
                            Pars.Add("@CreatedBy", locationDepartmentStateTimeRoaster.CreatedBy);
                            Pars.Add("@CreatedDate", locationDepartmentStateTimeRoaster.CreatedDate);
                            Pars.Add("@ModifiedBy", locationDepartmentStateTimeRoaster.ModifiedBy);
                            Pars.Add("@ModifiedDate", locationDepartmentStateTimeRoaster.ModifiedDate);
                            Pars.Add("@Source", locationDepartmentStateTimeRoaster.Source);
                            Pars.Add("@Active", locationDepartmentStateTimeRoaster.Active);

                            var res = await _DBGateway.ExeScalarQuery<int>("insert into mp_location_department_state_time "
                            + " (LocationDepartmentStateTimeUid,LocationDepartmentStateUid,StartDate,EndDate,StartTime,EndTime,WeekCode,DayDisplay, "
                            + "CreatedByType,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate,Source,Active) values "

                            + "(@LocationDepartmentStateTimeUid,@LocationDepartmentStateUid,@StartDate,@EndDate,@StartTime,@EndTime,@WeekCode,@DayDisplay, "
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
                            #endregion
                        }

                    }

                }
                return Result;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /*
         * This method insert the Roaster timing Values for multiple States with multiple Departments and multiple locations possible
         */
        public async Task<ResultModel<object>> UpdateMultiple(TokenModel oTokenModel, LocationDepartmentStateTimeModel oLocationDepartmentStateTimeModel)
        {
            ResultModel<object> Result = new ResultModel<object>();
            try
            {
                if (!string.IsNullOrEmpty(oLocationDepartmentStateTimeModel.LocationDepartmentStateUids))
                {
                    for (int i = 0; i < oLocationDepartmentStateTimeModel.ListLocationDepartmentStateTimeRoasterModel.Count; i++)
                    {
                        var startTime = oLocationDepartmentStateTimeModel.ListLocationDepartmentStateTimeRoasterModel[i];
                        var endTime = oLocationDepartmentStateTimeModel.ListLocationDepartmentStateTimeRoasterModel[i];
                        var test = startTime.RoasterTime.GroupBy(x => x.StartTime)
                       .Where(g => g.Count() > 1)
                       .Select(y => y.Key)
                       .ToList();
                        if (test.Count > 0)
                        {
                            Result.Success = false;
                            Result.Message = Constants.ROASTERTIME_VALIDATION_MESSAGE;
                            Result.MsgCode = Constants.ROASTERTIME_VALIDATION;
                            return Result;
                        }
                    }

                        //case 1 : multiple locationDepartmentState uids
                        foreach (var oLocationDepartmentStateUid in oLocationDepartmentStateTimeModel.LocationDepartmentStateUids.Split(","))
                    {
                        // call commmon function that loop over Raoster model and insert value to db
                        await UpdateRoasterCommon(oLocationDepartmentStateUid, oLocationDepartmentStateTimeModel, Result);
                    }
                }
                else if (!string.IsNullOrEmpty(oLocationDepartmentStateTimeModel.LocationDepartmentUids))
                {
                    // case : mulitple location department uids, so we will get all LocationDepartmentStateUids from LocationDepartmentState by LocationDepartmentUid s
                    foreach (var oLocationDepartmentUid in oLocationDepartmentStateTimeModel.LocationDepartmentUids.Split(","))
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
                                await UpdateRoasterCommon(oLocationDepartmentStateUid, oLocationDepartmentStateTimeModel, Result);
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(oLocationDepartmentStateTimeModel.LocationUids))
                {
                    // get all dept and then states for location and insert for all
                    // loop over location ids to get depts uids
                    foreach (var oLocationUid in oLocationDepartmentStateTimeModel.LocationUids.Split(","))
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
                                        await UpdateRoasterCommon(oLocationDepartmentStateUid, oLocationDepartmentStateTimeModel, Result);

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

        /*
        * common method that update roaster values into db 
        */
        private async Task<ResultModel<object>> UpdateRoasterCommon(string oLocationDepartmentStateUid, LocationDepartmentStateTimeModel oLocationDepartmentStateTimeModel, ResultModel<object> Result)
        {
            try
            {
                //foreach (LocationDepartmentStateTimeRoasterModel locationDepartmentStateTimeRoaster in oLocationDepartmentStateTimeModel.ListLocationDepartmentStateTimeRoasterModel)
                //{
                //    // remove all roasters first and then call insert roaster 
                //    try
                //    {
                //        Dapper.DynamicParameters Parms = new Dapper.DynamicParameters();
                //        Parms.Add("@LocationDepartmentStateUid", oLocationDepartmentStateUid);

                //        var resp = await _DBGateway.ExeQuery("delete from mp_location_department_state_time "
                //        + " where LocationDepartmentStateUid=@LocationDepartmentStateUid;"
                //        , Parms);
                //    }
                //    catch (Exception ex)
                //    {
                //        throw;
                //    }
                //}
                // insert th roasters
                await InsertRoasterCommon(oLocationDepartmentStateUid, oLocationDepartmentStateTimeModel, Result, oLocationDepartmentStateTimeModel.RosterType);

                return Result;
            }
            catch (Exception)
            {

                throw;
            }

        }



    }
}