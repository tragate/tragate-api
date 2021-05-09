using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Amazon.S3;
using Amazon.SimpleEmail;
using AutoMapper;
using Equinox.Infra.Data.EventSourcing;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Tragate.Application;
using Tragate.Application.AutoMapper;
using Tragate.Application.ServiceUtility.Login;
using Tragate.Common.Library;
using Tragate.Common.Library.Helpers;
using Tragate.CrossCutting.Bus;
using Tragate.Domain.CommandHandlers;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Events;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Core.Infrastructure.ExternalService;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.Interfaces;
using Tragate.Infra.Data;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;
using Tragate.Infra.Data.Repository.EventSourcing;
using Tragate.Infra.Data.UoW;
using Tragate.WebApi;

namespace Tragate.Tests
{
    public class TestBase
    {
        protected ServiceProvider BuildServiceProvider(){
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.json",
                    optional: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration["RedisUrl"];
                options.InstanceName = "tragate-api-";
            });

            services.AddOptions();
            services.AddAutoMapper();
            services.AddMediatR(typeof(Startup));
            services.AddSingleton(AutoMapperConfig.RegisterMappings());
            services.Configure<ConfigSettings>(configuration);

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<TragateContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<EventStoreSQLContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddSingleton(x =>
                new ElasticClient(new ConnectionSettings(new Uri(configuration["ElasticSearchUrl"]))));

            services.AddScoped<ILoginFactory, LoginFactory>();
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            services.AddSingleton<IEventStoreRepository, EventStoreSQLRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();


            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyAdminService, CompanyAdminService>();
            services.AddScoped<ICompanyNoteService, CompanyNoteService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<ICompanyMembershipService, CompanyMembershipService>();
            services.AddScoped<ICompanyNoteService, CompanyNoteService>();
            services.AddScoped<ICompanyTaskService, CompanyTaskService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IQuoteProductService, QuoteProductService>();
            services.AddScoped<IQuoteHistoryService, QuoteHistoryService>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyAdminRepository, CompanyAdminRepository>();
            services.AddScoped<ICompanyNoteRepository, CompanyNoteRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<ICompanyMembershipRepository, CompanyMembershipRepository>();
            services.AddScoped<ICompanyNoteRepository, CompanyNoteRepository>();
            services.AddScoped<ICompanyTaskRepository, CompanyTaskRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IQuoteProductRepository, QuoteProductRepository>();
            services.AddScoped<IQuoteHistoryRepository, QuoteHistoryRepository>();


            services.AddScoped<INotificationHandler<AddNewCategoryCommand>, CategoryCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateCategoryCommand>, CategoryCommandHandler>();
            services.AddScoped<INotificationHandler<UploadCategoryImageCommand>, CategoryCommandHandler>();
            services.AddScoped<INotificationHandler<RegisterNewCompanyCommand>, CompanyCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateCompanyCommand>, CompanyCommandHandler>();
            services.AddScoped<INotificationHandler<RemoveCompanyCommand>, CompanyCommandHandler>();
            services.AddScoped<INotificationHandler<AddNewProductCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateProductCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateStatusProductCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<DeleteProductCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateDefaultProductListImageCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<AddNewProductImageCommand>, ProductImageCommandHandler>();
            services.AddScoped<INotificationHandler<DeleteProductImageCommand>, ProductImageCommandHandler>();
            services.AddScoped<INotificationHandler<AddNewCompanyMembershipCommand>, CompanyMembershipCommandHandler>();
            services.AddScoped<INotificationHandler<AddNewCompanyNoteCommand>, CompanyNoteCommandHandler>();
            services.AddScoped<INotificationHandler<DeleteCompanyNoteCommand>, CompanyNoteCommandHandler>();
            services.AddScoped<INotificationHandler<AddNewCompanyTaskCommand>, CompanyTaskCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateStatusCompanyTaskCommand>, CompanyTaskCommandHandler>();
            services.AddScoped<INotificationHandler<DeleteCompanyTaskCommand>, CompanyTaskCommandHandler>();
            services.AddScoped<INotificationHandler<ChangeEmailCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<SendActivationEmailCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<AddNewQuoteCommand>, QuoteCommandHandler>();
            services.AddScoped<INotificationHandler<AddNewQuoteHistoryCommand>, QuoteHistoryCommandHandler>();

            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IRestClient, RestClient>();
            services.AddDefaultAWSOptions(configuration.GetAWSOptions()
                .GetConfig(configuration["AWS_Access_Key"], configuration["AWS_Secret_Key"]));
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSimpleEmailService>();

            services.AddScoped<IMapper>(sp =>
                new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            services.AddScoped<IDbConnection>(x =>
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            return services.BuildServiceProvider();
        }
    }
}