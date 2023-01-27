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
        /// Registers a User.
        /// </summary>
        /// 
        /// <remarks>
        /// Sample Request:
        /// 
        ///     POST Identity/Register
        ///     {
        ///      "userName": "JCDenton", 
        ///      "password": "NanoAugmented"
        ///     }
        /// </remarks>
        /// 
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Register the User. </param>
        ///                      
        /// <returns> An Action Result of Ok. </returns>
        ///           
        /// <response code="200"> Returns Ok Indicating that the User was Registered Successfully. </response>
        /// <response code="400"> If the Registration Data Isn't Valid. </response>
        [HttpPost(nameof(Register))]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// Logs in a User.
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
        /// </remarks>
        /// 
        /// <param name="model"> A Data Transfer Object whose Data will be Used to Register the User. </param>
        /// 
        /// <returns> The Users JWT Token. </returns>
        ///
        /// <response code="200"> Returns the Users JWT Token. </response>
        /// <response code="401"> If the Login Credentials aren't Valid. </response>
        [HttpPost(nameof(Login))]
        [Consumes(ContentTypeApplicationJson)]
        [Produces(ContentTypeApplicationJson)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
