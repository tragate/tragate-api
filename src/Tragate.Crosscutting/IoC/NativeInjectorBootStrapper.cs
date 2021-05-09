using System;
using System.Data;
using System.Data.SqlClient;
using Amazon.S3;
using Amazon.SimpleEmail;
using AutoMapper;
using Equinox.Infra.Data.EventSourcing;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Tragate.Application;
using Tragate.Application.ServiceUtility.Login;
using Tragate.Application.ServiceUtility.Search;
using Tragate.Common.Library.Constants;
using Tragate.Common.Library.Dto;
using Tragate.Common.Library.Helpers;
using Tragate.CrossCutting.Bus;
using Tragate.CrossCutting.Filters;
using Tragate.Domain.CommandHandlers;
using Tragate.Domain.Commands;
using Tragate.Domain.Core.Bus;
using Tragate.Domain.Core.Events;
using Tragate.Domain.Core.Infrastructure;
using Tragate.Domain.Core.Infrastructure.ExternalService;
using Tragate.Domain.Core.Notifications;
using Tragate.Domain.EventHandlers;
using Tragate.Domain.Events;
using Tragate.Domain.Events.CompanyData;
using Tragate.Domain.Events.Image;
using Tragate.Domain.Events.ProductImage;
using Tragate.Domain.Interfaces;
using Tragate.Domain.Interfaces.Email;
using Tragate.Infra.Data;
using Tragate.Infra.Data.Context;
using Tragate.Infra.Data.Repository;
using Tragate.Infra.Data.Repository.EventSourcing;
using Tragate.Infra.Data.UoW;

namespace Tragate.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        private static IConfiguration _configuration { get; set; }

        public static void RegisterServices(IServiceCollection services, IConfiguration configuration){
            _configuration = configuration;

            // External Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IMediatorHandler, InMemoryBus>();
            services.AddSingleton<EmailHandler>();
            services.AddSingleton(x =>
                new ElasticClient(new ConnectionSettings(new Uri(_configuration["ElasticSearchUrl"]))
                    .DefaultMappingFor<Root>(m =>
                        m.TypeName(TragateConstants.ROOT_TYPE))
                    .DefaultMappingFor<CompanySearchDto>(m =>
                        m.TypeName(TragateConstants.ROOT_TYPE))
                    .DefaultMappingFor<SearchAllDto>(m =>
                        m.TypeName(TragateConstants.ROOT_TYPE))
                    .DefaultMappingFor<ProductDto>(m =>
                        m.TypeName(TragateConstants.ROOT_TYPE)
                            .IdProperty(p => p.UuId))
                    .DefaultMappingFor<CompanyDto>(m =>
                        m.RelationName(TragateConstants.PARENT_TYPE))));

            services.AddScoped<IDbConnection>(x =>
                new SqlConnection(_configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultAWSOptions(_configuration.GetAWSOptions()
                .GetConfig(_configuration["AWS_Access_Key"], _configuration["AWS_Secret_Key"]));

            // Domain - Events
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            //User
            services.AddScoped<INotificationHandler<UserRegisteredEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<UserForgotPasswordEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<UserImageUploadedEvent>, UserEventHandler>();
            services.AddScoped<INotificationHandler<AnonymUserCreatedEvent>, UserEventHandler>();

            //Company
            services.AddScoped<INotificationHandler<CompanyRegisteredEvent>, CompanyEventHandler>();
            services.AddScoped<INotificationHandler<CompanyUpdatedEvent>, CompanyEventHandler>();
            services.AddScoped<INotificationHandler<CompanyFastAddedEvent>, CompanyEventHandler>();

            //CompanyMembership
            services.AddScoped<INotificationHandler<CompanyMembershipCreatedEvent>, CompanyMembershipEventHandler>();

            //CompanyData
            services.AddScoped<INotificationHandler<CompanyDataReferenceUpdatedEvent>, CompanyDataEventHandler>();

            //Category
            services.AddScoped<INotificationHandler<CategoryImageUploadedEvent>, CategoryEventHandler>();

            //Product
            services.AddScoped<INotificationHandler<ProductRegisteredEvent>, ProductEventHandler>();
            services.AddScoped<INotificationHandler<ProductUpdatedEvent>, ProductEventHandler>();

            //Image
            services.AddScoped<INotificationHandler<ImageDeletedEvent>, ImageEventHandler>();

            //Product - Image
            services.AddScoped<INotificationHandler<ProductImageUploadedEvent>, ProductImageEventHandler>();
            services.AddScoped<INotificationHandler<ProductImageDeletedEvent>, ProductImageEventHandler>();

            //Quote History
            services.AddScoped<INotificationHandler<QuoteHistoryCreatedEvent>, QuoteHistoryEventHandler>();

            //EmailHandler
            services.AddScoped<INotificationHandler<EmailSentEvent>, EmailHandler>();

            // Application
            services.AddTransient<IRestClient, RestClient>();
            services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp =>
                new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
            services.AddScoped<IContentService, ContentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyMembershipService, CompanyMembershipService>();
            services.AddScoped<ICompanyAdminService, CompanyAdminService>();
            services.AddScoped<ICompanyNoteService, CompanyNoteService>();
            services.AddScoped<ICompanyTaskService, CompanyTaskService>();
            services.AddScoped<ICompanyDataService, CompanyDataService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IQuoteHistoryService, QuoteHistoryService>();
            services.AddScoped<IQuoteProductService, QuoteProductService>();
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<ILoginFactory, LoginFactory>();

            // Domain - Commands


            //Mailing
            services.AddScoped<INotificationHandler<SendNewMailCommand>, MailCommandHandler>();

            //User
            services.AddScoped<INotificationHandler<RegisterNewPersonCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateUserEmailVerifyCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<ForgotPasswordCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<ResetPasswordCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<UploadImageCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<ChangePasswordCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<ChangeEmailCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateUserCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<SendActivationEmailCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<CompleteSignUpCommand>, UserCommandHandler>();
            services.AddScoped<INotificationHandler<RegisterNewExternalUserCommand>, UserCommandHandler>();

            //CompanyAdmin
            services.AddScoped<INotificationHandler<AddNewCompanyAdminCommand>, CompanyAdminCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateCompanyAdminCommand>, CompanyAdminCommandHandler>();
            services.AddScoped<INotificationHandler<RemoveCompanyAdminCommand>, CompanyAdminCommandHandler>();

            //Company
            services.AddScoped<INotificationHandler<RegisterNewCompanyCommand>, CompanyCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateCompanyCommand>, CompanyCommandHandler>();
            services.AddScoped<INotificationHandler<RemoveCompanyCommand>, CompanyCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateCompanyMembershipCommand>, CompanyCommandHandler>();
            services.AddScoped<INotificationHandler<RegisterFastNewCompanyCommand>, CompanyCommandHandler>();

            //Company Membership
            services.AddScoped<INotificationHandler<AddNewCompanyMembershipCommand>, CompanyMembershipCommandHandler>();

            //Company Note
            services.AddScoped<INotificationHandler<AddNewCompanyNoteCommand>, CompanyNoteCommandHandler>();
            services.AddScoped<INotificationHandler<DeleteCompanyNoteCommand>, CompanyNoteCommandHandler>();

            //Company Note
            services.AddScoped<INotificationHandler<AddNewCompanyTaskCommand>, CompanyTaskCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateStatusCompanyTaskCommand>, CompanyTaskCommandHandler>();
            services.AddScoped<INotificationHandler<DeleteCompanyTaskCommand>, CompanyTaskCommandHandler>();

            //CompanyData
            services.AddScoped<INotificationHandler<UpdateCompanyDataCommand>, CompanyDataCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateReferenceCompanyDataCommand>, CompanyDataCommandHandler>();

            //Category
            services.AddScoped<INotificationHandler<AddNewCategoryCommand>, CategoryCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateCategoryCommand>, CategoryCommandHandler>();
            services.AddScoped<INotificationHandler<UploadCategoryImageCommand>, CategoryCommandHandler>();

            //Content
            services.AddScoped<INotificationHandler<AddNewContentCommand>, ContentCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateContentCommand>, ContentCommandHandler>();

            //Product
            services.AddScoped<INotificationHandler<AddNewProductCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateProductCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<DeleteProductCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateDefaultProductListImageCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateProductListImageCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateStatusProductCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<RemoveProductListImageCommand>, ProductCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateCategoryProductCommand>, ProductCommandHandler>();

            //ProductImage
            services.AddScoped<INotificationHandler<AddNewProductImageCommand>, ProductImageCommandHandler>();
            services.AddScoped<INotificationHandler<DeleteProductImageCommand>, ProductImageCommandHandler>();

            //Quote
            services.AddScoped<INotificationHandler<AddNewQuoteCommand>, QuoteCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateQuoteContactStatusCommand>, QuoteCommandHandler>();
            services.AddScoped<INotificationHandler<UpdatedDateQuoteCommand>, QuoteCommandHandler>();
            services.AddScoped<INotificationHandler<UpdateQuoteStatusCommand>, QuoteCommandHandler>();

            //QuoteHistory
            services.AddScoped<INotificationHandler<AddNewQuoteHistoryCommand>, QuoteHistoryCommandHandler>();


            // Infra - Data
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<TragateContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICompanyAdminRepository, CompanyAdminRepository>();
            services.AddScoped<ICompanyDataRepository, CompanyDataRepository>();
            services.AddScoped<ICompanyMembershipRepository, CompanyMembershipRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IParameterRepository, ParameterRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IContentRepository, ContentRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<ICompanyNoteRepository, CompanyNoteRepository>();
            services.AddScoped<ICompanyTaskRepository, CompanyTaskRepository>();
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.AddScoped<IQuoteProductRepository, QuoteProductRepository>();
            services.AddScoped<IQuoteHistoryRepository, QuoteHistoryRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();

            // Infra - Data EventSourcing
            services.AddScoped<IEventStoreRepository, EventStoreSQLRepository>();
            services.AddScoped<IEventStore, SqlEventStore>();
            services.AddDbContext<EventStoreSQLContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            //Infra
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IQueryBuilder, QueryBuilder>();
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonSimpleEmailService>();

            services.AddScoped<AnonymUserValidationFilterAttribute>();
            services.AddScoped<ExternalUserValidationFilterAttribute>();
        }
    }
}