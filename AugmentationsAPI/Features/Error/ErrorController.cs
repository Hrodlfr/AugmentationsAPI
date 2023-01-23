namespace AugmentationsAPI.Features.Error
{
    using Microsoft.AspNetCore.Mvc;
    using static Infrastructure.Constants;

    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Returns an Internal Server Error.
        /// </summary>
        /// <returns> An Internal Server Error. </returns>
        [Route(RouteError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() =>
            Problem(title: "Internal Server Error", statusCode: StatusCodes.Status500InternalServerError);
    }
}