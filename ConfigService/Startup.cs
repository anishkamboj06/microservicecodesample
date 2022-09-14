using ConfigurationService.Filters;
using ConfigurationService.Utility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Logging;
using Models;
using CommonLibrary.Utility;
using ConfigurationService.Services;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

namespace ConfigurationService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddLogging(config =>
            {
                config.AddConfiguration(Configuration.GetSection("Logging"));
                //  config.AddConsole();
                //  config.AddDebug();
                //  config.AddEventLog();

            }
            );

            services.Configure<KestrelServerOptions>(Configuration.GetSection("Kestrel"));

            // Set the Configuration Parameters in Static Class
            ConnectionSetting.SetSetting(Configuration);

            services.AddControllers();
            services.AddHealthChecks();

            services.AddApplicationInsightsTelemetry();
            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                //   x.AssumeDefaultVersionWhenUnspecified = true;
                x.ReportApiVersions = true;
            });

            //Configure Redis Distribution cache for storing the cache in Redis
            //var configString = $"{ConnectionSetting.RedisURL}:{ConnectionSetting.RedisPort},connectRetry=5";
            //services.AddDistributedRedisCache(
            //options =>
            //{
            //    options.Configuration = configString;
            //}
            //);

            // add authentication sever 
            services.AddAuthentication("Bearer")
           .AddIdentityServerAuthentication("Bearer", options =>
           {
               options.ApiName = ConnectionSetting.APIName;
               options.Authority = ConnectionSetting.IdentityServerURL;
               options.JwtValidationClockSkew = TimeSpan.FromMinutes(0);
               options.RequireHttpsMetadata = false;
           });

         //   services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            //Add Swagger Custom Documentation with Authorize the header functionality
            services.AddSwaggerDocumentation();

            // Add check model custom validation filter
            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ValidateModelFilter));
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });


            services.AddCors();
            //  Add Scoped service
            ServiceToScope oServiceToScope = new ServiceToScope(Configuration);
            oServiceToScope.AddToScope(services);
            services.AddGrpc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //string webRootPath = env.ContentRootPath;
            //loggerFactory.AddFile(webRootPath + "/log/InformationLogs/InformationLogs.txt", LogLevel.Information);
            //loggerFactory.AddFile(webRootPath + "/log/WarningLogs/WarningLogs.txt", LogLevel.Warning);
            //loggerFactory.AddFile(webRootPath + "/log/ErrorLogs/ErrorLogs.txt", LogLevel.Error);
            //loggerFactory.AddFile(webRootPath + "/log/CriticalLogs/CriticalLogs.txt", LogLevel.Critical);



            //healthCheck
            app.UseHealthChecks(path: "/", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });


            //for documentation
            app.UseSwaggerDocumentation();
            app.UseRouting();
            app.UseAuthentication();
            app.UseCors(
          options => options.WithOrigins("http://esanjeevaniopd.in/")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()
           );
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<gRPCServices>();
                endpoints.MapGrpcService<gRPCLocationDepartment>();
                endpoints.MapGrpcService<gRPCLocation>();
                endpoints.MapControllers();
            });
        }
    }
}
