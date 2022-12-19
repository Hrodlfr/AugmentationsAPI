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
               // Enable Swagger Middleware for Serving Generated JSON Documents 
               .UseSwagger()
               // Enable Swagger Middleware for Serving the Swagger UI
               .UseSwaggerUI(options =>
               {
                   // Add a Swagger Endpoint
                   options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                   // Set the Route Prefix to an Empty String so that the Swagger UI will be Served at the App's Root
                   options.RoutePrefix = string.Empty;
               })
               // Enable Authentication Middleware
               .UseAuthentication()
               // Enable Authorization Middleware
               .UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}