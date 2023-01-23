namespace AugmentationsAPI.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Exception Handler Middleware to the Application Builder.
        /// </summary>
        /// <returns> The <see cref="IApplicationBuilder"/> so that additional calls can be chained. </returns>
        public static IApplicationBuilder AddExceptionHandler(this IApplicationBuilder app)
        {
            // Add the Exception Handler Middleware with a Path that Points to the Error Controller
            return app.UseExceptionHandler("/error");
        }
    }
}
