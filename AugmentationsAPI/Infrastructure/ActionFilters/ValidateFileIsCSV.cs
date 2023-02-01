namespace AugmentationsAPI.Infrastructure.ActionFilters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    /// <summary>
    /// An Action Filter which Checks Whether the Uploaded File is a CSV File or Not.
    /// </summary>
    /// <remarks>
    /// The Validation Occurs when the Action is Executing.
    /// If the File Isn't an CSV file Returns a Result of BadRquest.
    /// </remarks>
    public class ValidateFileIsCSV : IActionFilter
    {
        /// <summary>
        /// Checks Whether the Uploaded File is a CSV File or Not.
        /// </summary>
        /// <param name="context"> The Context of the Executing Action. </param>
        /// <return> A Result of BadRequest If the File Isn't a CSV File. </return>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Get the File
            var file = (IFormFile)context.ActionArguments.ElementAt(0).Value!;

            // If the File Isn't a CSV File...
            if (Path.GetExtension(file.FileName).ToLower() != ".csv")
            {
                // Set the Result of the Action to a BadRequest Result
                context.Result = new BadRequestResult();

                // Return
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }

}
