using CommonLibrary.Utility; 
using ConfigurationService.Utility;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;

namespace ConfigurationService.Controllers
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Return the Current User Data from Reading the JWT Token
        /// </summary> 
        public TokenModel Me
        {
            get
            {
                try
                {
                    TokenModel oTokenModel = new TokenModel();
                    oTokenModel = Helper.GetIdentity(Request.Headers["Authorization"].ToString());
                    return oTokenModel;
                }
                catch (Exception ex)
                {
                    return new TokenModel();
                }
            }
        }
    }
}
