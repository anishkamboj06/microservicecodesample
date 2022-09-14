using ConfigurationService.Protos.LocationDepartment;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ConfigurationService.Interfaces;
namespace ConfigurationService.Services
{
    public class gRPCLocationDepartment : LocationDepartmentProtoService.LocationDepartmentProtoServiceBase
    {
        private readonly ILocationDepartment _srvILocationDepartment;
        private readonly ILogger _logger;

        public gRPCLocationDepartment(ILocationDepartment srvILocationDepartment, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<gRPCLocationDepartment>();
            _srvILocationDepartment = srvILocationDepartment;
        }
        public override async Task<Reply> UnmapLocDeptPrac(Request request, ServerCallContext context)
        {
            return await Task.FromResult(new Reply
            {
                Response = _srvILocationDepartment.UnmapLocDeptPrac(null, request.Name).Result
            });
        }
    }
}
