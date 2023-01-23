namespace AugmentationsAPI.Features.Error
{
    using Microsoft.AspNetCore.Mvc;

    public class ErrorController : ControllerBase
    {
        /// <summary>
        /// Returns an Internal Server Error.
        /// </summary>
        /// <returns> An Internal Server Error. </returns>
        [Route("/error")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleError() =>
            Problem(title: "Internal Server Error", statusCode: StatusCodes.Status500InternalServerError);
    }
}