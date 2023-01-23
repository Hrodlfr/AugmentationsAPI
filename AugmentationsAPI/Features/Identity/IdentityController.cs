namespace AugmentationsAPI.Features.Identity
{
    using Models;
    using Data.Models;
    using Mapster;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using static Infrastructure.Constants;

    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IIdentityService identityService;
        private readonly JwtOptions jwtOptions;

        public IdentityController(UserManager<User> userManager,
            IIdentityService identityService,
            IOptions<JwtOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.identityService = identityService;
            this.jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Attempts to Register a User.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST Identity/Register
        ///     {
        ///      "userName": "JCDenton", 
        ///      "password": "NanoAugmented"
        ///     {
        /// 
        /// </remarks>
        /// 
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Register the User. </param>
        ///                      
        /// <returns> Ok if the Registration is Successful
        ///           or BadRequest if the Registration is Unsuccessful. </returns>
        ///           
        /// <response code="200"> The User was Successfully Registered. </response>
        /// <response code="400"> The Users Registration was Unsuccessful. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            // Initialize a New User and Map the Request Models Values to It
            var user = model.Adapt<User>();

            // Attempt to Register the User and Assign the Returned Value,
            // which indicates if the Registration was successful, to a Variable
            var result = await userManager.CreateAsync(user, model.Password);

            // Return Ok If the Registration was Successful...
            // OR BadRequest with the Errors If the Registration was Unsuccessful 
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        /// <summary>
        /// Attempts to Login a User.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST Identity/Login
        ///     {
        ///      "userName": "JCDenton",
        ///      "password": "NanoAugmented"
        ///     }
        /// 
        /// </remarks>
        /// 
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Register the User. </param>
        /// 
        /// <returns> A Model which contains the Users JWT Token if the Login is Successful
        ///           or an Unauthorized if the Login is Unsuccessful. </returns>
        ///
        /// <response code="200"> The User is Successfully Logged In and Their Unique JWT Token was Returned. </response>
        /// <response code="401"> The Log In Was Unsuccessful Possibly due to a Wrong Password or Username. </response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Produces(ContentTypeApplicationJson)]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            // Attempt to Find a User by the Given UserName
            // and Store the Result in a Variable
            var user = await userManager.FindByNameAsync(model.UserName);

            // If No Such User Exists...
            if (user == null)
            {
                // ...Return Unauthorized
                return Unauthorized();
            }

            // Check if the Given Password Is the Password of the Existing User
            // and Store the Result in a Variable
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

            // If the Given Password is Invalid...
            if (!passwordValid)
            {
                // ...Return Unauthorized
                return Unauthorized();
            }

            // Generate a JWT Token and Store it in a Variable
            var token = identityService.GenerateJwtToken(user.Id, user.UserName!, jwtOptions.Key);

            // Return a Model which contains the JWT Token of the User
            return new LoginResponseModel
            {
                Token = token
            };
        }
    }
}
