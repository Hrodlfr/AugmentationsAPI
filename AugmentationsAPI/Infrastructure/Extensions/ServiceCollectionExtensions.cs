namespace AugmentationsAPI.Infrastructure.Extensions
{
    using AugmentationsAPI.Features.Identity;
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.OpenApi.Models;
    using System.Text;

    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Injects this Applications Database Context to the Collection of Services.
        /// </summary>
        /// <param name="configuration"> A Collection of the Applicaitons Configuration Providers. </param>
        /// <returns> The <see cref="IServiceCollection"/> so that additional calls can be chained. </returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Using the Default Connection String to Inject the Applications Database Context
            // to the Collection of Services and Return Them
            return services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(configuration.GetDefaultConnectionString()));
        }

        /// <summary>
        /// Injects the Identity Services with a Custom User Class to the Collection of Services.
        /// </summary>
        /// <returns> The <see cref="IServiceCollection"/> so that additional calls can be chained. </returns>
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
        {
            services
                // Inject the Identity Services with a Custom User Class
                .AddIdentity<Agent, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Return the Collection of Services
            return services;
        }

        /// <summary>
        /// Injects Configuration Settings to the Collection of Services.
        /// </summary>
        /// <param name="configuration"> A Collection of the Applicaitons Configuration Providers. </param>
        /// <returns> The <see cref="IServiceCollection"/> so that additional calls can be chained. </returns>
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Inject the JWT Section of the App Settings to Services
            // so that the Secret Key contained in the Section can be used to Generate JWT Tokens
            services.Configure<JwtOptions>(
                configuration.GetSection(JwtOptions.Jwt));

            // Return the Collection of Services
            return services;
        }

        /// <summary>
        /// Injects Jwt Authentication Services to the Collection of Services.
        /// </summary>
        /// <returns> The <see cref="IServiceCollection"/> so that additional calls can be chained. </returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            // Encode the characters of the Secret Key into a Sequence of Bytes
            // and Store it in a Variable
            var securityKey = Encoding.ASCII.GetBytes(configuration.GetSection(JwtOptions.Jwt)[JwtOptions.JwtKey]);

            // Inject the Jwt Authentication Services
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // Return the Collection of Services
            return services;
        }

        /// <summary>
        /// Injects this Application's Services to the Collection of Services.
        /// </summary>
        /// <returns> The <see cref="IServiceCollection"/> so that additional calls can be chained. </returns>
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services
                // Inject the Identity Service
                .AddTransient<IIdentityService, IdentityService>();

            // Return the Collection of Services
            return services;
        }

        /// <summary>
        /// Injects a Swagger Generator to the Collection of Services.
        /// </summary>
        /// <returns> The <see cref="IServiceCollection"/> so that additional calls can be chained. </returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            // Inject a Swagger Generator
            services.AddSwaggerGen(optioins =>
            {
                // Define a Swagger Document
                optioins.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AugmentationsAPI",
                    Version = "v1",
                    Description = "An API about Deus Ex's Augmentations",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Ali Atanasov",
                        Email = "theumbralpyre@gmail.com",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "License",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });

            // Return the Collection of Services
            return services;
        }
    }
}
