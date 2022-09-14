using ConfigurationService.ProtosLocation;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ConfigurationService.Interfaces;
using Models;
using Newtonsoft.Json;

namespace ConfigurationService.Services
{
    public class gRPCLocation : LocationProtoService.LocationProtoServiceBase
    {
        private readonly ILocation _srvILocation;
        private readonly ILogger _logger;
        public gRPCLocation(ILocation srvILocation, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<gRPCLocation>();
            _srvILocation = srvILocation;
        }
        public override async Task<Reply> GetLocationByUId(Request request, ServerCallContext context)
        {
            var location = await _srvILocation.GetLocationForConsultation(null, request.LocationUId);
            string jsonData = JsonConvert.SerializeObject(location.Model);
            return await Task.FromResult(new Reply
            {
                Response = jsonData
            }) ; 
        }
    }
}
