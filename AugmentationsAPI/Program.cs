namespace AugmentationsAPI
{
    using Infrastructure.Extensions;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Inject services to the Collection of Services.
            builder.Services
                // Inject the Database
                .AddDatabase(builder.Configuration)
                // Inject Custom Identity Services
                .AddCustomIdentity()
                // Inject Configuration Settings
                .AddConfigurationSettings(builder.Configuration)
                // Inject Jwt Authentication
                .AddJwtAuthentication(builder.Configuration)
                // Inject Application Services
                .AddApplicationService()
                // Inject Swagger
                .AddSwagger()
                // Inject Controller Services
                .AddControllers();
            
            var app = builder.Build();

            app
                // Add Swagger Middleware
                .AddSwagger()
                // Add Authentication Middleware
                .UseAuthentication()
                // Add Authorization Middleware
                .UseAuthorization()
                // Add Exception Handler Middleware
                .AddExceptionHandler();

            app.MapControllers();
            app.Run();
        }
    }
}