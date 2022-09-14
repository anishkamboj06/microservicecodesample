using ConfigurationService.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ConfigurationService.Interfaces;
namespace ConfigurationService.Services
{
    public class gRPCServices : OrganizationProtoService.OrganizationProtoServiceBase
    {
        private readonly IOrganization _srvIOrganization;
        private readonly ILogger _logger;

        public gRPCServices(IOrganization srvOrganization, ILoggerFactory logFactory)
        {
            _logger = logFactory.CreateLogger<gRPCServices>();
            _srvIOrganization = srvOrganization;
        }
        public override async Task<Reply> getGovernedByCount(Request request, ServerCallContext context)
        {           
            return await Task.FromResult(new Reply
            {                
               Response = _srvIOrganization.GetGovernedByCount(null,request.Name).Result
            });
        }
        public override async Task<Reply> getOrganizationTypeCount(Request request, ServerCallContext context)
        {
            return await Task.FromResult(new Reply
            {
                Response = _srvIOrganization.GetOrganizationTypeCount(null, request.Name).Result
            });
        }
    }
}
