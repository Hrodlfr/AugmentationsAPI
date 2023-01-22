namespace AugmentationsAPI.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Returns the Default Connection String.
        /// </summary>
        /// <returns> The Default Connection String. </returns>
        public static string GetDefaultConnectionString(this IConfiguration configuration)
            => configuration.GetConnectionString("DefaultConnection")!;
    }
}
