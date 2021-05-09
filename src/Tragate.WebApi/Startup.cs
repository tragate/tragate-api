using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Swashbuckle.AspNetCore.Swagger;
using Tragate.Common.Library;
using Tragate.Infra.CrossCutting.IoC;
using Tragate.WebApi.Configurations;
using Tragate.WebApi.Controllers;

namespace Tragate.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment HostingEnvironment { get; set; }

        public Startup(IHostingEnvironment env){
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json",
                    optional: false,
                    reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            HostingEnvironment = env;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Error()
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(
                        new Uri(Configuration["ElasticsearchUrl"]))
                    {
                        MinimumLogEventLevel = LogEventLevel.Error,
                        AutoRegisterTemplate = true
                    })
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services){
            sd.Fonksiyonlar.Ayarlar = Configuration;
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = Configuration["RedisUrl"];
                options.InstanceName = $"{HostingEnvironment.EnvironmentName}-tragate-api-";
            });

            services.AddWebApi(options =>
            {
                options.OutputFormatters.Remove(new XmlDataContractSerializerOutputFormatter());
            });
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            services.AddAutoMapper();

            services.AddCors(options => options
                .AddPolicy("AllowSpecificOrigin", p => p
                    .WithOrigins("tragate.com",
                        "http://yeni.tragate.com",
                        "http://test.tragate.com",
                        "http://admintest.tragate.com",
                        "http://prep.tragate.com",
                        "http://admin.tragate.com",
                        "localhost",
                        "http://127.0.0.1:52409",
                        "http://localhost:52409",
                        "http://127.0.0.1:5001",
                        "http://127.0.0.1:5003")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Tragate Web Api",
                    Description = "Tragate web api v1.0",
                    Contact = new Contact
                    {
                        Name = "tragate-tech",
                        Email = "info@tragate.com",
                        Url = "http://www.tragate.com"
                    }
                });
                if (HostingEnvironment.IsDevelopment()){
                    s.OperationFilter<FileUploadOperation>();
                }
            });

            // Adding MediatR for Domain Events and Notifications
            services.AddMediatR(typeof(Startup));
            services.Configure<ConfigSettings>(Configuration);

            // .NET Native DI Abstraction
            RegisterServices(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app){
            app.UseGlobalExceptionHandler();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(s => { s.SwaggerEndpoint("/swagger/v1/swagger.json", "Tragate Web API v1.0"); });
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration){
            // Adding dependencies from another layers (isolated from Presentation)
            NativeInjectorBootStrapper.RegisterServices(services, configuration);
        }
    }
}