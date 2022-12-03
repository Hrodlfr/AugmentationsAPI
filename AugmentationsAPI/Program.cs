namespace AugmentationsAPI
{
    using Features.Identity;
    using Infrastructure.Extensions;
    using System.Text;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;

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
                // Inject Controller Services
                .AddControllers();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}