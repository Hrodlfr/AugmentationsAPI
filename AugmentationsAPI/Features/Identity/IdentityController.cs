namespace AugmentationsAPI.Features.Identity
{
    using Models;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    [Route("[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly UserManager<Agent> userManager;
        private readonly IIdentityService identityService;
        private readonly JwtOptions jwtOptions;

        public IdentityController(UserManager<Agent> userManager,
            IIdentityService identityService,
            IOptions<JwtOptions> jwtOptions)
        {
            this.userManager = userManager;
            this.identityService = identityService;
            this.jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Attempts to Register an Agent.
        /// </summary>
        /// <param name="model"> The Model which contains both Required and Optional Information
        ///                      about the Agent Which is to be Registered. </param>
        /// <returns> An Action Result of Ok if the Registration is Successful
        ///           or An Action Result of Bad Request if the Registration is Unsuccessful. </returns>
        [HttpPost]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            // Initialize a New Agent and Assign to it the Values from the Model
            // TODO: Add an ORM
            var agent = new Agent
            {
                UserName = model.UserName
            };

            // Attempt to Register the Agent and Assign the Returned Value,
            // which indicates if the Registration was successful, to a Variable
            var result = await userManager.CreateAsync(agent, model.Password);

            // If the Result of the Registration was Successful...
            if (result.Succeeded)
            {
                // ...Return an Action Result of Ok
                return Ok();
            }

            // ...Else If the Result of the Registration was Unsuccessful...
            // ...Return an Action Result of Bad Request and The Errors which caused the Registration to Fail
            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Attempts to Login a User.
        /// </summary>
        /// <param name="model"> The Model which contains Required Information to Login the User. </param>
        /// <returns> A Model which contains the Agents JWT Token if the Login is Successful
        ///           or an Action Result of Unauthorized if the Login is Unsuccessful. </returns>
        [HttpPost]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            // Attempt to Find an Agent by the Given UserName
            // and Store the Result in a Variable
            var user = await userManager.FindByNameAsync(model.UserName);

            // If No Such User Exists...
            if (user == null)
            {
                // ...Return an Action Result of Unauthorized
                return Unauthorized();
            }
            // ...Else If a User Exists with the Same UserName...

            // Check if the Given Password Is the Password of the Existing User
            // and Store the Result in a Variable
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

            // If the Given Password is Invalid...
            if (!passwordValid)
            {
                // ...Return an Action Result of Unauthorized
                return Unauthorized();
            }
            // ...Else If the Given Password is Valid...

            // Generate a JWT Token and Store it in a Variable
            var token = identityService.GenerateJwtToken(user.Id, user.UserName, this.jwtOptions.Key);

            // Return a Model which contains the JWT Token of the Agent
            return new LoginResponseModel
            {
                Token = token
            };
        }
    }
}
