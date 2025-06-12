using Common.Configs;
using Common.Middleware;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Elastic.Apm.NetCoreAll;
using HealthChecks.UI.Client;
using technicaltest.Config;
using technicaltest.Repositories;
using technicaltest.UseCases;
using technicaltest.Protos;
using technicaltest.Repositories.MySql;
using technicaltest.Services;
using technicaltest.Repositories.Cache;
using FluentValidation;
using technicaltest.Models;
using technicaltest.Validators;
using Common.Email;
using Common.Interfaces;
using Common.Extensions;

namespace technicaltest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Setting for dapper
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
			
			services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();

            #region Register client api rest / grpc

            var policyConfigs = new HttpClientPolicyConfiguration();
            Configuration.Bind("HttpClientPolicies", policyConfigs);

            //services.AddRestClient<IClassRes, ClassRes>(Configuration["RestSettings:ProductUrl"], policyConfigs);
            //services.AddGrpcClient<ProductProtoService.ProductProtoServiceClient>(Configuration["GrpcSettings:ProductUrl"]);

            #endregion

            #region Redis Configuration
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
                {
                    EndPoints = { Configuration.GetValue<string>("CacheSettings:ConnectionString") },
                    DefaultDatabase = Configuration.GetValue<int>("CacheSettings:Database"),
					User = Configuration.GetValue<string>("CacheSettings:User"), 
                    Password = Configuration.GetValue<string>("CacheSettings:Password"), 
                    Ssl = false
                };
            });
            #endregion

            #region IOC Register
            services.AddScoped<IDbConnectionFactory>(_ => new Config.MySql.DbConnectionFactory(Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));
            
            //ResClient dan GRPC Client IOC tidak perlu ditambahkan

            //services.AddSingleton<IDriverPubSub, UpdateDriverPubSub>();
            			services.AddScoped<IProductDb, ProductDb>();
			services.AddScoped<IProductCache, ProductCache>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<IProductUseCase, ProductUseCase>();
			services.AddScoped<IValidator<Product>, ProductValidator>();



            #endregion

            services.AddAutoMapper(typeof(Startup));

            services.AddGrpc().AddJsonTranscoding();

            services.AddGrpcReflection();

            services.AddControllers();
            services.AddGrpcSwagger();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "technicaltest service", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddHealthChecks();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAllElasticApm(Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("./v1/swagger.json", "technicaltest service v1"));
            app.UseRouting();

            app.UseAuthorization();
            app.UseCustomResponseMiddleware();
            app.UseSSEMiddleware();

            app.UseEndpoints(endpoints =>
            {
                #region Service Register
                			endpoints.MapGrpcService<ProductService>();

                //endpoints.MapGrpcService<OrganizationService>();

                #endregion

                endpoints.MapGrpcReflectionService(); //  Focus!!!
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });

                endpoints.MapControllers();
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}