namespace AugmentationsAPI.Features.Identity
{
    using System.Text;
    using System.Security.Claims;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;

    public class IdentityService : IIdentityService
    {
        /// <summary>
        /// Generates a JWT Token.
        /// </summary>
        /// <param name="userId"> The Id of the User. </param>
        /// <param name="userName"> The Name of the User. </param>
        /// <param name="secretKey"> This Applications Secret Key which is used to generate
        ///                          the Signature Section of the Token. </param>
        /// <returns> A JWT Token. </returns>
        public string GenerateJwtToken(string userId, string userName, string secretKey)
        {
            // Initialize a new JWT Token Handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Encode the characters of the Secret Key into a Sequence of Bytes
            // and Store it in a Variable
            var securityKey = Encoding.ASCII.GetBytes(secretKey);

            // Initialize a New Token Descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create the Token and Store it in a Variable
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Serialize the Token
            var encryptedToken = tokenHandler.WriteToken(token);

            // Return the Token
            return encryptedToken;
        }
    }
}
