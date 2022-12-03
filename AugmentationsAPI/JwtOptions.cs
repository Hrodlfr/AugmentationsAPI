namespace AugmentationsAPI
{
    public class JwtOptions
    {
        /// <summary>
        /// This Field is used so that this Fields Value doesn't need to be Hardcoded
        /// when this class is added to this Apps Services.
        /// </summary>
        public const string Jwt = "Jwt";

        /// <summary>
        /// This Field is used so that this Fields Value doesn't need to be Hardcoded
        /// when this class is added to this Apps Services.
        /// </summary>
        public const string JwtKey = "Key";

        /// <summary>
        /// The Secret Key used to create the Signature Part of a JWT Token.
        /// </summary>
        public string Key { get; set; } = string.Empty;
    }
}
