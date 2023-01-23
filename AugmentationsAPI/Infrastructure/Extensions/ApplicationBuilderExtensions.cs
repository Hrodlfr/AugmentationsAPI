namespace AugmentationsAPI.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Exception Handler Middleware to the Application Builder.
        /// </summary>
        /// <param name="app"> The Application Builder. </param>
        /// <returns> The <see cref="IApplicationBuilder"/> so that additional calls can be chained. </returns>
        public static IApplicationBuilder AddExceptionHandler(this IApplicationBuilder app)
        {
            // Add the Exception Handler Middleware with a Path that Points to the Error Controller
            return app.UseExceptionHandler("/error");
        }

        /// <summary>
        /// Adds Swagger Middleware for Serving Generated JSON Documents and Swagger UI.
        /// </summary>
        /// <param name="app"> The Application Builder. </param>
        /// <returns> The <see cref="IApplicationBuilder"/> so that additional calls can be chained. </returns>
        public static IApplicationBuilder AddSwagger(this IApplicationBuilder app)
        {
            return app
                // Enable Swagger Middleware for Serving Generated JSON Documents 
                .UseSwagger()
                // Enable Swagger Middleware for Serving the Swagger UI
                .UseSwaggerUI(options =>
                {
                    // Add a Swagger Endpoint
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    // Set the Route Prefix to an Empty String so that the Swagger UI will be Served at the App's Root
                    options.RoutePrefix = string.Empty;
                });
        }
    }
}
