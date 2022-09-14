using ConfigurationService.Interfaces;
using ConfigurationService.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigurationService.Utility
{
    public class ServiceToScope
    {
        public ServiceToScope(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void AddToScope(IServiceCollection services)
        {
            services.AddTransient<ILocation>(s => new LocationService(Configuration.GetSection("ConnectionStrings:Connection").Value, Configuration));
            services.AddTransient<IOrganization>(s => new OrganizationService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<ILocationDepartment>(s => new LocationDepartmentService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<IOrganizationState>(s => new OrganizationStateService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<ILocationNetwork>(s => new LocationNetworkService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<ILocationDepartmentState>(s => new LocationDepartmentStateService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<ILocationDepartmentStateTime>(s => new LocationDepartmentStateTimeService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<ILocationDepartmentStateSetting>(s => new LocationDepartmentStateSettingService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<ILocationDepartmentNonOperational>(s => new LocationDepartmentNonOperationalService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            //  services.AddTransient<ILocationCode>(s => new LocationCodeService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            //  services.AddTransient<IOrganizationCode>(s => new OrganizationCodeService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<IDepartmentState>(s => new DepartmentStateService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<ISmsProvider>(s => new SmsProviderService(Configuration.GetSection("ConnectionStrings:Connection").Value));
            services.AddTransient<IPractitionerDepartment>(s => new PractitionerDepartmentService(Configuration.GetSection("ConnectionStrings:Connection").Value));
        }
    }
}
