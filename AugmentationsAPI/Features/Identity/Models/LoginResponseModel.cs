namespace AugmentationsAPI.Features.Identity.Models
{
    public class LoginResponseModel
    {
        /// <summary>
        /// The User's Unique JWT Token Which Will be used for Authentication.
        /// </summary>
        ///<example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.GVudG9uIiwCI6MTY3MTUzMjY3Mn0.N1DdkyPum8mCvILfujBkc</example>
        public string Token { get; set; }
    }
}
