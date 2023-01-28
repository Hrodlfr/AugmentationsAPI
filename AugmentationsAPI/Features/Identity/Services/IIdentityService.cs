namespace AugmentationsAPI.Features.Identity.Services
{
    public interface IIdentityService
    {
        /// <summary>
        /// Generates a JWT Token.
        /// </summary>
        /// <param name="userId"> The Id of the User. </param>
        /// <param name="userName"> The Name of the User. </param>
        /// <param name="secretKey"> This Applications Secret Key which is used to generate
        ///                          the Signature Section of the Token. </param>
        /// <returns> A JWT Token. </returns>

        string GenerateJwtToken(string userId, string userName, string secretKey);
    }
}
